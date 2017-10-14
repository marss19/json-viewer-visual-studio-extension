using Marss.JsonViewer.Services;
using Marss.JsonViewer.ViewModels.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace Marss.JsonViewer.ViewModels
{

    public class DefaultViewerTabViewModel : TabViewModelBase
    {
        public DefaultViewerTabViewModel()
        {
            Header = "Formatter & Validator";

            FormatProgrammerFriendlyCommand = new GenericCommand<DefaultViewerTabViewModel, object>(this, FormatProgrammerFriendly, CanFormat);
            FormatHumanFriendlyCommand = new GenericCommand<DefaultViewerTabViewModel, object>(this, FormatHumanFriendly, CanFormat);

            PrintCommand = new GenericCommand<DefaultViewerTabViewModel, object>(this, Print, CanPrint);
        }


        public override string UserControlName
        {
            get { return "DefaultViewerControl"; }
        }

        public string UnformattedJson
        {
            get { return _unformattedJson; }
            set { SetProperty(ref _unformattedJson, value, "UnformattedJson"); }
        }

        public ICommand FormatProgrammerFriendlyCommand { get; private set; }
        public ICommand FormatHumanFriendlyCommand { get; private set; }
        public ICommand PrintCommand { get; private set; }

        public override bool CanBeRemoved
        {
            get { return false; }
        }

        public override bool Editable
        {
            get { return false; }
        }

        #region private

        private string _unformattedJson;
        private string _formattedJson;

        private void FormatProgrammerFriendly(DefaultViewerTabViewModel vm, object parameter)
        {
            var tbViewer = ((List<object>)parameter)[0] as FlowDocumentScrollViewer;
            var tbInput = ((List<object>)parameter)[1] as TextBox;

            string errorMessage;
            var formattedText = new JsonFormatter().FormatIfPossible(UnformattedJson, out errorMessage);
            var doc = new FlowDocument(new Paragraph(new Run(formattedText ?? string.Empty)));
            Format(doc, errorMessage, tbViewer, tbInput);
        }

        private void FormatHumanFriendly(DefaultViewerTabViewModel vm, object parameter)
        {
            var tbViewer = ((List<object>)parameter)[0] as FlowDocumentScrollViewer;
            var tbInput = ((List<object>)parameter)[1] as TextBox;

            string errorMessage;
            var doc = new JsonCustomFormatter().FormatIfPossible(UnformattedJson, out errorMessage);
            Format(doc, errorMessage, tbViewer, tbInput);
        }

        private void Format(FlowDocument doc, string errorMessage, FlowDocumentScrollViewer viewer, TextBox input)
        {
            viewer.Document = doc;
            viewer.Document.FontSize = viewer.FontSize;
            viewer.Document.FontFamily = viewer.FontFamily;

            if (!string.IsNullOrEmpty(errorMessage))
                HighlightIncorrectJson(UnformattedJson, errorMessage, input);

            Message = errorMessage != null ? $" Failed to format. {errorMessage} " : "";
        }

        private bool CanFormat(DefaultViewerTabViewModel vm, object parameter)
        {
            return !string.IsNullOrWhiteSpace(vm.UnformattedJson);
        }

        private void Print(DefaultViewerTabViewModel vm, object parameter)
        {
            var tbViewer = parameter as FlowDocumentScrollViewer;

            //needs to be cloned to preserve the color scheme on UI
            FlowDocument clone;
            var str = XamlWriter.Save(tbViewer.Document);

            using(var sr = new StringReader(str))
            {
                using(var xr = XmlReader.Create(sr))
                {
                    clone = (FlowDocument)XamlReader.Load(xr);
                }
            }
 
            clone.Background = Brushes.White;
            clone.Foreground = Brushes.Black;

            var printDlg = new PrintDialog();
            printDlg.PrintDocument(((IDocumentPaginatorSource)clone).DocumentPaginator, "JSON Viewer");
        }

        private bool CanPrint(DefaultViewerTabViewModel vm, object parameter)
        {
            if (parameter == null)
                return false;

            var tbViewer = parameter as FlowDocumentScrollViewer;
            return tbViewer.Document != null; 
        }

        private  bool TryGetWrongCharacterPosition(string erroMessage, out int line, out int column)
        {
            line = -1;
            column = -1;

            var re = new Regex("line (?<line>\\d+), position (?<column>\\d+).$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var match = re.Match(erroMessage);
            if (match.Success)
            {
                line = int.Parse(match.Groups["line"].Value);
                column = int.Parse(match.Groups["column"].Value);

                //the column and the line are not a zero-based indexes but somethemes they show "0"
                if (line > 0) line--;
                if (column > 0) column--;
            }
            return match.Success;
        }

        private void HighlightIncorrectJson(string json, string errorMessage, TextBox tb)
        {
            int line;
            int position;
            if (TryGetWrongCharacterPosition(errorMessage, out line, out position))
            {
                var re = new Regex($"^(?:[^\n]*[\n]){{{line}}}.{{{position}}}", RegexOptions.Singleline);
                var match = re.Match(json);
                if (match.Success)
                {
                    var pos = match.Length;

                    //hightlight few more characters after the indicated error position
                    var end = pos;
                    var count = 5;
                    while (count > 0 && end < json.Length)
                    {
                        if (!char.IsWhiteSpace(json[end]))
                            count--;
                        end++;
                    }

                    //hightlight few more characters before the indicated error position; move to the previous row if needed 
                    var start = pos;
                    count = 5;
                    while (count > 0 && start > 0)
                    {
                        if (!char.IsWhiteSpace(json[start]))
                            count--;
                        start--;
                    }

                    tb.Focus();
                    tb.Select(start, end - start);
                }
                else
                {
                    var totalLinesCount = json.Split('\n').Count();
                    //sometimes line number is bigger that actual count of lines,
                    //e.g. in cases when the closing bracket is missed
                    if (line >= totalLinesCount)
                    {
                        var start = json.Length - 1;
                        var count = 5;
                        while (count > 0 && start > 0)
                        {
                            if (!char.IsWhiteSpace(json[start]))
                                count--;
                            start--;
                        }

                        tb.Focus();
                        tb.Select(start, json.Length - start);
                    }
                }
            }

        }

        #endregion
    }
}
