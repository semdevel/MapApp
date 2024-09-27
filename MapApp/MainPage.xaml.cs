using MapApp.Services;

namespace MapApp
{
    public partial class MainPage : ContentPage
    {
        private double _availableHeight = 0;
#if WINDOWS
        private readonly IWindowSizeChangedService? _windowSizeChangedService;
#endif

        /// <summary>
        /// Represents current available height of the page.
        /// </summary>
        public double AvailableHeight
        {
            get => _availableHeight;
            set
            {
                _availableHeight = value;
                SetHeightOfBlazorWebView(_availableHeight);
            }
        }

        public MainPage(IWindowSizeChangedService windowSizeChangedService)
        {
            InitializeComponent();
#if WINDOWS
            _windowSizeChangedService = windowSizeChangedService;
#endif
            SetPageHeight();
        }

        /// <summary>
        /// Set page height of a main page.
        /// </summary>
        private void SetPageHeight()
        {
            AvailableHeight = GetCurrentDisplayHeight();
        }

        /// <summary>
        /// Set display height according to the specific device.
        /// </summary>
        private double GetCurrentDisplayHeight()
        {
#if WINDOWS
            const double navigationBarHeight = 0;
            const double statusBarHeight = 0;
#elif ANDROID
            double navigationBarHeight = 0;
            double statusBarHeight = 0;
#elif IOS
            const double navigationBarHeight = 0;
            double statusBarHeight = 0;
#endif
            // Assuming blazorWebView is your BlazorWebView's x:Name
            var screenHeight = DeviceDisplay.MainDisplayInfo.Height;
            // Adjust the height based on other elements' heights and desired margins
            var adjustedHeight = screenHeight / DeviceDisplay.MainDisplayInfo.Density;

#if ANDROID
            if (!MapApp.Platforms.Android.NavigationBarService.IsGestureNavigationBarEnabled)
            {
                navigationBarHeight = MapApp.Platforms.Android.NavigationBarService.GetNavigationBarHeight() / DeviceDisplay.MainDisplayInfo.Density;
            }
            statusBarHeight = MapApp.Platforms.Android.StatusBarService.GetStatusBarHeight() / DeviceDisplay.MainDisplayInfo.Density;
#elif IOS
            statusBarHeight = MapApp.Platforms.iOS.StatusBarService.GetStatusBarHeight() ?? 0;

#elif WINDOWS
            var borderHeight = MapApp.Platforms.Windows.TitleBarService.GetBorderHeight();
            var titleBarHeight = MapApp.Platforms.Windows.TitleBarService.GetTitleBarHeight();
            if (_windowSizeChangedService is not null)
	        {
#pragma warning disable RCS1163
                _windowSizeChangedService.WindowSizeChangedServiceEvent += (windowWidth, windowHeight) =>
                {
                    var isWindowMaximized = MapApp.Platforms.Windows.TitleBarService.IsWindowMaximized();
                    if (isWindowMaximized)
	                {
                        AvailableHeight = windowHeight - (2 * titleBarHeight);
	                }
                    else
                    {
                        AvailableHeight = windowHeight - (2 * titleBarHeight) + (2 * borderHeight);
                    }
                };
#pragma warning restore RCS1163
	        }
#endif
            if (Double.IsNaN(adjustedHeight))
            {
                return 0;
            }
            else
            {
                // Android and iOS platform
                return adjustedHeight - statusBarHeight - navigationBarHeight;
            }
        }

        private void SetHeightOfBlazorWebView(double height)
        {
            blazorWebView.HeightRequest = height;
        }
    }
}
