using System;

namespace Greenleaf.MVVM
{
    public class DelegateCommand<T> : IRaisableCommand where T : class
    {
        private readonly Action<T> _executeAction;
        private readonly Predicate<T> _canExecutePredicate;

        public DelegateCommand(Action<T> executeAction, Predicate<T> canExecutePredicate = null)
        {
            _executeAction = executeAction;
            _canExecutePredicate = canExecutePredicate;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecutePredicate == null)
            {
                return true;
            }
            else if (parameter == null || parameter is T)
            {
                return _canExecutePredicate((T)parameter);
            }
            return false;
        }

        public void Execute(object parameter)
        {
            if (_executeAction != null && (parameter == null || parameter is T))
            {
                _executeAction((T)parameter);
            }
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            EventHandler handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
