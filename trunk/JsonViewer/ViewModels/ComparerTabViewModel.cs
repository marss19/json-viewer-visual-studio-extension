using Marss.JsonViewer.Services;
using Marss.JsonViewer.ViewModels.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Marss.JsonViewer.ViewModels
{
    public class ComparerTabViewModel : TabViewModelBase
    {
        public ComparerTabViewModel()
        {
            Header = "Comparer";

            Json1Name = "JSON #1";
            Json2Name = "JSON #2";

            CompareCommand = new GenericCommand<ComparerTabViewModel, object>(this, Compare, CanCompare);
            CompareFormattedCommand = new GenericCommand<ComparerTabViewModel, object>(this, CompareFormatted, CanCompare);

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

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value, "Message"); }
        }

        public ICommand CompareCommand { get; private set; }
        public ICommand CompareFormattedCommand { get; private set; }

        public override string UserControlName
        {
            get { return "ComparerControl"; }
        }

        public override bool CanBeRemoved
        {
            get { return false; }
        }

        public override bool Editable
        {
            get { return false; }
        }

        #region private

        public string _json1Name;
        public string _json1Content;

        public string _json2Name;
        public string _json2Content;

        private string _message;

        private void Compare(ComparerTabViewModel vm, object parameter)
        {
            try
            {
                JsonComparer.Compare(vm.Json1Content, vm.Json1Name, vm.Json2Content, vm.Json2Name);
                Message = "";
            }
            catch (Exception e)
            {
                Message = $"Failed to compare JSONs. {e.Message} ";
            }
        }

        private void CompareFormatted(ComparerTabViewModel vm, object parameter)
        {
            string formattedJsonl;
            string formattedJson2;
            try
            {
                formattedJsonl = Format(vm.Json1Content, vm.Json1Name);
                formattedJson2 = Format(vm.Json2Content, vm.Json2Name);
            }
            catch(Exception e)
            {
                Message = $"{e.Message} Correct JSON or compare without preliminary formatting. ";
                return;
            }

            try
            {
                JsonComparer.Compare(formattedJsonl, vm.Json1Name, formattedJson2, vm.Json2Name);
                Message = "";
            }
            catch (Exception e)
            {
                Message = $"Failed to compare JSONs. {e.Message} ";
            }
        }

        private bool CanCompare(ComparerTabViewModel vm, object parameter)
        {
            return !string.IsNullOrWhiteSpace(vm.Json1Name)
                && !string.IsNullOrWhiteSpace(vm.Json1Content)
                && !string.IsNullOrWhiteSpace(vm.Json2Name)
                && !string.IsNullOrWhiteSpace(vm.Json2Content);
        }

        private string Format(string json, string jsonName)
        {
            try
            {
                return JToken.Parse(json).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to format {jsonName}. {ex.Message}");
            }
        }

        #endregion
    }
}
