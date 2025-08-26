using Inventor;
using Application = Inventor.Application;

namespace ExtractInternalNames.Buttons;

public abstract class BaseGetButton : InventorButton
{
    protected override string RibbonTabName => "Internal Names";
    protected override string RibbonPanelName => this.GetType().Name;
    protected override string Description => "";
    protected override string Tooltip => Description;
    protected override string LargeIconResourceName => "ExtractInternalNames.Buttons.Assets.Default-Light.png";

    protected override string DarkThemeLargeIconResourceName =>
        "ExtractInternalNames.Buttons.Assets.Default-Dark.png";

    protected override string SmallIconResourceName => LargeIconResourceName;

    protected override string DarkThemeSmallIconResourceName => DarkThemeLargeIconResourceName;
}