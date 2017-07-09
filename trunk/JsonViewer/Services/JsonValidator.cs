
using System;
using System.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;

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

        //TODO: can this be implemented simpler?
        private const string StartErrorHightlightPlaceholder = "|||start highlighting|||";
        private const string EndErrorHightlightPlaceholder = "|||end highlighting|||";

        private static bool TryGetWrongCharacterPosition(string erroMessage, out int line, out int column)
        {
            var re = new Regex("At line (?<line>\\d+), column (?<column>\\d+).$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var match = re.Match(erroMessage);
            if (match.Success)
            {
                line = int.Parse(match.Groups["line"].Value) - 1;
                column = int.Parse(match.Groups["column"].Value) - 1;

                //there is a bug: the column is not a zero-based index but somethemes it shows "0"
                if (column < 0)
                    column = 0;
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

                        //hightlight few more characters after the indicated error position
                        if (pos + 10 < json.Length)
                            json = json.Insert(pos + 10, EndErrorHightlightPlaceholder);
                        else
                            json = json + EndErrorHightlightPlaceholder;


                        //hightlight few more characters before the indicated error position; move to the previous row if needed 
                        var startPos = pos >= 10 ? pos - 10 : 0;
                        while (char.IsWhiteSpace(json[startPos]) && startPos > 0)
                            startPos--;
                        json = json.Insert(startPos, StartErrorHightlightPlaceholder);
                    }
                }
            }
            catch
            {
            }

            return HtmlEncodeAndReplacePlaceholders(json);
        }

        
        private static string HtmlEncodeAndReplacePlaceholders(string json)
        {
            //do not use HtmlEncode as it changes quotes
            var jsonHtml = json.Replace("<", "&lt;").Replace(">", "&gt;");
            jsonHtml = jsonHtml
                .Replace(StartErrorHightlightPlaceholder, "<span class='error'>")
                .Replace(EndErrorHightlightPlaceholder, "</span>");
            return jsonHtml;
        }


        #endregion
    }
}
