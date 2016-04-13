using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Greenleaf.MVVM;
using Greenleaf.Phone;
using Microsoft.Practices.Unity;

namespace Greenleaf.Samples.WP81
{
    public class IoCContainer : UnityContainer
    {
        public IoCContainer()
        {
            this.RegisterInstance(GetNavigationManager());
        }

        private INavigationManager GetNavigationManager()
        {
            var manager = new NavigationManager(() => (Frame) Window.Current.Content);
            NavigationConfig.Initialize(manager);
            return manager;
        }
    }
}