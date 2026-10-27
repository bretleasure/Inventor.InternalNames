using System.Diagnostics;
using System.CommandLine;
using ExtractInternalNames.Core;
using ExtractInternalNames.Extractors;

namespace ExtractInternalNames;

internal static class Program
{
    [STAThread]
    private static int Main(string[] args)
    {
        try
        {
            var command = ExtractorCommand.Create(Run, ExtractorRegistry.Create().Select(e => e.Id));
            return command.Parse(args).Invoke(new InvocationConfiguration { EnableDefaultExceptionHandler = false });
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine("Extraction failed: " + exception.Message);
            return 1;
        }
    }

    private static int Run(CommandLineOptions options)
    {
        var extractors = ExtractorRegistry.Create();
        if (options.List)
        {
            foreach (var extractor in extractors) Console.WriteLine($"{extractor.Id,-20} {extractor.Description}");
            return 0;
        }
        var selected = options.Only ?? new HashSet<string>(extractors.Select(e => e.Id), StringComparer.Ordinal);
        var repository = CommandLineOptions.FindRepository(Directory.GetCurrentDirectory()) ??
                         CommandLineOptions.FindRepository(AppContext.BaseDirectory);
        var output = options.Output ?? (repository == null ? null : Path.Combine(repository, "src", "Inventor.InternalNames"));
        if (output == null)
        {
            throw new ArgumentException("Run inside the repository or specify --output PATH.");
        }

        var catalogPath = options.Catalog ?? (repository == null ? Path.Combine(AppContext.BaseDirectory, "Names.xml") :
            Path.Combine(repository, "src", "ExtractInternalNames", "Names.xml"));
        if (!File.Exists(catalogPath))
        {
            throw new FileNotFoundException("Name catalog not found. Use --catalog PATH.", catalogPath);
        }

        var catalog = NameCatalog.Load(catalogPath);
        if (!options.FromCatalog)
        {
            using var session = InventorSession.Connect(options.Connect, options.TimeoutSeconds);
            Console.WriteLine($"Connected: {session.Application.SoftwareVersion.DisplayName} ({session.Application.LanguageCode})");
            if (session.Application.LanguageCode != "en-US" && (selected.Contains("properties") || selected.Contains("defaults")))
            {
                throw new InvalidOperationException("The properties/defaults categories require English Inventor to preserve existing string values. " +
                                                    "Use --only to extract invariant categories on other languages.");
            }

            using var context = new ExtractionContext(session.Application, catalog, options.Templates);
            var discovered = new List<NameGroup>();
            foreach (var extractor in extractors.Where(e => selected.Contains(e.Id)))
            {
                Console.WriteLine("Extracting " + extractor.Id + "...");
                try { discovered.AddRange(extractor.Extract(context)); }
                catch (Exception error) { throw new InvalidOperationException($"{extractor.Id}: {error.Message}", error); }
            }
            catalog = CatalogMerger.Merge(catalog, discovered, out var summary);
            Console.WriteLine($"Constants: {summary.Added} added, {summary.Observed} observed, {summary.Retained} retained without observation.");
        }
        // Validate the entire catalog even when writing only selected categories.
        var generated = SourceGenerator.Generate(catalog);
        var selectedFiles = new HashSet<string>(catalog.Groups.Where(g => selected.Contains(g.Category)).Select(g => g.File));
        var changes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var file in generated.Where(f => selectedFiles.Contains(f.Key)))
            changes.Add(Path.GetFullPath(Path.Combine(output, file.Key.Replace('/', Path.DirectorySeparatorChar))), file.Value);
        if (changes.ContainsKey(Path.GetFullPath(catalogPath)))
        {
            throw new InvalidOperationException("Catalog path overlaps a generated file.");
        }

        changes.Add(Path.GetFullPath(catalogPath), catalog.Serialize());
        var changed = FileTransaction.ChangedFiles(changes);
        foreach (var path in changed) Console.WriteLine((options.DryRun || options.Check ? "Would update: " : "Update: ") + path);
        if (options.Check || options.DryRun)
        {
            Console.WriteLine($"{changed.Count} file(s) would change.");
            return options.Check && changed.Count > 0 ? 2 : 0;
        }
        FileTransaction.Apply(changes);
        Console.WriteLine($"{changed.Count} file(s) changed.");
        if (options.Build)
        {
            var project = Path.Combine(output, "Inventor.InternalNames.csproj");
            if (!File.Exists(project))
            {
                throw new FileNotFoundException("--build requires the destination library project.", project);
            }

            var start = new ProcessStartInfo("dotnet") { UseShellExecute = false, WorkingDirectory = output,
                Arguments = "build Inventor.InternalNames.csproj --configuration Release --nologo" };
            using var process = Process.Start(start);
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException("Generated files were written, but dotnet build failed.");
            }
        }
        return 0;
    }
}
