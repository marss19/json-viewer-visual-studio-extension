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

            tbData.Focus();
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

        private void SplitHorizontal(object sender, RoutedEventArgs e)
        {
            grid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions[1].Width = new GridLength(0);

            grid.RowDefinitions[0].Height = new GridLength(200);
            grid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);

            Grid.SetColumn(pnlFormattedView, 0);
            Grid.SetRow(pnlFormattedView, 1);
            pnlFormattedView.Margin = new Thickness(5); 

            splitter.ResizeDirection = GridResizeDirection.Rows;
            splitter.Height = 5;
            splitter.Width = Double.NaN;
            splitter.HorizontalAlignment = HorizontalAlignment.Stretch;
            splitter.VerticalAlignment =  VerticalAlignment.Bottom;
            splitter.Margin = new Thickness(5, 0, 5, 0);
            splitter.Cursor = Cursors.SizeNS;

            Grid.SetColumnSpan(splitter, 2);
            Grid.SetRowSpan(splitter, 1);
        }

        private void SplitVertical(object sender, RoutedEventArgs e)
        {
            grid.ColumnDefinitions[0].Width = new GridLength(200);
            grid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

            grid.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
            grid.RowDefinitions[1].Height = new GridLength(0);
            
            Grid.SetColumn(pnlFormattedView, 1);
            Grid.SetRow(pnlFormattedView, 0);
            pnlFormattedView.Margin = new Thickness(0, 5, 5, 5); 

            splitter.ResizeDirection = GridResizeDirection.Columns;
            splitter.Height = Double.NaN;
            splitter.Width = 5;
            splitter.HorizontalAlignment = HorizontalAlignment.Right;
            splitter.VerticalAlignment = VerticalAlignment.Stretch;
            splitter.Margin = new Thickness(0, 5, 0, 5);
            splitter.Cursor = Cursors.SizeWE;

            Grid.SetColumnSpan(splitter, 1);
            Grid.SetRowSpan(splitter, 2);
        }

    }
}