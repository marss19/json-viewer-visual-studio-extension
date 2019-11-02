using Marss.JsonViewer.Helpers;
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
    class PathEvaluatorControlViewModel : BaseViewModel
    {
        public PathEvaluatorControlViewModel()
        {
            Results = Array.Empty<string>();

            FindCommand = new GenericCommand<PathEvaluatorControlViewModel, object>(this, Find, CanFind);
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
                SetProperty(ref _source, value, "Source");
            }
        }

        public IEnumerable<string> Results
        {
            get { return _results; }
            set { SetProperty(ref _results, value, "Results"); }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value, "ErrorMessage"); }
        }

        public ICommand FindCommand { get; private set; }


        #region private

        private string _source;
        private string _expression;
        private IEnumerable<string> _results;
        private string _errorMessage;

        private void Find(PathEvaluatorControlViewModel vm, object parameter)
        {
            Results = Array.Empty<string>();
            try
            {
                var input = JToken.Parse(Source);
                var results = input.SelectTokens(Expression);
                Results = results.Select(x => x.ToString()).ToArray();

                ErrorMessage = Results.Any() ? null :"Nothing found.";
            }
            catch (Exception e)
            {
                ErrorMessage = $"Search failed. {e.Message}";
            }
        }

        private bool CanFind(PathEvaluatorControlViewModel vm, object parameter)
        {
            return !string.IsNullOrWhiteSpace(vm.Source) && !string.IsNullOrWhiteSpace(vm.Expression);
        }

        private void ValidateJson(string text)
        {
            JsonFormatter.FormatIfPossible(text, out string error);

            Results = Enumerable.Empty<string>();
            ErrorMessage = string.IsNullOrEmpty(error) ? null : $"Invalid JSON format. {error}";
        }

        #endregion
    }


}
