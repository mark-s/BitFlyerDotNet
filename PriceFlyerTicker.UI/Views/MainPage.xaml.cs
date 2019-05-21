using System;

using PriceFlyerTicker.UI.ViewModels;

using Windows.UI.Xaml.Controls;

namespace PriceFlyerTicker.UI.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
