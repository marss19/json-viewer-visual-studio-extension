using Marss.JsonViewer.Services;
using Marss.JsonViewer.ViewModels.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Marss.JsonViewer.ViewModels
{

    public class DefaultViewerTabViewModel : TabViewModelBase
    {
        public DefaultViewerTabViewModel()
        {
            Header = "Formatter & Validator";

            FormatProgrammerFriendlyCommand = new GenericCommand<DefaultViewerTabViewModel, object>(this, FormatProgrammerFriendly, CanFormat);
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

        public string FormattedJson
        {
            get { return _formattedJson; }
            set { SetProperty(ref _formattedJson, value, "FormattedJson"); }
        }

        public ICommand FormatProgrammerFriendlyCommand { get; private set; }
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
            string errorMessage;
            FormattedJson = JsonFormatter.FormatIfPossible(UnformattedJson, out errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
                HighlightIncorrectJson(UnformattedJson, errorMessage, (TextBox)parameter);

            Message = errorMessage != null ? $" Failed to format. {errorMessage} " : "";
        }

        private bool CanFormat(DefaultViewerTabViewModel vm, object parameter)
        {
            return !string.IsNullOrWhiteSpace(vm.UnformattedJson);
        }

        private void Print(DefaultViewerTabViewModel vm, object parameter)
        {
            var doc = new FlowDocument(new Paragraph(new Run(FormattedJson)));
            doc.PagePadding = new Thickness(100);

            var printDlg = new PrintDialog();
            printDlg.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "JSON Viewer");
        }

        private bool CanPrint(DefaultViewerTabViewModel vm, object parameter)
        {
            return !string.IsNullOrWhiteSpace(vm.FormattedJson); 
        }

        private  bool TryGetWrongCharacterPosition(string erroMessage, out int line, out int column)
        {
            line = -1;
            column = -1;

            var re = new Regex("line (?<line>\\d+), position (?<column>\\d+).$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var match = re.Match(erroMessage);
            if (match.Success)
            {
                line = int.Parse(match.Groups["line"].Value) - 1;
                column = int.Parse(match.Groups["column"].Value) - 1;

                //there is a bug: the column is not a zero-based index but somethemes it shows "0"
                if (column < 1)
                    column = 1;
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
                    var end = pos + 10 < json.Length ? pos + 10 : pos;
                    while (char.IsWhiteSpace(json[end]) && end < json.Length)
                        end++;

                    //hightlight few more characters before the indicated error position; move to the previous row if needed 
                    var start = pos >= 10 ? pos - 10 : 0;
                    while (char.IsWhiteSpace(json[start]) && start > 0)
                        start--;

                    tb.Focus();
                    tb.Select(start, end - start);

                }
            }

        }

        #endregion
    }
}
