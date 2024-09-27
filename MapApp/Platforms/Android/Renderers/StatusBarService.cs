using AppAndroid = Android.App.Application;

[assembly: Dependency(typeof(MapApp.Platforms.Android.StatusBarService))]
namespace MapApp.Platforms.Android;
public static class StatusBarService
{
    public static int GetStatusBarHeight()
    {
        if (AppAndroid.Context.Resources is not null)
        {
            int resourceId = AppAndroid.Context.Resources!.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
            {
                return AppAndroid.Context.Resources.GetDimensionPixelSize(resourceId);
            }
        }
        return 0;
    }
}