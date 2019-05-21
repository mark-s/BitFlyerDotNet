using System;

using PriceFlyerTicker.UI.ViewModels;

using Windows.UI.Xaml.Controls;

namespace PriceFlyerTicker.UI.Views
{
    public sealed partial class ContentGridPage : Page
    {
        private ContentGridViewModel ViewModel => DataContext as ContentGridViewModel;

        public ContentGridPage()
        {
            InitializeComponent();
        }
    }
}
