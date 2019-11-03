using Marss.JsonViewer.Helpers;
using Marss.JsonViewer.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Marss.JsonViewer.ViewModels
{
    public class SelectorWindowViewModel : BaseViewModel
    {
        public SelectorWindowViewModel()
        {
            OpenEmptyJsonFileCommand = new GenericCommand<SelectorWindowViewModel, object>(this, OpenEmptyJsonFile);
            OpenClipboardDataJsonFileCommand = new GenericCommand<SelectorWindowViewModel, object>(this, OpenClipboardDataJsonFile, CanOpenClipboardDataJsonFile);
            CompareJsonsCommand = new GenericCommand<SelectorWindowViewModel, object>(this, CompareJsons);
            OpenPathEvaluatorCommand = new GenericCommand<SelectorWindowViewModel, object>(this, OpenPathEvaluator);
        }

        public ICommand OpenEmptyJsonFileCommand { get; private set; }
        public ICommand OpenClipboardDataJsonFileCommand { get; private set; }
        public ICommand CompareJsonsCommand { get; private set; }
        public ICommand OpenPathEvaluatorCommand { get; private set; }

        public Window Window { get; set; }

        private void OpenEmptyJsonFile(SelectorWindowViewModel model, object parameter)
        {
            OpenTextInNewTab("");
            Window.Close();
        }

        private void OpenClipboardDataJsonFile(SelectorWindowViewModel model, object parameter)
        {
            OpenTextInNewTab(Clipboard.GetText());
            Window.Close();
        }

        private bool CanOpenClipboardDataJsonFile(SelectorWindowViewModel model, object parameter)
        {
            return Clipboard.ContainsText();
        }

        private void CompareJsons(SelectorWindowViewModel model, object parameter)
        {
            VsExtensionHelper.ShowToolWindow<ComparerToolWindowPane>();
            Window.Close();
        }

        private void OpenPathEvaluator(SelectorWindowViewModel model, object parameter)
        {
            VsExtensionHelper.ShowToolWindow<PathEvaluatorToolWindowPane>();
            Window.Close();
        }

        private void OpenTextInNewTab(string text)
        {
            TempFileManager.PurgeOldTempFiles();
            var path = TempFileManager.GetTempFileFullPath();

            var formattedText = JsonFormatter.FormatIfPossible(text);
            File.WriteAllText(path, formattedText);
 
            VsExtensionHelper.OpenFile(path);
        }
    }
}
