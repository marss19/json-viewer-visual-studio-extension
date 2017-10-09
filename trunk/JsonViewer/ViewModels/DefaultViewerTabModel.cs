using Marss.JsonViewer.Services;
using Marss.JsonViewer.ViewModels.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Header = "Formatter";

            FormatHumanFriendlyCommand = new GenericCommand<DefaultViewerTabViewModel, object>(this, FormatHumanFriendly, CanFormat);
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

        public ICommand FormatHumanFriendlyCommand { get; private set; }
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

        private void Format(bool humanFriendly)
        {
            string errorMessage;
            FormattedJson = JsonFormatter.FormatIfPossible(UnformattedJson, out errorMessage);
            Message = errorMessage != null ? $" Failed to format. {errorMessage} " : "";
        }


        private void FormatHumanFriendly(DefaultViewerTabViewModel vm, object parameter)
        {
           // Format(true);
        }

        private void FormatProgrammerFriendly(DefaultViewerTabViewModel vm, object parameter)
        {
            Format(false);
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

        #endregion
    }
}
