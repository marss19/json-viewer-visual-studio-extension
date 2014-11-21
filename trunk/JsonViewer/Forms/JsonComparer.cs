using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Marss.JsonViewer.Forms
{
    
    public class JsonComparer
    {
        public JsonComparer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public void Compare(string unformatterJson1, string unformatterJson2)
        {
            var formattedJsonl = VerifyAndFormatJson(unformatterJson1, "1st JSON data");
            var formattedJson2 = VerifyAndFormatJson(unformatterJson2, "2nd JSON data");

            var tempFolder = Path.GetTempPath();
            PurgeOldTempFiles(tempFolder);

            var tempFile1 = Path.Combine(tempFolder, string.Format(TempFileNamePattern, Guid.NewGuid()));
            File.WriteAllText(tempFile1, formattedJsonl);

            var tempFile2 = Path.Combine(tempFolder, string.Format(TempFileNamePattern, Guid.NewGuid()));
            File.WriteAllText(tempFile2, formattedJson2);


            DTE dte = (DTE)_serviceProvider.GetService(typeof(DTE));
            dte.ExecuteCommand("Tools.DiffFiles", string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\"", tempFile1, tempFile2, "1st JSON data", "2nd JSON data"));
        }

        #region private

        private const string TempFileNamePattern = "Json Viewer Temp File {0}.txt";
        private IServiceProvider _serviceProvider;

        private static void PurgeOldTempFiles(string tempFolder)
        {
            try
            {
                var oldFiles = Directory.GetFiles(tempFolder, string.Format(TempFileNamePattern, "*"));
                foreach (var oldFile in oldFiles)
                {
                    File.Delete(oldFile);
                }
            }
            catch
            {
            }
        }

        private string VerifyAndFormatJson(string input, string inputName)
        {
            try
            {
                return Stringify(input);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to format {0}. Error: {1}", inputName, ex.Message));
            }
        }

        private string Stringify(string unformatterJson)
        {
            const char Indent = '\t';

            var indent = 0;
            var quoted = false;
            var sb = new StringBuilder();

            for (var i = 0; i < unformatterJson.Length; i++)
            {
                var c = unformatterJson[i];
                switch (c)
                {
                    case '{':
                    case '[':
                        sb.Append(c);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            sb.Append(Indent, ++indent);
                        }
                        break;

                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            sb.Append(Indent, --indent);
                        }
                        sb.Append(c);
                        break;

                    case '"':
                        sb.Append(c);
                        bool escaped = false;
                        var index = i;
                        while (index > 0 && unformatterJson[--index] == '\\')
                            escaped = !escaped;
                        if (!escaped)
                            quoted = !quoted;
                        break;

                    case ',':
                        sb.Append(c);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            sb.Append(Indent, indent);
                        }
                        break;

                    case ':':
                        sb.Append(c);
                        if (!quoted)
                            sb.Append(" ");
                        break;

                    default:
                        if (quoted || !Char.IsWhiteSpace(c))
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }

            return sb.ToString();
        }

        #endregion

    }
}
