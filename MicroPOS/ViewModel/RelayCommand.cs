using System;
using System.Windows.Input;

namespace MicroPOS.ViewModel
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T> _canExecute;

        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute) : this(execute, null)
        {
            this._execute = execute;
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (this._canExecute == null)
            {
                return true;
            }
            return this._canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            this._execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }
}