using Greenleaf.MVVM;

namespace Greenleaf.Samples.WP81
{
    public static class NavigationConfig
    {
        public static void Initialize(INavigationManager navigationManager)
        {
            navigationManager.Register<SecondPage>("second");
        }
    }
}