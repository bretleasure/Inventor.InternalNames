using Inventor;
using Inventor.InternalNames.Ribbon;
using Application = Inventor.Application;

namespace ExtractInternalNames.Buttons
{
	public class GetCommandNames : BaseGetButton
	{
		protected override void Execute(NameValueMap context, Application inventor)
		{
			var commands = inventor.CommandManager.ControlDefinitions
				.Cast<ControlDefinition>()
				.ToList();
        
			var names = commands.ToDictionary(l => l.InternalName, l => l.InternalName);
        
			WriteClass("CommandNames", names, "c");
		}

		protected override string RibbonName => InventorRibbons.Assembly;
	}
}