using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Marss.JsonViewer.Forms
{
    public static class HtmlBuilder
    {
        public static string PrepateHtml(string json, bool keepJsonMarkup)
        {
            json = json.Replace(Environment.NewLine, "").Replace("\\", "\\\\").Replace("\"", "\\\"");
            var html = ReadContentFromResources("Marss.JsonViewer.Resources.JsonView.htm");
            html = html.Replace("[InputDataPlaceholder]", json);

            return html.Replace("[KeepJsonMarkupPlaceholder]", keepJsonMarkup.ToString().ToLower());
        }

        #region private

        private static string ReadContentFromResources(string pathToResource)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(pathToResource))
            {
                if (stream != null)
                {
                    return new StreamReader(stream).ReadToEnd();
                }
            }
            return string.Empty;
        }

        #endregion
    }
}
