using System.Runtime.InteropServices;
using WinRT.Interop;

[assembly: Dependency(typeof(MapApp.Platforms.Windows.TitleBarService))]
namespace MapApp.Platforms.Windows
{
    public static class TitleBarService
    {
#pragma warning disable SYSLIB1054
        [DllImport(dllName: "user32.dll", SetLastError = true)]
        static extern int GetSystemMetrics(int nIndex);
        [DllImport(dllName: "user32.dll")]
        static extern bool IsZoomed(IntPtr hWnd);
#pragma warning restore SYSLIB1054
#pragma warning disable IDE1006 // Naming Styles
        const int SM_CYCAPTION = 4;
        const int SM_CYFRAME = 33;
#pragma warning restore IDE1006 // Naming Styles

        public static double GetTitleBarHeight()
        {
            return GetSystemMetrics(SM_CYCAPTION);
        }

        public static double GetBorderHeight()
        {
            int borderHeight = GetSystemMetrics(SM_CYFRAME);
            return borderHeight;
        }

        public static bool IsWindowMaximized()
        {
            var hWnd = WindowNative.GetWindowHandle(Microsoft.Maui.Controls.Application.Current!.Windows[0].Handler.PlatformView as Microsoft.UI.Xaml.Window);
            return IsZoomed(hWnd);
        }
    }
}