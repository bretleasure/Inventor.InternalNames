using Inventor;
using Inventor.InternalNames.Ribbon;
using Application = Inventor.Application;

namespace ExtractInternalNames.Buttons
{
	public class GetAssetPropertyNames : BaseGetButton
	{
		protected override void Execute(NameValueMap context, Application inventor)
		{
			var partDoc = inventor.ActiveDocument as PartDocument;
			var asset = partDoc.Assets[1];
			
			var props = asset
				.Cast<AssetValue>()
				.ToList();
        
			var names = props.ToDictionary(l => l.Name, l => l.Name);
        
			WriteClass("AssetPropertyNames", names);
		}

		protected override string RibbonName => InventorRibbons.Part;
	}
}