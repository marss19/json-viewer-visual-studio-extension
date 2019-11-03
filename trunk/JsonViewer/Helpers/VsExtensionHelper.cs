using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marss.JsonViewer.Helpers
{
    public static class VsExtensionHelper
    {
        public static DTE CurrentDTE { get; set; }

        public static JsonViewerPackage Package { get; set; }

        public static void OpenFile(string filePath)
        {
            CurrentDTE.ItemOperations.OpenFile(filePath);
        }

        public static void ShowToolWindow<T>()
            where T: ToolWindowPane
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            ToolWindowPane window = Package.FindToolWindow(typeof(T), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException("Cannot create the tool window.");
            }
            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}
