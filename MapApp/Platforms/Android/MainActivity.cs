using Android.App;
using Android.Content.PM;
using Android.OS;

namespace MapApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set the ScreenOrientation to Portrait only
            this.RequestedOrientation = ScreenOrientation.Portrait;
        }

        protected override void OnResume()
        {
            base.OnResume();
            UpdateNavigationBarState();
        }

        public override void OnBackPressed()
        {
            // microsoft doesn't support back key navigation for MAUI blazor yet
            //base.OnBackPressed();
        }

        private void UpdateNavigationBarState()
        {
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
            {
                // Api > 29
                if (Window?.DecorView.RootWindowInsets is not null)
                {
                    var bottom = Window.DecorView.RootWindowInsets.SystemGestureInsets.Bottom;
                }
            }
            else
            {

            }
        }

        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            UpdateNavigationBarState();
        }
    }
}
