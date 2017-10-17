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
    public class JsonPathEvaluatorTabViewModel : TabViewModelBase
    {
        public JsonPathEvaluatorTabViewModel()
        {
            Header = "JSONPath Evaluator";
            Results = Enumerable.Empty<string>();

            FindCommand = new GenericCommand<JsonPathEvaluatorTabViewModel, object>(this, Find, CanFind);
         }


        public override string UserControlName
        {
            get { return "JsonPathEvaluatorControl"; }
        }

        public string Expression
        {
            get { return _expression; }
            set { SetProperty(ref _expression, value, "Expression"); }
        }

        public string Source
        {
            get { return _source; }
            set
            {
                ValidateJson(value);
                if (_textPasted) //format text if it is pasted
                {
                    _textPasted = false;
                    value = new JsonFormatter().FormatIfPossible(value);
                }
                SetProperty(ref _source, value, "Source");
            }
        }

        public IEnumerable<string> Results
        {
            get { return _results; }
            set { SetProperty(ref _results, value, "Results"); }
        }

        public void TextPasted()
        {
            _textPasted = true;
        }

        public ICommand FindCommand { get; private set; }

        public override bool CanBeRemoved
        {
            get { return false; }
        }

        public override bool Editable
        {
            get { return false; }
        }


        #region private

        private string _source;
        private string _expression;
        private IEnumerable<string> _results;
        private bool _textPasted;

        private void Find(JsonPathEvaluatorTabViewModel vm, object parameter)
        {
            try
            {
                var input = JToken.Parse(Source);
                var results = input.SelectTokens(Expression);
                Results = results.Select(x => x.ToString());

                Message = Results.Any() ? "" : " Nothing found. " ;
            }
            catch (Exception e)
            {
                Results = Enumerable.Empty<string>();

                Message = $" Failed to search using the expression. {e.Message} ";
            }
        }

        private bool CanFind(JsonPathEvaluatorTabViewModel vm, object parameter)
        {
            return !string.IsNullOrWhiteSpace(vm.Source) && !string.IsNullOrWhiteSpace(vm.Expression);
        }

        private void ValidateJson(string text)
        {
            try
            {
                JToken.Parse(text);
                Message = "";
            }
            catch (Exception ex)
            {
                Message = $" Invalid JSON. {ex.Message} ";
            }
        }

        #endregion
    }
}