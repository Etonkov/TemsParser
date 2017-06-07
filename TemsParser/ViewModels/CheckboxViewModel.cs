using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using TemsParser.Models.Config;
using TemsParser.Common;

namespace TemsParser.ViewModels
{
    public class CheckboxViewModel : NotifyPropertyChanged
    {
        public event EventHandler IsCheckedChanged;

        private bool _isChecked;

        private bool _isEnabled;

        public CheckboxViewModel(string name, bool isChecked)
        {
            Name = name;
            IsChecked = isChecked;
            IsEnabled = true;
        }


        public string Name { get; set; }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    var handler = IsCheckedChanged;

                    if (handler != null)
                    {
                        handler(this, EventArgs.Empty);
                    }
                }
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            private set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged();
                }
            }
        }


        public void IsEnabledUpdate(object sender, BooleanPropertyChangedEventArgs ea)
        {
            IsEnabled = ea.NewValue;
        }
    }
}
