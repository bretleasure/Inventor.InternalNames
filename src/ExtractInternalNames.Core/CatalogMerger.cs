using System.Security.Cryptography;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;

namespace ExtractInternalNames.Core;

public sealed class MergeSummary
{
    public int Added { get; internal set; }
    public int Observed { get; internal set; }
    public int Retained { get; internal set; }
}

public static class CatalogMerger
{
    // Merge into a copy so a failed extraction never mutates the caller's baseline.
    public static NameCatalog Merge(NameCatalog baseline, IEnumerable<NameGroup> discovered,
        out MergeSummary summary)
    {
        var catalog = Clone(baseline);
        summary = new MergeSummary();
        var observed = new HashSet<NameEntry>();
        foreach (var incoming in discovered.OrderBy(g => g.Category, StringComparer.Ordinal)
                     .ThenBy(g => g.Key, StringComparer.Ordinal))
        {
            var group = catalog.Groups.SingleOrDefault(g => g.Category == incoming.Category && g.Key == incoming.Key);
            if (group == null)
            {
                group = new NameGroup { Category = incoming.Category, Key = incoming.Key, File = incoming.File,
                    Namespace = incoming.Namespace, Class = incoming.Class };
                catalog.Groups.Add(group);
            }
            foreach (var entry in incoming.Names.OrderBy(n => n.Key, StringComparer.Ordinal)
                         .ThenBy(n => n.Value, StringComparer.Ordinal))
            {
                if (string.IsNullOrWhiteSpace(entry.Key) || entry.Value == null)
                {
                    throw new InvalidDataException($"Missing discovered identity or value in {group.Class}.");
                }

                var matches = group.Names.Where(n => n.Key == entry.Key).ToList();
                if (matches.Any(n => n.Value != entry.Value))
                {
                    throw new InvalidDataException($"Value conflict in {group.Class} for {entry.Key}. " +
                                                   "Existing values are preserved; review the catalog before accepting a changed value.");
                }

                if (matches.Count == 0)
                {
                    matches = group.Names.Where(n => n.Key == null && n.Value == entry.Value).ToList();
                }

                if (matches.Count == 0)
                {
                    var name = Identifier(entry.Name);
                    var reserved = new HashSet<string>(group.Names.Select(n => n.Name), StringComparer.Ordinal);
                    // Nested types and their enclosing type also reserve member names.
                    reserved.Add(group.Class.Split('.').Last());
                    foreach (var child in catalog.Groups.Where(g => g.Namespace == group.Namespace &&
                                 g.Class.StartsWith(group.Class + ".", StringComparison.Ordinal)))
                        reserved.Add(child.Class.Substring(group.Class.Length + 1).Split('.')[0]);
                    if (reserved.Contains(name))
                    {
                        name += "_" + StableSuffix(entry.Key);
                    }

                    if (reserved.Contains(name))
                    {
                        throw new InvalidDataException($"Name collision in {group.Class}: {name}");
                    }

                    var added = new NameEntry { Name = name, Value = entry.Value, Key = entry.Key };
                    group.Names.Add(added);
                    matches.Add(added);
                    summary.Added++;
                }
                foreach (var match in matches)
                {
                    match.Key = entry.Key;
                    observed.Add(match);
                }
            }
        }
        catalog.Validate();
        summary.Observed = observed.Count;
        summary.Retained = catalog.Groups.Sum(g => g.Names.Count) - observed.Count;
        return catalog;
    }

    public static string Identifier(string label)
    {
        var sb = new StringBuilder();
        foreach (var c in label ?? "")
            if (SyntaxFacts.IsIdentifierPartCharacter(c))
            {
                sb.Append(c);
            }

        if (sb.Length == 0)
        {
            sb.Append("Unnamed");
        }

        if (!SyntaxFacts.IsIdentifierStartCharacter(sb[0]))
        {
            sb.Insert(0, 'e');
        }

        var identifier = sb.ToString();
        if (SyntaxFacts.GetKeywordKind(identifier) != SyntaxKind.None)
        {
            identifier = "_" + identifier;
        }

        return identifier;
    }

    public static string StableSuffix(string identity)
    {
        using var hash = SHA256.Create();
        return BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(identity)), 0, 6)
            .Replace("-", "").ToLowerInvariant();
    }

    private static NameCatalog Clone(NameCatalog source)
    {
        source.Validate();
        var copy = new NameCatalog();
        foreach (var group in source.Groups)
        {
            var target = new NameGroup { Category = group.Category, File = group.File,
                Namespace = group.Namespace, Class = group.Class, Key = group.Key };
            target.Names.AddRange(group.Names.Select(n => new NameEntry { Name = n.Name, Value = n.Value, Key = n.Key }));
            copy.Groups.Add(target);
        }
        return copy;
    }
}
