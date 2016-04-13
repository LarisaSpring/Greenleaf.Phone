using System;
using System.Windows.Input;
using Greenleaf.MVVM;

namespace Greenleaf.Samples.WP81
{
    public class MainViewModel : BindableBase
    {
        private readonly INavigationManager _navigationManager;
        private string _text;
        private ICommand _nextPageCommand;

        public MainViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            _text = "Main page";
        }

        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        public ICommand NextPageCommand => 
            _nextPageCommand ?? (_nextPageCommand = new DelegateCommand<object>(OnNextPageTapped));

        private void OnNextPageTapped(object o)
        {
            _navigationManager.NavigateTo("second");
        }
    }
}