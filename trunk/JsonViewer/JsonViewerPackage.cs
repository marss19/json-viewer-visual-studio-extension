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
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Windows;

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
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    // This attribute registers a tool window exposed by this package.
    [ProvideToolWindow(typeof(ComparerToolWindowPane))]
    [ProvideToolWindow(typeof(PathEvaluatorToolWindowPane))]
    [Guid(GuidList.guidJsonViewerPkgString)]
    public sealed class JsonViewerPackage : AsyncPackage
    {

        #region Package Members

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {

            VsExtensionHelper.CurrentDTE = (DTE)((System.IServiceProvider)this).GetService(typeof(DTE));
            VsExtensionHelper.Package = this;

            OleMenuCommandService commandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                commandService.AddCommand(new MenuCommand(
                    PasteFromClipboardMenuItemCallback,
                    new CommandID(GuidList.guidJsonViewerCmdSet, (int)PkgCmdIDList.cmdidPasteJsonFromClipboard))
                );

                commandService.AddCommand(new MenuCommand(
                    OpenEmptyJsonFileMenuItemCallback, 
                    new CommandID(GuidList.guidJsonViewerCmdSet, (int)PkgCmdIDList.cmdidOpenEmptyJsonFile))
                );

                commandService.AddCommand(new MenuCommand(
                    CompareJsonDataMenuItemCallback,
                    new CommandID(GuidList.guidJsonViewerCmdSet, (int)PkgCmdIDList.cmdidCompareJsonData))
                );

                commandService.AddCommand(new MenuCommand(
                    EvaluateJsonPathMenuItemCallback,
                    new CommandID(GuidList.guidJsonViewerCmdSet, (int)PkgCmdIDList.cmdidEvaluateJSONPath))
                );

                commandService.AddCommand(new MenuCommand(
                    SendFeedbackMenuItemCallback,
                    new CommandID(GuidList.guidJsonViewerCmdSet, (int)PkgCmdIDList.cmdidSendFeedback))
                );
            }

            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
        }


        #endregion

        private void OpenJsonFile(string text)
        {
            TempFileManager.PurgeOldTempFiles();
            var path = TempFileManager.GetTempFileFullPath();

            var formattedText = JsonFormatter.FormatIfPossible(text);
            File.WriteAllText(path, formattedText);

            VsExtensionHelper.OpenFile(path);
        }

        private void PasteFromClipboardMenuItemCallback(object sender, EventArgs e)
        {
            OpenJsonFile(Clipboard.GetText());
        }

        private void OpenEmptyJsonFileMenuItemCallback(object sender, EventArgs e)
        {
            OpenJsonFile("");
        }

        private void CompareJsonDataMenuItemCallback(object sender, EventArgs e)
        {
            VsExtensionHelper.ShowToolWindow<ComparerToolWindowPane>();
        }

        private void EvaluateJsonPathMenuItemCallback(object sender, EventArgs e)
        {
            VsExtensionHelper.ShowToolWindow<PathEvaluatorToolWindowPane>();
        }

        private void SendFeedbackMenuItemCallback(object sender, EventArgs e)
        {
            var url = "https://marketplace.visualstudio.com/items?itemName=MykolaTarasyuk.JSONViewerVS2022&ssr=false#review-details";
            System.Diagnostics.Process.Start(url);
        }

    }
}
