using ExtractInternalNames.Core;
using Inventor;
using Environment = Inventor.Environment;

namespace ExtractInternalNames.Extractors;

internal sealed class CommandsExtractor : INameExtractor
{
    public string Id => "commands";
    public string Description => "CommandManager control definition internal names";
    public IEnumerable<NameGroup> Extract(ExtractionContext context)
    {
        context.PrepareDocuments(includePresentation: true);
        var group = context.Group(Id, "CommandNames.cs", "CommandNames");
        foreach (ControlDefinition command in context.Application.CommandManager.ControlDefinitions)
            ExtractionContext.Add(group, command.InternalName, command.InternalName, command.InternalName);
        if (group.Names.Count == 0)
        {
            throw new InvalidOperationException("Inventor returned no commands.");
        }

        return new[] { group };
    }
}

internal sealed class EnvironmentsExtractor : INameExtractor
{
    public string Id => "environments";
    public string Description => "Available Inventor environment internal names";
    public IEnumerable<NameGroup> Extract(ExtractionContext context)
    {
        context.PrepareDocuments(includePresentation: true);
        var group = context.Group(Id, "Environments.cs", "Environments");
        foreach (Environment environment in context.Application.Environments)
            ExtractionContext.Add(group, environment.InternalName, environment.InternalName, environment.InternalName);
        return new[] { group };
    }
}

internal sealed class AddinsExtractor : INameExtractor
{
    public string Id => "addins";
    public string Description => "Installed application add-in GUIDs";
    public IEnumerable<NameGroup> Extract(ExtractionContext context)
    {
        var group = context.Group(Id, "ApplicationAddinIds.cs", "ApplicationAddinIds");
        foreach (ApplicationAddIn addin in context.Application.ApplicationAddIns)
        {
            var id = Guid.Parse(addin.ClassIdString).ToString("B").ToUpperInvariant();
            ExtractionContext.Add(group, id, addin.DisplayName, id);
        }
        return new[] { group };
    }
}

internal sealed class AssetLibrariesExtractor : INameExtractor
{
    public string Id => "asset-libraries";
    public string Description => "Available material and appearance library internal names";
    public IEnumerable<NameGroup> Extract(ExtractionContext context)
    {
        var group = context.Group(Id, "AssetLibraryNames.cs", "AssetLibraryNames");
        foreach (AssetLibrary library in context.Application.AssetLibraries)
            ExtractionContext.Add(group, library.InternalName, library.DisplayName, library.InternalName);
        return new[] { group };
    }
}

internal sealed class AssetPropertiesExtractor : INameExtractor
{
    public string Id => "asset-properties";
    public string Description => "Property keys across library appearance, material, and physical assets";
    public IEnumerable<NameGroup> Extract(ExtractionContext context)
    {
        var group = context.Group(Id, "AssetPropertyNames.cs", "AssetPropertyNames");
        foreach (AssetLibrary library in context.Application.AssetLibraries)
        {
            Read(library.AppearanceAssets, group);
            Read(library.MaterialAssets, group);
            Read(library.PhysicalAssets, group);
        }
        if (group.Names.Count == 0)
        {
            throw new InvalidOperationException("No asset properties were found. Check the configured asset libraries.");
        }

        return new[] { group };
    }

    private static void Read(AssetsEnumerator assets, NameGroup group)
    {
        foreach (Asset asset in assets)
            foreach (AssetValue property in asset)
                ExtractionContext.Add(group, property.Name, property.Name, property.Name);
    }
}
