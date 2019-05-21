using System;

using PriceFlyerTicker.UI.Core.Models;
using PriceFlyerTicker.UI.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace PriceFlyerTicker.UI.Views
{
    public sealed partial class ContentGridDetailPage : Page
    {
        public ContentGridDetailPage()
        {
            InitializeComponent();
        }

        private ContentGridDetailViewModel ViewModel => DataContext as ContentGridDetailViewModel;
    }
}
