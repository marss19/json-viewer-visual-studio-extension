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
    /// Interaction logic for ComparerControl.xaml
    /// </summary>
    public partial class ComparerControl : UserControl
    {
        public ComparerControl()
        {
            InitializeComponent();
            DataContext = new ComparerControlViewModel();
        }
    }
}
