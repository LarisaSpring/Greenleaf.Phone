
using System.Threading.Tasks;
using System.Windows.Input;

namespace Greenleaf.MVVM
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object param);
    }
}
