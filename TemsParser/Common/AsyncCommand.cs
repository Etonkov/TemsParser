using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TemsParser.Common
{
    public class AsyncCommand : IAsyncCommand
    {
        private bool _canExecute = true;

        #region Constructors

        public AsyncCommand(Func<object, Task> execute)
        {
            ExecuteDelegate = execute;
        }

        public AsyncCommand(Func<object, Task> execute, Predicate<object> canExecute)
        {
            ExecuteDelegate = execute;
            CanExecuteDelegate = canExecute;
        }

        #endregion

        #region Properties

        public Predicate<object> CanExecuteDelegate { get; private set; }
        public Func<object, Task> ExecuteDelegate { get; private set; }

        #endregion

        public Task Execution { get; private set; }

        public bool IsCanExecute
        {
            get
            { 
                return _canExecute;
            }
            set
            {
                if (_canExecute != value)
                {
                    _canExecute = value;
                    RaiseCanExecuteChanged();
                }
            }
        }


        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public bool CanExecute(object parameter)
        {
            return ((CanExecuteDelegate == null) || CanExecuteDelegate(parameter)) && IsCanExecute;
        }

        public async void Execute(object parameter)
        {
            IsCanExecute = false;
            await ExecuteAsync(parameter);
            IsCanExecute = true;
        }

        public async Task ExecuteAsync(object parameter)
        {
            await ExecuteDelegate(parameter);
        }
    }
}
