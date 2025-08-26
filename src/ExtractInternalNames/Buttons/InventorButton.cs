using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Inventor;
using IPictureDisp = Inventor.IPictureDisp;

namespace ExtractInternalNames.Buttons
{
    public abstract class InventorButton
    {
        protected abstract void Execute(NameValueMap context, Inventor.Application inventor);

        /// <summary>
        /// Name of the ribbon.
        /// </summary>
        protected abstract string RibbonName { get; }

        /// <summary>
        /// Name of the ribbon tab where the button will be placed. If the tab
        /// does not exist, it will be created.
        /// </summary>
        protected abstract string RibbonTabName { get; }

        /// <summary>
        /// Name of the ribbon panel where the button will be placed. If the
        /// panel does not exist, it will be created.
        /// </summary>
        protected abstract string RibbonPanelName { get; }

        /// <summary>
        /// Name of the button. This is the name that will be displayed in the
        /// UI.
        /// </summary>
        protected abstract string Label { get; }

        /// <summary>
        /// Internal name of the button. This is the name that will be used to
        /// identify the button in the code. By default, it is the GUID of the
        /// add-in following by the button's label.
        /// </summary>
        protected virtual string InternalName => string.Concat(AddinServer.AddinGuid.ToString(), "-", Label);

        /// <summary>
        /// Description
        /// </summary>
        protected abstract string Description { get; }

        /// <summary>
        /// The tooltip text that should be displayed when the user hovers over
        /// the button.
        /// </summary>
        protected abstract string Tooltip { get; }

        /// <summary>
        /// The name of the embedded resource that should be used for the icon
        /// when the button is large. Large icons should be 32px by 32px.
        /// </summary>
        protected abstract string LargeIconResourceName { get; }

        /// <summary>
        /// The name of the embedded resource that should be used for the icon
        /// when the button is large and the application is using the dark
        /// UI theme. Large icons should be 32px by 32px.
        /// </summary>
        protected abstract string DarkThemeLargeIconResourceName { get; }

        /// <summary>
        /// The name of the embedded resource that should be used for the icon
        /// when the button is small. Small icons should be 16px by 16px.
        /// </summary>
        protected abstract string SmallIconResourceName { get; }

        /// <summary>
        /// The name of the embedded resource that should be use for the icon
        /// when the button is small and the application is using the light
        /// UI theme. Small icons should be 16px by 16px.
        /// </summary>
        protected abstract string DarkThemeSmallIconResourceName { get; }

        protected virtual CommandTypesEnum CommandType => CommandTypesEnum.kEditMaskCmdType;
        protected virtual bool UseLargeIcon => true;
        protected virtual bool ShowText => true;
        internal virtual int SequenceNumber => 0;

        protected virtual void ConfigureProgressiveToolTip(ProgressiveToolTip toolTip)
        {
        }

        protected virtual void OnHelp(NameValueMap context, out HandlingCodeEnum handlingcode)
        {
            handlingcode = HandlingCodeEnum.kEventNotHandled;
        }


        private bool AddToRibbon => true;

        private Ribbon _ribbon;

        private Ribbon Ribbon
        {
            get
            {
                if (_ribbon == null)
                {
                    if (string.IsNullOrWhiteSpace(RibbonName))
                    {
                        throw new Exception(
                            $"Unable to find or create ribbon. The name specified in {this.GetType().Name} is null or empty.");
                    }

                    _ribbon = AddinServer.InventorApp.UserInterfaceManager.Ribbons
                        .Cast<Ribbon>()
                        .FirstOrDefault(r => r.InternalName == RibbonName);

                    if (_ribbon == null)
                    {
                        throw new Exception($"Ribbon {RibbonName} not found");
                    }
                }

                return _ribbon;
            }
        }

        private RibbonTab _ribbonTab;

        private RibbonTab RibbonTab
        {
            get
            {
                if (_ribbonTab == null)
                {
                    if (string.IsNullOrEmpty(RibbonTabName))
                    {
                        throw new Exception(
                            $"Unable to find or create ribbon tab. The name specified in {this.GetType().Name} is null or empty.");
                    }

                    _ribbonTab = Ribbon.RibbonTabs
                        .Cast<RibbonTab>()
                        .FirstOrDefault(t => t.InternalName == RibbonTabName);

                    if (_ribbonTab == null)
                    {
                        _ribbonTab = Ribbon.RibbonTabs.Add(RibbonTabName, RibbonTabName, Guid.NewGuid().ToString());
                    }
                }

                return _ribbonTab;
            }
        }

        private RibbonPanel _ribbonPanel;

        private RibbonPanel RibbonPanel
        {
            get
            {
                if (_ribbonPanel == null)
                {
                    if (string.IsNullOrEmpty(RibbonPanelName))
                    {
                        throw new Exception(
                            $"Unable to find or create ribbon panel. The name specified in {this.GetType().Name} is null or empty.");
                    }

                    _ribbonPanel = RibbonTab.RibbonPanels
                        .Cast<RibbonPanel>()
                        .FirstOrDefault(p => p.InternalName == RibbonPanelName);

                    if (_ribbonPanel == null)
                    {
                        _ribbonPanel = RibbonTab.RibbonPanels.Add(RibbonPanelName, RibbonPanelName,
                            Guid.NewGuid().ToString());
                    }
                }

                return _ribbonPanel;
            }
        }

        private ButtonDefinition _buttonDefinition;

        private ButtonDefinition Definition
        {
            get
            {
                if (_buttonDefinition == null)
                {
                    _buttonDefinition = AddinServer.InventorApp.CommandManager.ControlDefinitions.AddButtonDefinition(
                        Label, InternalName,
                        CommandType, null, Description, Tooltip, SmallIcon, LargeIcon);

                    _buttonDefinition.Enabled = true;
                    _buttonDefinition.OnExecute += (NameValueMap context) => Execute(context, AddinServer.InventorApp);
                    _buttonDefinition.OnHelp += (NameValueMap context, out HandlingCodeEnum handlingCode) =>
                        OnHelp(context, out handlingCode);
                    this.ConfigureProgressiveToolTip(_buttonDefinition.ProgressiveToolTip);
                }

                return _buttonDefinition;
            }
        }

        private CommandControl _button;

        /// <summary>
        /// Controls whether the button is enabled or not. If false, the button will be disabled (greyed out) and cannot be clicked.
        /// </summary>
        internal virtual bool Enabled
        {
            get => Definition.Enabled;
            set => Definition.Enabled = value;
        }

        internal void Initialize()
        {
            if (AddToRibbon)
            {
                _button = RibbonPanel.CommandControls.AddButton(Definition, UseLargeIcon, ShowText);
            }
            else
            {
                throw new NotImplementedException("AddToRibbon is false");
            }
        }

        internal void Dispose()
        {
            Definition.Delete();
            _button.Delete();
        }

        private IPictureDisp LightThemeLargeIcon => CreateIcon(LargeIconResourceName);
        private IPictureDisp DarkThemeLargeIcon => CreateIcon(DarkThemeLargeIconResourceName);
        private IPictureDisp LightThemeSmallIcon => CreateIcon(SmallIconResourceName);
        private IPictureDisp DarkThemeSmallIcon => CreateIcon(DarkThemeSmallIconResourceName);

        private IPictureDisp LargeIcon
        {
            get
            {
                if (AddinServer.InventorApp.ThemeManager.ActiveTheme.Name == "LightTheme")
                {
                    return LightThemeLargeIcon;
                }
                else
                {
                    return DarkThemeLargeIcon;
                }
            }
        }

        private IPictureDisp SmallIcon
        {
            get
            {
                if (AddinServer.InventorApp.ThemeManager.ActiveTheme.Name == "LightTheme")
                {
                    return LightThemeSmallIcon;
                }
                else
                {
                    return DarkThemeSmallIcon;
                }
            }
        }

        private IPictureDisp CreateIcon(string resourceName)
        {
            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

            if (resourceStream == null)
                throw new Exception(
                    $"Resource {resourceName} not found in assembly {Assembly.GetExecutingAssembly().FullName}");

            var bitmap = new Bitmap(resourceStream);

            return ImageConverter.BitmapToPicture(bitmap);
        }
    }

    internal class ImageConverter : AxHost
    {
        public ImageConverter() : base(string.Empty)
        {
        }

        public static IPictureDisp BitmapToPicture(Bitmap bitmap)
        {
            return (IPictureDisp)GetIPictureDispFromPicture(bitmap);
        }
    }
}