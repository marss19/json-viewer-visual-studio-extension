using Marss.JsonViewer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Windows;

namespace Marss.JsonViewer.ViewModels
{
    public class ComparerControlViewModel : BaseViewModel
    {
        public ComparerControlViewModel()
        {
            Json1Name = "Left";
            Json2Name = "Right";

            CompareCommand = new GenericCommand<ComparerControlViewModel, object>(this, Compare, CanCompare);
            CompareFormattedCommand = new GenericCommand<ComparerControlViewModel, object>(this, CompareFormatted, CanCompare);
        }

        public string Json1Name
        {
            get { return _json1Name; }
            set { SetProperty(ref _json1Name, value, "Json1Name"); }
        }

        public string Json1Content
        {
            get { return _json1Content; }
            set { SetProperty(ref _json1Content, value, "Json1Content"); }
        }

        public string Json2Name
        {
            get { return _json2Name; }
            set { SetProperty(ref _json2Name, value, "Json2Name"); }
        }

        public string Json2Content
        {
            get { return _json2Content; }
            set { SetProperty(ref _json2Content, value, "Json2Content"); }
        }


        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value, "ErrorMessage"); }
        }

        public ICommand CompareCommand { get; private set; }
        public ICommand CompareFormattedCommand { get; private set; }


        #region private

        private string _json1Name;
        private string _json1Content;

        private string _json2Name;
        private string _json2Content;

        private string _errorMessage;

        private void Compare(ComparerControlViewModel vm, object parameter)
        {
            ErrorMessage = null;
            try
            {
                JsonComparer.Compare(vm.Json1Content, vm.Json1Name, vm.Json2Content, vm.Json2Name);
            }
            catch (Exception e)
            {
                ErrorMessage = $"Failed to compare. {e.Message}";
            }
        }

        private void CompareFormatted(ComparerControlViewModel vm, object parameter)
        {
            ErrorMessage = null;
            string error;
            string formattedJsonl = JsonFormatter.FormatIfPossible(vm.Json1Content, out error);
            if (error != null)
            {
                ErrorMessage = $"Invalid JSON in \"{vm.Json1Name}\". {error}";
                return;
            }

            string formattedJson2 = JsonFormatter.FormatIfPossible(vm.Json2Content, out error);
            if (error != null)
            {
                ErrorMessage = $"Invalid JSON in \"{vm.Json2Name}\". {error}";
                return;
            }

            try
            {
                JsonComparer.Compare(formattedJsonl, vm.Json1Name, formattedJson2, vm.Json2Name);
            }
            catch (Exception e)
            {
                ErrorMessage = $"Failed to compare. {e.Message}";
            }
        }

        private bool CanCompare(ComparerControlViewModel vm, object parameter)
        {
            return !string.IsNullOrWhiteSpace(vm.Json1Name)
                && !string.IsNullOrWhiteSpace(vm.Json1Content)
                && !string.IsNullOrWhiteSpace(vm.Json2Name)
                && !string.IsNullOrWhiteSpace(vm.Json2Content);
        }


        #endregion
    }
}
