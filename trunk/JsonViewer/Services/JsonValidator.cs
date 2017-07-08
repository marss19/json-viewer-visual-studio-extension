
using System;
using System.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace Marss.JsonViewer.Services
{
    public class JsonValidator
    {
        public static bool IsJsonValid(string json, out string jsonErrorHtml, out string errorMessage)
        {
            jsonErrorHtml = null;
            errorMessage = null;

            try
            {
                var value = JsonValue.Parse(json);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                jsonErrorHtml = ConvertToHtmlWithHightlightedError(json, ex.Message);
                return false;
            }
        }

        #region private

        private static bool TryGetWrongCharacterPosition(string erroMessage, out int line, out int column)
        {
            var re = new Regex("At line (?<line>\\d+), column (?<column>\\d+).$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var match = re.Match(erroMessage);
            if (match.Success)
            {
                line = int.Parse(match.Groups["line"].Value) - 1;
                column = int.Parse(match.Groups["column"].Value);
            }
            else
            {
                line = -1;
                column = -1;
            }
            return match.Success;
        }

        private static string ConvertToHtmlWithHightlightedError(string json, string errorMessage)
        {
            try
            {
                int lineNumber;
                int linePosition;
                if (TryGetWrongCharacterPosition(errorMessage, out lineNumber, out linePosition))
                {
                    var re = new Regex($"^(?:[^\n]*[\n]){{{lineNumber}}}.{{{linePosition}}}", RegexOptions.Singleline);
                    var match = re.Match(json);
                    if (match.Success)
                    {
                        var pos = match.Length;

                        //hightlight few more character after the indicated error position
                        if (pos + 10 < json.Length)
                            json = json.Insert(pos + 10, "</span>");
                        else
                            json = json + "</span>";


                        //hightlight few more character before the indicated error position; move to the previous row if needed 
                        var startPos = pos >= 10 ? pos - 10 : 0;
                        while (char.IsWhiteSpace(json[startPos]) && startPos > 0)
                            startPos--;
                        json = json.Insert(startPos, "<span class='error'>");
                    }
                }
            }
            catch
            {
            }

            return json;
        }


        #endregion
    }
}
