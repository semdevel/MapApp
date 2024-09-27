using MapApp.Services;

namespace MapApp
{
    public partial class App : Application
    {
        private readonly IWindowSizeChangedService _windowSizeChangedService;
        private Window? _window;

        public App(IWindowSizeChangedService windowSizeChangedService)
        {
            InitializeComponent();
            _windowSizeChangedService = windowSizeChangedService;
            MainPage = new MainPage(_windowSizeChangedService);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            _window = base.CreateWindow(activationState);
#if WINDOWS
            _window.SizeChanged += (_, _) =>
             {
                 // read window width and height values
                 _windowSizeChangedService?.InvokeWindowSizeChanged(_window.Width, _window.Height);
             };
#endif
            return _window;
        }
    }
}
