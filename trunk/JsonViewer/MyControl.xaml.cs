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

namespace Marss.JsonViewer
{
    /// <summary>
    /// Interaction logic for MyControl.xaml
    /// </summary>
    public partial class MyControl : UserControl
    {
        public MyToolWindow _parent;

        public MyControl(MyToolWindow parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        private void FormatJson(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbData.Text))
                return;

            var json = tbData.Text;
            json =  json.Replace(Environment.NewLine, "").Replace("\\", "\\\\").Replace("\"", "\\\"");
            var html = ReadContentFromResources("Marss.JsonViewer.Resources.JsonView.htm");
            html = html.Replace("[InputDataPlaceholder]", json);

            var keepJsonMarkup = btnProgrammerFriendlyView.IsChecked;
            html = html.Replace("[KeepJsonMarkupPlaceholder]", keepJsonMarkup.HasValue && keepJsonMarkup.Value ? "true" : "false" );

            webBrowser.NavigateToString(html);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        private void PrintFormattedJson(object sender, RoutedEventArgs e)
        {
            var doc = webBrowser.Document as mshtml.IHTMLDocument2;
            if (doc != null)
                doc.execCommand("Print", true, null);
        }

        private String ReadContentFromResources(String pathToResource)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(pathToResource))
            {
                if (stream != null)
                {
                    return new StreamReader(stream).ReadToEnd();
                }
            }
            return string.Empty;
        }

        
    }
}