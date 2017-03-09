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

        protected void SetProperty<T>(ref T field, T newValue, string propertyName)
            where T : IComparable
        {
            if ((field == null && newValue != null)
                || field.CompareTo(newValue) != 0)
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region private

        private UserControl _userControl;
        private string _header;

        #endregion

    }
}
