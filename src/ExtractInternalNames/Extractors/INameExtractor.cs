using ExtractInternalNames.Core;

namespace ExtractInternalNames.Extractors;

internal interface INameExtractor
{
    string Id { get; }
    string Description { get; }
    IEnumerable<NameGroup> Extract(ExtractionContext context);
}

internal static class ExtractorRegistry
{
    public static IReadOnlyList<INameExtractor> Create() => new INameExtractor[]
    {
        new RibbonsExtractor(), new PropertiesExtractor(), new DefaultsExtractor(), new CommandsExtractor(),
        new EnvironmentsExtractor(), new AddinsExtractor(), new AssetLibrariesExtractor(), new AssetPropertiesExtractor()
    };
}
