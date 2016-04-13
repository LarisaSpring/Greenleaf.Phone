using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Greenleaf.MVVM;

namespace Greenleaf.Phone
{
    public class NavigationManager : NavigationManagerBase
    {
        private readonly Func<Frame> _frameNavigation;
        private object _currentNavigationContext;

        public NavigationManager(Func<Frame> frameNavigation)
        {
            _frameNavigation = frameNavigation;
        }

        protected override void NavigateImpl(Type pageType, IDictionary<string, object> prms, object context)
        {
            var page = _frameNavigation();

            if (page == null)
            {
#if DEBUG
                throw new NullReferenceException("No RootFrame was found");
#else
                return;
#endif
            }

            if (context != null)
            {
                _currentNavigationContext = context;
                page.Navigated += OnPageNavigated;
            }

            page.Navigate(pageType, prms);
        }

        protected override void GoBackImpl(int steps)
        {

            if (steps < 1)
            {
                return;
            }

            var page = _frameNavigation();

            if (page == null)
            {
#if DEBUG
                throw new NullReferenceException("No RootFrame was found");
#else
                            return;
#endif
            }

            //while (steps > 1 && page.CanGoBack)
            //{
            //    page.RemoveBackEntry();
            //    steps--;
            //}

            if (page.CanGoBack)
            {
                page.GoBack();
            }
        }

        protected override void ClearHistoryImpl()
        {
            var page = _frameNavigation();

            page.BackStack.Clear();
        }

        protected override object GetCurrentPage()
        {
            var page = _frameNavigation();

            return page.Content;
        }

        protected override bool RemoveHistoryEntryImpl()
        {
            var page = _frameNavigation();

            if (!page.CanGoBack)
            {
                return false;
            }

            page.BackStack.RemoveAt(page.BackStack.Count - 1);

            return true;
        }

        private void OnPageNavigated(object sender, NavigationEventArgs navigationEventArgs)
        {
            if (_currentNavigationContext == null)
            {
                throw new InvalidOperationException("Empty context happen to be while navigating.");
            }

            var element = (FrameworkElement)navigationEventArgs.Content;
            element.DataContext = _currentNavigationContext;

            _currentNavigationContext = null;

            ((Frame)sender).Navigated -= OnPageNavigated;
        }
    }
}