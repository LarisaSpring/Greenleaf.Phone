using System.Collections.Generic;

namespace Greenleaf.MVVM
{
    public interface INavigationManager
    {
        string Previous { get; }

        void NavigateTo(string route);

        void NavigateTo(string route, object prms, object context = null);

        void NavigateTo(string route, IDictionary<string, object> prms, object context = null);

        void GoBack();

        void GoBack(int steps);

        void ClearHistory();

        void Register<T>();

        void Register<T>(string route);

        bool RemoveHistoryEntry();
    }
}