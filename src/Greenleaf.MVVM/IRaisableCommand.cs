
using System.Windows.Input;

namespace Greenleaf.MVVM
{
    public interface IRaisableCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
