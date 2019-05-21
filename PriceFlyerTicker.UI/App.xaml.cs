using System;
using System.Globalization;
using System.Threading.Tasks;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Practices.Unity;

using PriceFlyerTicker.UI.Core.Services;
using PriceFlyerTicker.UI.Services;

using Prism.Mvvm;
using Prism.Unity.Windows;
using Prism.Windows.AppModel;

using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PriceFlyerTicker.UI
{
    [Windows.UI.Xaml.Data.Bindable]
    public sealed partial class App : PrismUnityApplication
    {
        public App()
        {
            InitializeComponent();

            // TODO WTS: Add your app in the app center and set your secret here. More at https://docs.microsoft.com/en-us/appcenter/sdk/getting-started/uwp
            //AppCenter.Start("{Your App Secret}", typeof(Analytics), typeof(Crashes));
        }

        protected override void ConfigureContainer()
        {
            // register a singleton using Container.RegisterType<IInterface, Type>(new ContainerControlledLifetimeManager());
            base.ConfigureContainer();
            Container.RegisterType<ILiveTileService, LiveTileService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IToastNotificationsService, ToastNotificationsService>(new ContainerControlledLifetimeManager());
            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));
            Container.RegisterType<ISampleDataService, SampleDataService>();
        }



        protected override async Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500, 80));
            ApplicationView.GetForCurrentView().TryResizeView(new Size(500,80));


            await LaunchApplicationAsync(PageTokens.ContentGridPage, null);
        }

        private async Task LaunchApplicationAsync(string page, object launchParam)
        {
            await ThemeSelectorService.SetRequestedThemeAsync();
            var rootFrame = Window.Current.Content as Frame;
            Container.RegisterInstance<IConnectedAnimationService>(new ConnectedAnimationService(rootFrame));
            NavigationService.Navigate(page, launchParam);
            Window.Current.Activate();
            Container.Resolve<ILiveTileService>().SampleUpdate();
            Container.Resolve<IToastNotificationsService>().ShowToastNotificationSample();
        }

        protected override async Task OnActivateApplicationAsync(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.ToastNotification && args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // Handle a toast notification here
                // Since dev center, toast, and Azure notification hub will all active with an ActivationKind.ToastNotification
                // you may have to parse the toast data to determine where it came from and what action you want to take
                // If the app isn't running then launch the app here
                await OnLaunchApplicationAsync(args as LaunchActivatedEventArgs);
            }

            await Task.CompletedTask;
        }

        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await base.OnInitializeAsync(args);
            await ThemeSelectorService.InitializeAsync().ConfigureAwait(false);

            // We are remapping the default ViewNamePage and ViewNamePageViewModel naming to ViewNamePage and ViewNameViewModel to
            // gain better code reuse with other frameworks and pages within Windows Template Studio
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "PriceFlyerTicker.UI.ViewModels.{0}ViewModel, PriceFlyerTicker.UI", viewType.Name.Substring(0, viewType.Name.Length - 4));
                return Type.GetType(viewModelTypeName);
            });
            await Container.Resolve<ILiveTileService>().EnableQueueAsync().ConfigureAwait(false);
        }

        protected override IDeviceGestureService OnCreateDeviceGestureService()
        {
            var service = base.OnCreateDeviceGestureService();
            service.UseTitleBarBackButton = false;
            return service;
        }
    }
}
