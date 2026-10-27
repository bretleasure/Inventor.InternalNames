using ExtractInternalNames.Core;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace ExtractInternalNames.Core.Tests;

public sealed class GenerationTests
{
    [Fact]
    public void CatalogPreservesEveryExistingPublicConstantAndGeneratedCodeCompiles()
    {
        var catalog = NameCatalog.Load(Path.Combine(AppContext.BaseDirectory, "Names.xml"));
        var baseline = new SortedDictionary<string, string>(StringComparer.Ordinal);
        foreach (var file in Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "Baseline"), "*.cs", SearchOption.AllDirectories))
            ReadConstants(File.ReadAllText(file), baseline);
        var generated = new SortedDictionary<string, string>(StringComparer.Ordinal);
        foreach (var source in SourceGenerator.Generate(catalog).Values) ReadConstants(source, generated);
        Assert.True(baseline.Count > 4000);
        Assert.Equal(baseline.ToArray(), generated.ToArray());
    }

    private static void ReadConstants(string source, IDictionary<string, string> constants)
    {
        var root = CSharpSyntaxTree.ParseText(source).GetRoot();
        foreach (var field in root.DescendantNodes().OfType<FieldDeclarationSyntax>())
        {
            var ns = field.Ancestors().OfType<BaseNamespaceDeclarationSyntax>().Single().Name.ToString();
            var type = string.Join(".", field.Ancestors().OfType<ClassDeclarationSyntax>().Reverse().Select(c => c.Identifier.ValueText));
            foreach (var variable in field.Declaration.Variables)
                constants.Add(ns + "." + type + "." + variable.Identifier.ValueText,
                    ((LiteralExpressionSyntax)variable.Initializer!.Value).Token.ValueText);
        }
    }

    [Fact]
    public void MergePreservesAliasesAndMissingEntriesAndIsRepeatable()
    {
        var baseline = Baseline();
        baseline.Groups[0].Names.Add(new NameEntry { Name = "Alias", Value = "original" });
        baseline.Groups[0].Names.Add(new NameEntry { Name = "Absent", Value = "absent", Key = "absent" });
        var incoming = Group();
        incoming.Names.Add(new NameEntry { Name = "RenamedDisplay", Value = "original", Key = "id-1" });
        incoming.Names.Add(new NameEntry { Name = "new name", Value = "new", Key = "id-2" });
        var first = CatalogMerger.Merge(baseline, new[] { incoming }, out var summary);
        Assert.Equal(1, summary.Added);
        Assert.Equal(1, summary.Retained);
        Assert.Contains(first.Groups[0].Names, n => n.Name == "Original" && n.Key == "id-1");
        Assert.Contains(first.Groups[0].Names, n => n.Name == "Alias" && n.Key == "id-1");
        incoming.Names.Reverse();
        var second = CatalogMerger.Merge(first, new[] { incoming }, out var secondSummary);
        Assert.Equal(0, secondSummary.Added);
        Assert.Equal(first.Serialize(), second.Serialize());
        Assert.Equal(SourceGenerator.Generate(first).ToArray(), SourceGenerator.Generate(second).ToArray());
        Assert.Null(baseline.Groups[0].Names[0].Key);
    }

    [Fact]
    public void ConflictingValuesFailWithoutChangingBaseline()
    {
        var baseline = Baseline();
        baseline.Groups[0].Names[0].Key = "stable";
        var before = baseline.Serialize();
        var incoming = Group();
        incoming.Names.Add(new NameEntry { Name = "Original", Value = "changed", Key = "stable" });
        Assert.Throws<InvalidDataException>(() => CatalogMerger.Merge(baseline, new[] { incoming }, out _));
        Assert.Equal(before, baseline.Serialize());
    }

    [Fact]
    public void DuplicateLabelsKeywordsAndEscapedValuesProduceValidRepeatableCode()
    {
        var incoming = Group();
        incoming.Names.Add(new NameEntry { Name = "Original", Key = "new-id", Value = "quote\" slash\\ newline\n\u0000" });
        incoming.Names.Add(new NameEntry { Name = "class", Key = "keyword", Value = "keyword" });
        incoming.Names.Add(new NameEntry { Name = "123", Key = "digit", Value = "digit" });
        incoming.Names.Add(new NameEntry { Name = "!!!", Key = "empty", Value = "empty" });
        var merged = CatalogMerger.Merge(Baseline(), new[] { incoming }, out _);
        Assert.Contains(merged.Groups[0].Names, n => n.Name.StartsWith("Original_", StringComparison.Ordinal));
        Assert.Contains(merged.Groups[0].Names, n => n.Name == "_class");
        SourceGenerator.Generate(merged);
        incoming.Names.Reverse();
        var reordered = CatalogMerger.Merge(Baseline(), new[] { incoming }, out _);
        Assert.Equal(merged.Serialize(), reordered.Serialize());
    }

    [Fact]
    public void InvalidGeneratedMemberConflictsAreCaughtBeforeWriting()
    {
        var catalog = Baseline();
        catalog.Groups[0].Names.Add(new NameEntry { Name = "Example", Value = "type-name" });
        Assert.Throws<InvalidDataException>(() => SourceGenerator.Generate(catalog));
    }

    [Fact]
    public void CatalogRoundTripsControlCharactersAndAliases()
    {
        var catalog = Baseline();
        catalog.Groups[0].Names.Add(new NameEntry { Name = "Escaped", Value = "a\u0000\n\t\"\\\uD83D\uDE00", Key = "key\u0000" });
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, catalog.Serialize());
            var loaded = NameCatalog.Load(path);
            Assert.Equal(catalog.Groups[0].Names[1].Value, loaded.Groups[0].Names.Single(n => n.Name == "Escaped").Value);
            Assert.Equal(catalog.Serialize(), loaded.Serialize());
            SourceGenerator.Generate(loaded);
        }
        finally { File.Delete(path); }
    }

    [Fact]
    public void CategorySelectionDoesNotGenerateUnselectedFiles()
    {
        var catalog = Baseline();
        catalog.Groups.Add(new NameGroup { Category = "other", Key = "Other", File = "Other.cs",
            Namespace = "Example.Names", Class = "Other" });
        var generated = SourceGenerator.Generate(catalog, new HashSet<string> { "test" });
        Assert.Equal(new[] { "Example.cs" }, generated.Keys);
    }

    [Theory]
    [InlineData("../escape.cs")]
    [InlineData("/escape.cs")]
    [InlineData("C:\\escape.cs")]
    [InlineData("a/../escape.cs")]
    [InlineData("a\\escape.cs")]
    public void CatalogRejectsUnsafePaths(string path)
    {
        var catalog = Baseline();
        catalog.Groups[0].File = path;
        Assert.Throws<InvalidDataException>(catalog.Validate);
    }

    [Fact]
    public void FailedFileApplicationRestoresOriginalBytesAndRemovesStagingFiles()
    {
        var directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            var original = Path.Combine(directory, "a.cs");
            var blocked = Path.Combine(directory, "z.cs");
            var bytes = new byte[] { 239, 187, 191, 65, 13, 10 };
            File.WriteAllBytes(original, bytes);
            Directory.CreateDirectory(blocked);
            var files = new SortedDictionary<string, string> { [original] = "changed", [blocked] = "blocked" };
            Assert.ThrowsAny<IOException>(() => FileTransaction.Apply(files));
            Assert.Equal(bytes, File.ReadAllBytes(original));
            Assert.Empty(Directory.GetFiles(directory, ".names-*"));
        }
        finally { Directory.Delete(directory, recursive: true); }
    }

    [Fact]
    public void UnchangedFilesAreNotRewritten()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, "same");
            File.SetLastWriteTimeUtc(path, new DateTime(2020, 1, 1));
            var files = new Dictionary<string, string> { [path] = "same" };
            Assert.Empty(FileTransaction.ChangedFiles(files));
            FileTransaction.Apply(files);
            Assert.Equal(new DateTime(2020, 1, 1), File.GetLastWriteTimeUtc(path));
        }
        finally { File.Delete(path); }
    }

    private static NameGroup Group() => new()
    {
        Category = "test", Key = "Example", File = "Example.cs", Namespace = "Example.Names", Class = "Example"
    };

    private static NameCatalog Baseline()
    {
        var catalog = new NameCatalog();
        var group = Group();
        group.Names.Add(new NameEntry { Name = "Original", Value = "original" });
        catalog.Groups.Add(group);
        return catalog;
    }
}
