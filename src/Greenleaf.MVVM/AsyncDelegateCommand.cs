using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Greenleaf.MVVM
{
    public class AsyncDelegateCommand<T> : IRaisableCommand, IAsyncCommand where T : class
    {
        private readonly Func<T, Task> _executeAction;
        private readonly Predicate<T> _canExecutePredicate;

        public AsyncDelegateCommand(Func<T, Task> executeAction, Predicate<T> canExecutePredicate = null)
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

        async void ICommand.Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        public async Task ExecuteAsync(object parameter)
        {
            if (_executeAction != null && (parameter == null || parameter is T))
            {
                await _executeAction((T)parameter);
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
