using Inventor;
using Inventor.InternalNames.Ribbon;
using Application = Inventor.Application;

namespace ExtractInternalNames.Buttons;

public class GetApplicationAddins : BaseGetButton
{
    protected override void Execute(NameValueMap context, Application inventor)
    {
        var addins = inventor.ApplicationAddIns
            .Cast<ApplicationAddIn>()
            .ToList();
        
        var names = addins.ToDictionary(a => a.DisplayName, a => a.ClassIdString);
        
        WriteClass("ApplicationAddinIds", names);
    }

    protected override string RibbonName => InventorRibbons.Assembly;
}