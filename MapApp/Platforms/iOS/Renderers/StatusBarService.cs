using Foundation;
using Microsoft.Maui;
using UIKit;
using Application = Microsoft.Maui.Controls.Application;

[assembly: Dependency(typeof(MapApp.Platforms.iOS.StatusBarService))]
namespace MapApp.Platforms.iOS
{
    public static class StatusBarService
    {
        public static double? GetStatusBarHeight()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(12, 0))
            {
                var window = UIApplication.SharedApplication.KeyWindow;
                var safeAreaTopInset = window?.SafeAreaInsets.Top ?? 0;
                return safeAreaTopInset;
            }
            return UIApplication.SharedApplication.StatusBarFrame.Height;
        }
    }
}
