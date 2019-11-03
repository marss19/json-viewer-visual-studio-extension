using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;

using EnvDTE;

using Marss.JsonViewer.Helpers;
using Marss.JsonViewer.Views;
using System.Windows.Interop;

namespace Marss.JsonViewer
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    // This attribute registers a tool window exposed by this package.
    [ProvideToolWindow(typeof(ComparerToolWindowPane))]
    [ProvideToolWindow(typeof(PathEvaluatorToolWindowPane))]
    [Guid(GuidList.guidJsonViewerPkgString)]
    public sealed class JsonViewerPackage : Package
    {

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            VsExtensionHelper.CurrentDTE = (DTE)((System.IServiceProvider)this).GetService(typeof(DTE));
            VsExtensionHelper.Package = this;

            OleMenuCommandService commandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {

                CommandID menuCommandID = new CommandID(GuidList.guidJsonViewerCmdSet, (int)PkgCmdIDList.cmdidJsnVwr);
                MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);

                CommandID toolwndCommandID = new CommandID(GuidList.guidJsonViewerCmdSet, (int)PkgCmdIDList.cmdidJsnVwrTool);
                MenuCommand menuToolWin = new MenuCommand(MenuItemCallback, toolwndCommandID);
                commandService.AddCommand(menuToolWin);
            }
        }
        #endregion


        private void MenuItemCallback(object sender, EventArgs e)
        {
            var dialog = new SelectorWindow();

            var hwnd = new IntPtr(VsExtensionHelper.CurrentDTE.MainWindow.HWnd);
            var window = (System.Windows.Window)HwndSource.FromHwnd(hwnd).RootVisual;
            dialog.Owner = window;

            dialog.ShowDialog();
        }

    }
}
