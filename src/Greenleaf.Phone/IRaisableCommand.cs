
using System.Windows.Input;

namespace Greenleaf.Phone
{
    public interface IRaisableCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
