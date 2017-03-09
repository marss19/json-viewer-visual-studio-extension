using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marss.JsonViewer.Services
{
    public class JsonFormatter
    {
        public static string Format(string input, string inputName)
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

        private static string Stringify(string unformattedJson)
        {
            var indent = 0;
            var quoted = false;
            var sb = new StringBuilder();

            for (var i = 0; i < unformattedJson.Length; i++)
            {
                var c = unformattedJson[i];
                switch (c)
                {
                    case '{':
                    case '[':
                        sb.Append(c);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            sb.Append('\t', ++indent);
                        }
                        break;

                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            sb.Append('\t', --indent);
                        }
                        sb.Append(c);
                        break;

                    case '"':
                        sb.Append(c);
                        bool escaped = false;
                        var index = i;
                        while (index > 0 && unformattedJson[--index] == '\\')
                            escaped = !escaped;
                        if (!escaped)
                            quoted = !quoted;
                        break;

                    case ',':
                        sb.Append(c);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            sb.Append('\t', indent);
                        }
                        break;

                    case ':':
                        sb.Append(c);
                        if (!quoted)
                            sb.Append(" ");
                        break;

                    default:
                        if (quoted || !char.IsWhiteSpace(c))
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }

            return sb.ToString();
        }
    }
}
