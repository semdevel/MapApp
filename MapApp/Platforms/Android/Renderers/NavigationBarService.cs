using AppAndroid = Android.App.Application;

[assembly: Dependency(typeof(MapApp.Platforms.Android.NavigationBarService))]
namespace MapApp.Platforms.Android;

public static class NavigationBarService
{
    public static bool IsGestureNavigationBarEnabled { get; set; }
    public static event NotifyNavigationBar? NavigationBarChangedEvent;
    public static int GetNavigationBarHeight()
    {
        if (AppAndroid.Context.Resources is not null)
        {
            int resourceId = AppAndroid.Context.Resources!.GetIdentifier("navigation_bar_height", "dimen", "android");
            if (resourceId > 0)
            {
                return AppAndroid.Context.Resources.GetDimensionPixelSize(resourceId);
            }
        }
        return 0;
    }

    //public static void IsGestureNavigationEnabled()
    //{
    //    // This setting's value might vary across different devices/manufacturers
    //    string navigationMode = Settings.Secure.GetString(AppAndroid.Context.ContentResolver, "navigation_mode");

    //    // Typically, "0" = 3-button navigation, "1" = 2-button navigation, "2" = gesture navigation
    //    // These values can vary, so check your device's settings or documentation
    //    IsGestureNavigationBarEnabled = navigationMode == "2" ? true : false;
    //    InvokeNavigationBarChanged();
    //}

    public static void IsGestureNavigationEnabled(int value)
    {
        IsGestureNavigationBarEnabled = value == 0;
        InvokeNavigationBarChanged();
    }

    public static void InvokeNavigationBarChanged()
    {
        NavigationBarChangedEvent?.Invoke();
    }
}

public delegate void NotifyNavigationBar();