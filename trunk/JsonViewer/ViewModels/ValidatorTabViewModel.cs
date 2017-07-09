using Marss.JsonViewer.Services;
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
    public class ValidatorTabViewModel : TabViewModelBase
    {
        public ValidatorTabViewModel()
        {
             Header = "Validator";
             ResourcePath = "Marss.JsonViewer.Resources.JsonParseResults.htm";
             WordWrap = true;

             ParseCommand = new GenericCommand<ValidatorTabViewModel, object>(this, Parse, CanParse);
             WordWrapCommand = new GenericCommand<ValidatorTabViewModel, object>(this, ChangeWordWrap, (vm, p) => true);
        }


        public override string UserControlName
        {
            get { return "ValidatorControl"; }
        }

        public string JsonToParse
        {
            get { return _jsonToParse; }
            set { SetProperty(ref _jsonToParse, value, "JsonToParse"); }
        }

        public bool WordWrap
        {
            get { return _wordWrap; }
            set
            {
                SetProperty(ref _wordWrap, value, "WordWrap");
                TextWrapping = _wordWrap ? TextWrapping.Wrap : TextWrapping.NoWrap;
            }
        }

        public TextWrapping TextWrapping
        {
            get { return _textWrapping; }
            private set { SetProperty(ref _textWrapping, value, "TextWrapping"); }
        }

        public string ResourcePath { get; private set; }

        public ICommand ParseCommand { get; private set; }
        public ICommand WordWrapCommand { get; private set; }

        public override bool CanBeRemoved
        {
            get { return false; }
        }

        public override bool Editable
        {
            get { return false; }
        }


        #region private

        private string _jsonToParse;
        private bool _wordWrap;
        private TextWrapping _textWrapping;
        private const string DisplayValidJsonFunctionName = "displaValidJson";
        private const string DisplayInvalidJsonFunctionName = "displayInvalidJson";

        private void Parse(ValidatorTabViewModel vm, object parameter)
        {
            try
            {
                var webBrowser = ((WebBrowser)parameter);

                string jsonErrorHtml;
                string errorMessage;
                if (JsonValidator.IsJsonValid(JsonToParse, out jsonErrorHtml, out errorMessage))
                {
                    webBrowser.InvokeScript(DisplayValidJsonFunctionName, JsonToParse, WordWrap);
                }
                else
                {
                    webBrowser.InvokeScript(DisplayInvalidJsonFunctionName, jsonErrorHtml, errorMessage, WordWrap);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while executing Javascript: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanParse(ValidatorTabViewModel vm, object parameter)
        {
            var browser = parameter as WebBrowser;
            return browser != null && browser.IsLoaded && !string.IsNullOrWhiteSpace(vm.JsonToParse);
        }

        private void ChangeWordWrap(ValidatorTabViewModel vm, object parameter)
        {
            if (CanParse(vm, parameter))
                Parse(vm, parameter);
        }

        #endregion
    }
}
