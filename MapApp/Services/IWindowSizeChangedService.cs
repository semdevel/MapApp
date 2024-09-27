namespace MapApp.Services;

/// <summary>
/// Allows notify and react when Window size changed event is called
/// </summary>
public interface IWindowSizeChangedService
{
    event NotifyWindowSizeChangedService? WindowSizeChangedServiceEvent;

    void InvokeWindowSizeChanged(double width, double height);
}
