using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Marss.JsonViewer.Forms
{
    /// <summary>
    /// Interaction logic for MyControl.xaml
    /// </summary>
    public partial class MainForm : UserControl
    {
        public MyToolWindow _parent;

        public MainForm(MyToolWindow parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        private void FormatJson(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbData.Text))
                return;

            var html = HtmlBuilder.PrepateHtml(tbData.Text, btnProgrammerFriendlyView.IsChecked.HasValue && btnProgrammerFriendlyView.IsChecked.Value);
            webBrowser.NavigateToString(html);
        }

        private void PrintFormattedJson(object sender, RoutedEventArgs e)
        {
            var doc = webBrowser.Document as mshtml.IHTMLDocument2;
            if (doc != null)
                doc.execCommand("Print", true, null);
        }

        private void CompareToAnotherJson(object sender, RoutedEventArgs e)
        {
            var comparisonWindow = new ComparisonWindow();
            comparisonWindow.SetInitialData((IServiceProvider)_parent.Package, tbData.Text);
            comparisonWindow.ShowDialog();
        }
        
    }
}