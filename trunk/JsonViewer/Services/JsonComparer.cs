using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Marss.JsonViewer.Services
{
    
    public class JsonComparer
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static void Compare(string json1, string json1Name, string json2, string json2Name)
        {
            TempFileManager.PurgeOldTempFiles();

            var tempFile1 = TempFileManager.GetTempFileFullPath();
            File.WriteAllText(tempFile1, json1);

            var tempFile2 = TempFileManager.GetTempFileFullPath();
            File.WriteAllText(tempFile2, json2);

            DTE dte = (DTE)ServiceProvider.GetService(typeof(DTE));
            dte.ExecuteCommand("Tools.DiffFiles", string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\"", tempFile1, tempFile2, json1Name, json2Name));
        }

    }
}
