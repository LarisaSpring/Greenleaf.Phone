using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Greenleaf.MVVM
{
    public abstract class NavigationManagerBase : INavigationManager
    {
        private readonly Dictionary<string, Type> _map = new Dictionary<string, Type>();

        public string Previous { get; protected set; }

        public void NavigateTo(string route)
        {
            NavigateTo(route, null);
        }

        public void NavigateTo(string route, object prms, object context = null)
        {
            var dictPrms = prms?.ToDictionary();

            NavigateTo(route, dictPrms, context);
        }

        public void NavigateTo(string route, IDictionary<string, object> prms, object context = null)
        {
            Type type;

            if (!_map.TryGetValue(route, out type))
            {
                throw new InvalidOperationException("Can't find such a route: " + route);
            }

            NavigateImpl(type, prms, context);
            SetPrevious();
        }

        public void GoBack()
        {
            GoBack(1);
        }

        public void GoBack(int steps)
        {
            GoBackImpl(steps);
            SetPrevious();
        }

        public bool RemoveHistoryEntry()
        {
            return RemoveHistoryEntryImpl();
        }

        public void ClearHistory()
        {
            ClearHistoryImpl();
        }

        public void Register<T>()
        {
            var pageName = typeof(T).Name;
            pageName = Regex.Replace(pageName, "(page|view)$", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                .Trim()
                .ToLower();
            Register<T>(pageName);
        }

        public void Register<T>(string route)
        {
            Type type;

            if (_map.TryGetValue(route, out type))
            {
                throw new InvalidOperationException("The route was already registered.");
            }

            _map.Add(route, typeof(T));
        }

        private void SetPrevious()
        {
            var current = GetCurrentPage();

            if (_map.ContainsValue(current.GetType()))
            {
                Previous = _map.First(x => x.Value == current.GetType()).Key;
            }
        }

        protected abstract void NavigateImpl(Type pageType, IDictionary<string, object> prms, object context);

        protected abstract void GoBackImpl(int steps);

        protected abstract void ClearHistoryImpl();

        protected abstract object GetCurrentPage();

        protected abstract bool RemoveHistoryEntryImpl();
    }
}