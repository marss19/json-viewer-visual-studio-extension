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
    class JsonCustomFormatter
    {
        public FlowDocument FormatIfPossible(string json, out string errorMessage)
        {
            errorMessage = null;

            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var token = JToken.Parse(json);

                    return new FlowDocument(new List(Get(token)) { MarkerStyle = TextMarkerStyle.None, Margin = _margin, Padding = _noneListPadding });
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }
            }
            return new FlowDocument(new Paragraph(new Run(json ?? string.Empty)));
        }

        #region private

        private Thickness _margin = new Thickness(0);
        private Thickness _decimalListPadding = new Thickness(30, 0, 0, 0);
        private Thickness _noneListPadding = new Thickness(10, 0, 0, 0);
        private double _markerOffset = 0;

        private List Get(JObject obj)
        {
            var list = new List() { MarkerStyle = TextMarkerStyle.None, Margin = _margin, Padding = _noneListPadding };
            var properties = obj.Properties();
            foreach (var property in properties)
            {
                list.ListItems.Add(Get(property));
            }
            return list;
        }

        private List Get(JArray array)
        {
            var list = new List() { MarkerStyle = TextMarkerStyle.Decimal, Margin = _margin, Padding = _decimalListPadding, MarkerOffset= _markerOffset };
            foreach (JToken element in array)
            {
                list.ListItems.Add(Get(element));
            }
            return list;
        }

        private ListItem Get(JToken token)
        {
            if (token is JValue)
            {
                return Get((JValue)token);
            }
            else if (token is JArray)
            {
                var li = new ListItem();
                li.Blocks.Add(Get((JArray)token));
                return li;
            }
            else if (token is JObject)
            {
                var li = new ListItem();
                li.Blocks.Add(Get((JObject)token));
                return li;
            }
            else if (token is JProperty)
            {
                return Get((JProperty)token);
            }
            else if (token is JConstructor)
            {
                return Get((JConstructor)token);
            }
            else
            {
                return new ListItem(new Paragraph(new Run($"{token}")));
            }
        }

        private ListItem Get(JProperty property)
        {
            if (property.Value is JObject || property.Value is JArray)
            {
                var li = new ListItem();
                li.Blocks.Add(new Paragraph(new Run($"{property.Name}: ") { FontWeight = FontWeights.Bold }));
                li.Blocks.Add(property.Value is JObject ? Get((JObject)property.Value) : Get((JArray)property.Value));
                return li;
            }
            else
            {
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run($"{property.Name}: ") { FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run($"{property.Value}"));
                return new ListItem(paragraph);
            }
        }

        private ListItem Get(JValue obj)
        {
            return new ListItem(new Paragraph(new Run($"{obj.Value}")));
        }

        private ListItem Get(JConstructor constructor)
        {
            return new ListItem(new Paragraph(new Run($"{constructor.ToString()}")));
        }

        #endregion
    }
}
