using System;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

using TemsParser.Views;
using TemsParser.Messages;
using TemsParser.Common;
using TemsParser.CustomAttributes;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
//using Microsoft.TeamFoundation.MVVM;


namespace TemsParser.ViewModels
{
    //[Serializable]
    public abstract class ViewModelBase : NotifyPropertyChanged, IDataErrorInfo
    {
        #region ChildWindows

        private ChildWindowView _childWindowView;


        public bool DialogResult { get; set; }

        public string Title { get; set; }

        public ICommand OkCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        protected virtual void Closed() { }

        public bool Close()
        {
            var result = false;

            if (_childWindowView != null)
            {
                _childWindowView.Close();
                _childWindowView = null;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Show child window.
        /// </summary>
        /// <param name="viewModel">View model of child window.</param>
        /// <param name="isModal">Defines the modality of window.</param>
        public void Show(ViewModelBase viewModel, bool isModal = true)
        {
            viewModel._childWindowView = new ChildWindowView();
            viewModel._childWindowView.DataContext = viewModel;
            viewModel._childWindowView.Closed += (sender, e) => Closed();

            if (isModal)
            {
                viewModel._childWindowView.ShowInTaskbar = false;
                Window actWnd = null;
                var allWnd = Application.Current.Windows;

                foreach (Window item in allWnd)
                {
                    if (item.IsActive) actWnd = item;
                }

                if (actWnd == null && Application.Current.MainWindow.IsLoaded)
                {
                    actWnd = Application.Current.MainWindow;
                }

                viewModel._childWindowView.Owner = actWnd;
                viewModel._childWindowView.ShowDialog();
            }
            else
	        {
                viewModel._childWindowView.Show();
	        }
        }

        #endregion


        #region Validation

        public bool ValidatePropery(string propertyName)
        {
            var value = GetType().GetProperty(propertyName).GetValue(this);
            var context = new ValidationContext(this) { MemberName = propertyName };
            return Validator.TryValidateProperty(value, context, new List<ValidationResult>());
        }

        public bool ValidatePropery(string propertyName, out string errorMessage)
        {
            errorMessage = String.Empty;
            var value = GetType().GetProperty(propertyName).GetValue(this);
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this) { MemberName = propertyName };

            if (Validator.TryValidateProperty(value, context, results))
            {
                return true;
            }
            else
            {
                var errors = from item in results
                             select item.ErrorMessage;

                errorMessage = String.Join("\n", errors);
                return false;
            }
        }

        public bool ValidateObject(object validationObject)
        {
            return Validator.TryValidateObject(validationObject,
                                               new ValidationContext(validationObject),
                                               new List<ValidationResult>(),
                                               true);
        }

        public bool ValidateObject(object validationObject, ref ICollection<ValidationResult> result)
        {
            return Validator.TryValidateObject(validationObject,
                                               new ValidationContext(validationObject),
                                               result,
                                               true);
        }

        #endregion


        #region IDataErrorInfo

        [IgnoreGetAllChildren]
        public string this[string columnName]
        {
            get
            {
                string errorMessage = String.Empty;
                ValidatePropery(columnName, out errorMessage);
                return errorMessage;
            }
        }

        [IgnoreGetAllChildren]
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
