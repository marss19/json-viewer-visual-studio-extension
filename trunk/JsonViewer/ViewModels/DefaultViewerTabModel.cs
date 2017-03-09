using Marss.JsonViewer.ViewModels.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Marss.JsonViewer.ViewModels
{
    public class DefaultViewerTabViewModel : TabViewModelBase
    {
        public DefaultViewerTabViewModel()
        {
            Header = "Formatter";
            ResourcePath = "Marss.JsonViewer.Resources.JsonView.htm";

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

        public string ResourcePath { get; private set; }

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

        private void Format(WebBrowser webBrowser, string javascriptFunctionName)
        {
            try
            {
                webBrowser.InvokeScript(javascriptFunctionName, UnformattedJson);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while executing Javascript: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void FormatHumanFriendly(DefaultViewerTabViewModel vm, object parameter)
        {
            Format((WebBrowser)parameter, "customFormat");
        }

        private void FormatProgrammerFriendly(DefaultViewerTabViewModel vm, object parameter)
        {
            Format((WebBrowser)parameter, "defaultFormat");
        }

        private bool CanFormat(DefaultViewerTabViewModel vm, object parameter)
        {
            var browser = parameter as WebBrowser;
            return browser != null && browser.IsLoaded && !string.IsNullOrWhiteSpace(vm.UnformattedJson);
        }


        private void Print(DefaultViewerTabViewModel vm, object parameter)
        {
            var doc = ((WebBrowser)parameter).Document as mshtml.IHTMLDocument2;
            if (doc != null)
                doc.execCommand("Print", true, null);
        }

        private bool CanPrint(DefaultViewerTabViewModel vm, object parameter)
        {
            var browser = parameter as WebBrowser;
            return browser != null ? browser.IsLoaded : false;
        }

        #endregion
    }
}
