using System.Xml.Linq;
using System.Xml;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;

namespace ExtractInternalNames.Core;

public sealed class NameEntry
{
    public string Name { get; set; }
    public string Value { get; set; }
    public string Key { get; set; }
}

public sealed class NameGroup
{
    public string Category { get; set; }
    public string File { get; set; }
    public string Namespace { get; set; }
    public string Class { get; set; }
    public string Key { get; set; }
    public List<NameEntry> Names { get; } = new();
}

/// <summary>The persistent public names, including aliases and entries absent from this installation.</summary>
public sealed class NameCatalog
{
    public List<NameGroup> Groups { get; } = new();

    public static NameCatalog Load(string path)
    {
        var document = XDocument.Load(path);
        if (document.Root?.Name != "names" || (string)document.Root.Attribute("version") != "1")
            throw new InvalidDataException("Unsupported name catalog format.");
        var catalog = new NameCatalog();
        foreach (var element in document.Root.Elements("group"))
        {
            var group = new NameGroup
            {
                Category = Required(element, "category"), File = Required(element, "file"),
                Namespace = Required(element, "namespace"), Class = Required(element, "class"),
                Key = Required(element, "key")
            };
            foreach (var name in element.Elements("name"))
                group.Names.Add(new NameEntry
                {
                    Name = Required(name, "name"), Value = Required(name, "value", allowEmpty: true),
                    Key = ReadAttribute(name, "key")
                });
            catalog.Groups.Add(group);
        }
        catalog.Validate();
        return catalog;
    }

    private static string Required(XElement element, string attribute, bool allowEmpty = false)
    {
        var value = ReadAttribute(element, attribute);
        if (value == null || (!allowEmpty && string.IsNullOrWhiteSpace(value)))
            throw new InvalidDataException($"Missing {attribute} on {element.Name}.");
        return value;
    }

    private static string ReadAttribute(XElement element, string attribute) => element.Attribute(attribute + "64") is XAttribute encoded
        ? Encoding.UTF8.GetString(Convert.FromBase64String(encoded.Value)) : (string)element.Attribute(attribute);

    private static XAttribute StringAttribute(string name, string value) => value.Any(c => !XmlConvert.IsXmlChar(c))
        ? new XAttribute(name + "64", Convert.ToBase64String(Encoding.UTF8.GetBytes(value))) : new XAttribute(name, value);

    public string Serialize()
    {
        Validate();
        var root = new XElement("names", new XAttribute("version", "1"));
        foreach (var group in Groups.OrderBy(g => g.File, StringComparer.Ordinal)
                     .ThenBy(g => g.Class, StringComparer.Ordinal))
        {
            var element = new XElement("group", new XAttribute("category", group.Category),
                new XAttribute("file", group.File), new XAttribute("namespace", group.Namespace),
                new XAttribute("class", group.Class), new XAttribute("key", group.Key));
            foreach (var name in group.Names.OrderBy(n => n.Name, StringComparer.Ordinal))
            {
                var entry = new XElement("name", new XAttribute("name", name.Name), StringAttribute("value", name.Value));
                if (name.Key != null) entry.Add(StringAttribute("key", name.Key));
                element.Add(entry);
            }
            root.Add(element);
        }
        return root.ToString().Replace("\r\n", "\n") + "\n";
    }

    public void Validate()
    {
        if (Groups.Count == 0) throw new InvalidDataException("The catalog has no groups.");
        var identities = new HashSet<string>(StringComparer.Ordinal);
        var types = new HashSet<string>(StringComparer.Ordinal);
        foreach (var group in Groups)
        {
            SafePath.ValidateRelative(group.File);
            if (!group.File.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                throw new InvalidDataException("Catalog output files must end in .cs.");
            if (string.IsNullOrWhiteSpace(group.Category) || string.IsNullOrWhiteSpace(group.Key) ||
                !identities.Add(group.Category + "|" + group.Key))
                throw new InvalidDataException($"Duplicate or empty group identity: {group.Key}");
            if (!QualifiedIdentifier(group.Namespace) || !QualifiedIdentifier(group.Class) ||
                !types.Add(group.Namespace + "." + group.Class))
                throw new InvalidDataException($"Invalid or duplicate type: {group.Namespace}.{group.Class}");
            var members = new HashSet<string>(StringComparer.Ordinal);
            foreach (var name in group.Names)
                if (!SyntaxFacts.IsValidIdentifier(name.Name) || name.Value == null || !members.Add(name.Name))
                    throw new InvalidDataException($"Invalid or duplicate constant: {group.Class}.{name.Name}");
        }
        foreach (var file in Groups.GroupBy(g => g.File, StringComparer.OrdinalIgnoreCase))
            if (file.Select(g => g.Namespace).Distinct(StringComparer.Ordinal).Count() != 1)
                throw new InvalidDataException($"Multiple namespaces in output file {file.Key}.");
    }

    private static bool QualifiedIdentifier(string value) => !string.IsNullOrWhiteSpace(value) &&
        value.Split('.').All(SyntaxFacts.IsValidIdentifier);
}

public static class SafePath
{
    public static void ValidateRelative(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || path.IndexOfAny("\\:<>\"|?*".ToCharArray()) >= 0 ||
            Path.IsPathRooted(path) || path.Split('/').Any(p => p.Length == 0 || p == "." || p == ".." ||
                p.EndsWith(".", StringComparison.Ordinal) || p.EndsWith(" ", StringComparison.Ordinal)))
            throw new InvalidDataException($"Unsafe output path: {path}");
    }
}
