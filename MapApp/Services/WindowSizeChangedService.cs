namespace MapApp.Services;

/// <summary>
/// Allows notify and react when Window size changed event is called
/// </summary>
public class WindowSizeChangedService : IWindowSizeChangedService
{
    public event NotifyWindowSizeChangedService? WindowSizeChangedServiceEvent;

    public void InvokeWindowSizeChanged(double width, double height)
    {
        WindowSizeChangedServiceEvent?.Invoke(width, height);
    }
}

public delegate void NotifyWindowSizeChangedService(double width, double height);
