using ExtractInternalNames.Core;
using Inventor;

namespace ExtractInternalNames.Extractors;

internal sealed class RibbonsExtractor : INameExtractor
{
    public string Id => "ribbons";
    public string Description => "Ribbons, tabs, and nested panel classes across document contexts";

    public IEnumerable<NameGroup> Extract(ExtractionContext context)
    {
        var groups = new HashSet<NameGroup>();
        Scan(context, groups);
        foreach (var type in new[] { DocumentTypeEnum.kPartDocumentObject, DocumentTypeEnum.kAssemblyDocumentObject,
                     DocumentTypeEnum.kDrawingDocumentObject, DocumentTypeEnum.kPresentationDocumentObject })
        {
            context.Document(type);
            Scan(context, groups);
        }
        return groups;
    }

    private void Scan(ExtractionContext context, ISet<NameGroup> groups)
    {
        var root = context.Group(Id, "Ribbon/InventorRibbons.cs", "InventorRibbons", namespaceName: "Inventor.InternalNames.Ribbon");
        groups.Add(root);
        foreach (var ribbon in context.Application.UserInterfaceManager.Ribbons.Cast<Ribbon>().OrderBy(r => r.InternalName, StringComparer.Ordinal))
        {
            var ribbonName = ExistingName(context, root, ribbon.InternalName, ribbon.InternalName);
            ExtractionContext.Add(root, ribbon.InternalName, ribbonName, ribbon.InternalName);
            var prefix = ribbonName == "iFeatures" ? "iFeature" : ribbonName;
            var tabsClass = prefix + "RibbonTabs";
            var tabs = context.Group(Id, "Ribbon/" + tabsClass + ".cs", tabsClass, namespaceName: "Inventor.InternalNames.Ribbon");
            groups.Add(tabs);
            var panelsClass = prefix + "RibbonPanels";
            var panelsFile = "Ribbon/" + panelsClass + ".cs";
            groups.Add(context.Group(Id, panelsFile, panelsClass, namespaceName: "Inventor.InternalNames.Ribbon"));
            foreach (var tab in ribbon.RibbonTabs.Cast<RibbonTab>().OrderBy(t => t.InternalName, StringComparer.Ordinal))
            {
                var tabName = ExistingName(context, tabs, tab.InternalName, tab.DisplayName);
                ExtractionContext.Add(tabs, tab.InternalName, tabName, tab.InternalName);
                var panels = context.Group(Id, panelsFile, panelsClass + "." + tabName + "Tab",
                    ribbon.InternalName + "|" + tab.InternalName, "Inventor.InternalNames.Ribbon");
                groups.Add(panels);
                foreach (RibbonPanel panel in tab.RibbonPanels)
                    ExtractionContext.Add(panels, panel.InternalName, panel.DisplayName, panel.InternalName);
            }
        }
    }

    private static string ExistingName(ExtractionContext context, NameGroup group, string key, string label)
    {
        var baseline = context.Catalog.Groups.SingleOrDefault(g => g.Category == group.Category && g.Key == group.Key);
        var known = baseline?.Names.FirstOrDefault(n => n.Key == key || (n.Key == null && n.Value == key));
        if (known != null) return known.Name;
        var observed = group.Names.FirstOrDefault(n => n.Key == key);
        if (observed != null) return observed.Name;
        var name = CatalogMerger.Identifier(label);
        if ((baseline?.Names ?? new List<NameEntry>()).Concat(group.Names).Any(n => n.Name == name && n.Key != key))
            name += "_" + CatalogMerger.StableSuffix(key);
        return name;
    }
}
