using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marss.JsonViewer.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T value, T newValue, string propertyName)
            where T : IComparable
        {
            if (value == null && newValue == null)
                return;

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
            where T : IComparable
        {
            if (arr1 == null && arr2 == null)
                return true;

            if (arr1 == null || arr2 == null)
                return false;

            return arr1.SequenceEqual(arr2);
        }
    }
}
