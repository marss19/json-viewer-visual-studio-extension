using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Marss.JsonViewer.ViewModels.Utils
{
    public static class WebBrowserUtility
    {
        public static readonly DependencyProperty BindableSourceProperty =
            DependencyProperty.RegisterAttached("BindableSource", typeof(string), typeof(WebBrowserUtility), new UIPropertyMetadata(null, BindableSourcePropertyChanged));

        public static string GetBindableSource(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject obj, string value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        public static void BindableSourcePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var browser = obj as WebBrowser;
            if (browser != null)
            {
                string resourcePath = e.NewValue as string;
                if (!string.IsNullOrEmpty(resourcePath))
                {
                    browser.NavigateToStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath));
                }
            }
        }

    }
}
