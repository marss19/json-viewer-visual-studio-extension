using Marss.JsonViewer.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Marss.JsonViewer.ViewModels
{

    public abstract class TabViewModelBase : INotifyPropertyChanged
    {
   
        public string Header
        {
            get { return _header; }
            set { SetProperty(ref _header, value, "Header"); }
        }


        public string HeaderIcon
        {
            get; protected set;
        }

        public bool ShowHeaderIcon
        {
            get; protected set;
        }

        public string HeaderIconTooltip
        {
            get; protected set;
        }


        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value, "Message"); }
        }

        public abstract string UserControlName { get; }
        public abstract bool CanBeRemoved { get; }
        public abstract bool Editable { get; }



        public UserControl UserControl
        {
            get
            {
                if (_userControl == null)
                {
                    var type = typeof(DefaultViewerControl);
                    _userControl = (UserControl)type.Assembly.CreateInstance(string.Format("{0}.{1}", type.Namespace, UserControlName));
                    _userControl.DataContext = this;
                }
                return _userControl;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T value, T newValue, string propertyName)
            where T : IComparable
        {
            if ((value == null && newValue != null)
                || value.CompareTo(newValue) != 0)
            {
                value = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void SetProperty<T>(ref IEnumerable<T> value, IEnumerable<T> newValue, string propertyName)
             where T : IComparable
        {
            if (!ArraysAreEqual(value, newValue))
            {
                value = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool ArraysAreEqual<T>(IEnumerable<T> arr1, IEnumerable<T> arr2)
            where T: IComparable
        {
            if (arr1 == null && arr2 == null)
                return true;

            if (arr1 == null || arr2 == null)
                return false;

            return arr1.SequenceEqual(arr2);
        }

        #region private

        private UserControl _userControl;
        private string _header;
        private string _message;

        #endregion

    }
}
