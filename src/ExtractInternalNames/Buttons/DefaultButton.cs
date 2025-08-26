using System.Windows.Forms;
using Inventor;
using Inventor.InternalNames.Ribbon;

namespace ExtractInternalNames.Buttons
{
    public class DefaultButton : InventorButton
    {
        protected override void Execute(NameValueMap context, Inventor.Application inventor)
        {
            MessageBox.Show($"Current document name: {inventor.ActiveDocument.DisplayName}");
        }

        protected override string RibbonName => InventorRibbons.Drawing;

        protected override string RibbonTabName => DrawingRibbonTabs.PlaceViews;

        protected override string RibbonPanelName => "ExtractInternalNames";

        protected override string Label => "DefaultButton";

        protected override string Description => "Default Button Description";

        protected override string Tooltip => "Click the Default Button";

        protected override string LargeIconResourceName => "ExtractInternalNames.Buttons.Assets.Default-Light.png";

        protected override string DarkThemeLargeIconResourceName =>
            "ExtractInternalNames.Buttons.Assets.Default-Dark.png";

        protected override string SmallIconResourceName => LargeIconResourceName;

        protected override string DarkThemeSmallIconResourceName => DarkThemeLargeIconResourceName;
    }
}