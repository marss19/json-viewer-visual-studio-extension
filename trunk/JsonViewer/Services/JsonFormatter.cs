using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Marss.JsonViewer.Services
{
    public class JsonFormatter
    {
        public string FormatIfPossible(string json)
        {
            string errorMessage;
            return FormatIfPossible(json, out errorMessage);
        }

        public string FormatIfPossible(string json, out string errorMessage)
        {
            errorMessage = null;
            try
            {
                return Format(json);
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return json;
            }
        }

   
        #region private

        private string Format(string json)
        {
            if (string.IsNullOrEmpty(json))
                return json;

            var token = JToken.Parse(json);

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                using (var jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;
                    jw.Indentation = 4;
                    jw.IndentChar = ' ';

                    var serializer = new JsonSerializer();
                    serializer.Serialize(jw, token);

                    return sb.ToString();
                }
            }
        }


        #endregion
    }
}
