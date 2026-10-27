using ExtractInternalNames.Core;
using Inventor;

namespace ExtractInternalNames.Extractors;

internal sealed class PropertiesExtractor : INameExtractor
{
    public string Id => "properties";
    public string Description => "Property-set and iProperty names in Part, Assembly, and Drawing templates";

    public IEnumerable<NameGroup> Extract(ExtractionContext context)
    {
        var groups = new List<NameGroup>();
        foreach (var pair in new[] { ("Part", DocumentTypeEnum.kPartDocumentObject),
                     ("Assembly", DocumentTypeEnum.kAssemblyDocumentObject), ("Drawing", DocumentTypeEnum.kDrawingDocumentObject) })
        {
            var document = context.Document(pair.Item2);
            var sets = context.Group(Id, "PropertySets/" + pair.Item1 + ".cs", pair.Item1,
                "PropertySets." + pair.Item1, "Inventor.InternalNames.PropertySets");
            var properties = context.Group(Id, "iProperties/" + pair.Item1 + ".cs", pair.Item1,
                "iProperties." + pair.Item1, "Inventor.InternalNames.iProperties");
            groups.Add(sets);
            groups.Add(properties);
            foreach (PropertySet set in document.PropertySets)
            {
                var setKey = Guid.TryParse(set.InternalName, out var guid) ? guid.ToString("B").ToUpperInvariant() : set.InternalName;
                // These public constants historically contain names, not FMTID GUIDs.
                ExtractionContext.Add(sets, setKey, set.Name, set.Name);
                foreach (Property property in set)
                    ExtractionContext.Add(properties, setKey + ":" + property.PropId, property.Name, property.Name);
            }
            if (sets.Names.Count == 0 || properties.Names.Count == 0)
            {
                throw new InvalidOperationException($"No standard properties were returned for {pair.Item1}.");
            }
        }
        return groups;
    }
}
