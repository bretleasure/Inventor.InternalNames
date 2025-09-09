using System.Diagnostics;
using Inventor;
using Application = Inventor.Application;

namespace ExtractInternalNames.Buttons;

public abstract class BaseGetButton : InventorButton
{
    protected override string Label => this.GetType().Name;
    protected override string RibbonTabName => "Internal Names";
    protected override string RibbonPanelName => this.GetType().Name;
    protected override string Description => "";
    protected override string Tooltip => Description;
    protected override string LargeIconResourceName => "ExtractInternalNames.Buttons.Assets.Default-Light.png";

    protected override string DarkThemeLargeIconResourceName =>
        "ExtractInternalNames.Buttons.Assets.Default-Dark.png";

    protected override string SmallIconResourceName => LargeIconResourceName;

    protected override string DarkThemeSmallIconResourceName => DarkThemeLargeIconResourceName;
    
    protected void WriteClass(string className, Dictionary<string, string> props, string prefix = "e")
    {
        var code = ClassGenerator.GenerateCode(className, props, prefix);

        var fileName = $"C:\\temp\\{this.GetType().Name}.txt";
			
        var file = new System.IO.StreamWriter(fileName);
        file.Write(code);
        file.Close();
			
        var startInfo = new ProcessStartInfo
        {
            FileName = fileName,
            UseShellExecute = true
        };
        Process.Start(startInfo);
    }
}