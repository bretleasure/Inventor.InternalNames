using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Inventor;
using ExtractInternalNames.Buttons;

namespace ExtractInternalNames
{
    /// <summary>
    /// This is the primary AddIn Server class that implements the ApplicationAddInServer interface
    /// that all Inventor AddIns are required to implement. The communication between Inventor and
    /// the AddIn is via the methods on this interface.
    /// </summary>
    [Guid("6954CBDD-6C6A-40AB-9509-ED165787C54D")]
    public class AddinServer : Inventor.ApplicationAddInServer
    {
        // The Inventor application instance
        public static Inventor.Application InventorApp;

        public static Guid AddinGuid = new("6954CBDD-6C6A-40AB-9509-ED165787C54D");

        List<InventorButton> _buttons;

        #region ApplicationAddInServer Members

        /// <summary>
        /// This method is called by Inventor when it loads the addin.
        /// The AddInSiteObject provides access to the Inventor Application object.
        /// The FirstTime flag indicates if the addin is loaded for the first time.
        /// </summary>
        public void Activate(Inventor.ApplicationAddInSite addInSiteObject, bool firstTime)
        {
            InventorApp = addInSiteObject.Application;
            InventorApp.ApplicationEvents.OnApplicationOptionChange += UpdateButtons;

            try
            {
                // If the addin is loaded for the first time, initialize the UI components
                if (firstTime)
                {
                    InitializeUIComponents();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load ExtractInternalNames.\n\n{ex.Message}", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Initializes the UI components of the addin.
        /// </summary>
        private void InitializeUIComponents()
        {
            _buttons = Assembly.GetAssembly(typeof(InventorButton)).GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(InventorButton)))
                .Select(Activator.CreateInstance)
                .Cast<InventorButton>()
                .Where(button => button.Enabled)
                .OrderBy(button => button.SequenceNumber)
                .ToList();

            _buttons.ForEach(b => b.Initialize());
        }

        /// <summary>
        /// Updates the buttons when the application options change.
        /// </summary>
        private void UpdateButtons(EventTimingEnum beforeOrAfter, NameValueMap context,
            out HandlingCodeEnum handlingCode)
        {
            if (beforeOrAfter == EventTimingEnum.kAfter)
            {
                _buttons.ForEach(b => b.Dispose());

                InitializeUIComponents();

                handlingCode = HandlingCodeEnum.kEventHandled;
            }

            handlingCode = HandlingCodeEnum.kEventNotHandled;
        }

        /// <summary>
        /// This method is called by Inventor when the AddIn is unloaded.
        /// The AddIn will be unloaded either manually by the user or
        /// when the Inventor session is terminated.
        /// </summary>
        public void Deactivate()
        {
            InventorApp = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// This method is now obsolete, you should use the
        /// ControlDefinition functionality for implementing commands.
        /// </summary>
        public void ExecuteCommand(int commandID)
        {
        }

        /// <summary>
        /// This property is provided to allow the AddIn to expose an API
        /// of its own to other programs. Typically, this  would be done by
        /// implementing the AddIn's API interface in a class and returning
        /// that class object through this property.
        /// </summary>
        public object Automation
        {
            get { return null; }
        }

        #endregion
    }
}