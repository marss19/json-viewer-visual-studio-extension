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
using System.Windows.Shapes;

namespace Marss.JsonViewer.Forms
{
    /// <summary>
    /// Interaction logic for ComparisonWindow.xaml
    /// </summary>
    public partial class ComparisonWindow : Window
    {
        public ComparisonWindow()
        {
            InitializeComponent();
        }

        public void SetInitialData(IServiceProvider serviceProvider, string json1)
        {
            _serviceProvider = serviceProvider;
            tbFirstData.Text = json1;
        }

        private IServiceProvider _serviceProvider;

        private void Compare(object sender, RoutedEventArgs e)
        {
            try
            {
                var comparer = new JsonComparer(_serviceProvider);
                comparer.Compare(tbFirstData.Text, tbSecondData.Text);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InputDataChanged(object sender, TextChangedEventArgs e)
        {
            btnCompare.IsEnabled = !string.IsNullOrWhiteSpace(tbFirstData.Text) && !string.IsNullOrWhiteSpace(tbSecondData.Text);
        }

    }
}
