using Marss.JsonViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Marss.JsonViewer.Views
{
    /// <summary>
    /// Interaction logic for JsonPathEvaluatorControl.xaml
    /// </summary>
    public partial class JsonPathEvaluatorControl : UserControl
    {
        public JsonPathEvaluatorControl()
        {
            InitializeComponent();

            DataObject.AddPastingHandler(tbInput, OnPaste);
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            var isText = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
            if (!isText)
                return;

            var text = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string;

            ((JsonPathEvaluatorTabViewModel)DataContext).TextPasted();
        }
    }
}
