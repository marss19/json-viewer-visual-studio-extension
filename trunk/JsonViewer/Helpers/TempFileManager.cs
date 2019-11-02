using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marss.JsonViewer.Helpers
{
    public class TempFileManager
    {
        public const string FileNameTemplate = "{0}.JSONViewer2 temp file.json";

        public static string GetTempFileFullPath()
        {
            var tempFolder = Path.GetTempPath();
            return Path.Combine(tempFolder, string.Format(FileNameTemplate, Guid.NewGuid()));
        }

        public static void PurgeOldTempFiles()
        {
            try
            {
                var tempFolder = Path.GetTempPath();
                var oldFiles = Directory.GetFiles(tempFolder, string.Format(FileNameTemplate, "*"));
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
