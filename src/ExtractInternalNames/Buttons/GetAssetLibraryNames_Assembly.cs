using Inventor;
using Inventor.InternalNames.Ribbon;
using Application = Inventor.Application;

namespace ExtractInternalNames.Buttons;

public class GetAssetLibraryNames_Assembly : InventorButton
{
    protected override void Execute(NameValueMap context, Application inventor)
    {
        var assetLibraries = inventor.AssetLibraries
            .Cast<AssetLibrary>()
            .ToList();
        
        var names = assetLibraries.ToDictionary(l => l.DisplayName, l => l.InternalName);
        
        var code = ClassGenerator.GenerateCode("AssetLibraryNames", names);

        MessageBox.Show(code);
    }

    protected override string RibbonName => InventorRibbons.Assembly;
    protected override string RibbonTabName => "Internal Names";
    protected override string RibbonPanelName => "GetAssetLibraryNames";
    protected override string Label => "Get Asset Library Names";
    protected override string Description => "Gets the names of all the asset libraries in the current assembly.";
    protected override string Tooltip => Description;
    protected override string LargeIconResourceName => "ExtractInternalNames.Buttons.Assets.Default-Light.png";

    protected override string DarkThemeLargeIconResourceName =>
        "ExtractInternalNames.Buttons.Assets.Default-Dark.png";

    protected override string SmallIconResourceName => LargeIconResourceName;

    protected override string DarkThemeSmallIconResourceName => DarkThemeLargeIconResourceName;
}