using Inventor;
using Inventor.InternalNames.Ribbon;
using Application = Inventor.Application;
using Environment = Inventor.Environment;

namespace ExtractInternalNames.Buttons
{
	public class GetEnvironments : BaseGetButton
	{
		protected override void Execute(NameValueMap context, Application inventor)
		{
			var environments = inventor.Environments
				.Cast<Environment>()
				.ToList();
			
			var names = environments.ToDictionary(l => l.InternalName, l => l.InternalName);
        
			WriteClass("Environments", names);
		}

		protected override string RibbonName => InventorRibbons.Assembly;
	}
}