using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Marss.JsonViewer.Views
{
    [Guid("dfe033dc-fa25-4ece-85d3-8ed65d600602")]
    public class PathEvaluatorToolWindowPane : ToolWindowPane
    {
        public PathEvaluatorToolWindowPane() :
            base(null)
        {
            // Set the window title reading it from the resources.
            this.Caption = "JSONPath Evaluator";
            // Set the image that will appear on the tab of the window frame
            // when docked with an other window
            // The resource ID correspond to the one defined in the resx file
            // while the Index is the offset in the bitmap strip. Each image in
            // the strip being 16x16.
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on 
            // the object returned by the Content property.
            base.Content = new PathEvaluatorControl();
        }
    }
}
