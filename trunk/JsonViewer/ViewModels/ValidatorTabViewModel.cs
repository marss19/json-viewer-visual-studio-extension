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

             ParseCommand = new GenericCommand<ValidatorTabViewModel, object>(this, Parse, CanParse);
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

        public string ResourcePath { get; private set; }

        public ICommand ParseCommand { get; private set; }

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
                    webBrowser.InvokeScript(DisplayValidJsonFunctionName, JsonToParse);
                }
                else
                {
                    webBrowser.InvokeScript(DisplayInvalidJsonFunctionName, jsonErrorHtml, errorMessage);
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

        #endregion
    }
}
