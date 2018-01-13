using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Marss.JsonViewer.Services
{
    public class TempFileManager
    {
        public static string GetTempFileFullPath()
        {
            var tempFolder = Path.GetTempPath();
            return Path.Combine(tempFolder, $"Json Viewer Temp File.{ Guid.NewGuid() }.json");
        }

        public static void PurgeOldTempFiles()
        {
            try
            {
                var tempFolder = Path.GetTempPath();
                var oldFiles = Directory.GetFiles(tempFolder, "Json Viewer Temp File.*.json");
                foreach (var filePath in oldFiles)
                {
                    File.Delete(filePath);
                }
            }
            catch
            {
            }
        }
    }
}
