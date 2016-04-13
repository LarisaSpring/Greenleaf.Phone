using Greenleaf.MVVM;

namespace Greenleaf.Samples.WP81
{
    public class ViewModelLocator : LocatorBase
    {
        public MainViewModel MainViewModel => this.Resolve<MainViewModel>(singleton: true);
    }
}