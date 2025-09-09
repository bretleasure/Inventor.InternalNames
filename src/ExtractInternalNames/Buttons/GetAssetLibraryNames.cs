using Inventor;
using Inventor.InternalNames.Ribbon;
using Application = Inventor.Application;

namespace ExtractInternalNames.Buttons;

public class GetAssetLibraryNames : BaseGetButton
{
    protected override void Execute(NameValueMap context, Application inventor)
    {
        var assetLibraries = inventor.AssetLibraries
            .Cast<AssetLibrary>()
            .ToList();
        
        var names = assetLibraries.ToDictionary(l => l.DisplayName, l => l.InternalName);
        
        WriteClass("AssetLibraryNames", names);
    }

    protected override string RibbonName => InventorRibbons.Assembly;
}