using MapApp.Component.LeafletMap.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace MapApp.Component.UI.Pages;

public partial class Home
{
    private LeafletMap.Map? _map;
    private readonly LatLng _startAt = new(lat: 49.8037633F, lng: 15.4749126F);

    private const int DefaultAreaOfInterest = 15;
    private const int InitializeZoom = 10;
    private const int MinimumZoom = 7;
    private const int DefaultZoom = 16;
    private const int MaximumZoom = 18;
    private const string MapUrlTemplate = "https://a.tile.openstreetmap.org/{z}/{x}/{y}.png";
    private const string MandatoryWatermark = @"&copy; <a href=""https://www.openstreetmap.org/copyright"">OpenStreetMap</a>";

    [Inject] private IJSRuntime? IJSRuntime { get; set; }

    [Inject] private ILogger<Home>? Logger { get; set; }

    protected override void OnInitialized()
    {
        _map = new LeafletMap.Map(id: "MapId", IJSRuntime!)
        {
            Center = _startAt,
            Zoom = MinimumZoom,
            ZoomControl = false
        };
        _map.OnInitialized += () =>
        {
            _map.AddLayer(layer: new TileLayer
            {
                UrlTemplate = MapUrlTemplate,
                Attribution = MandatoryWatermark,
                MinimumZoom = MinimumZoom,
                MaximumZoom = MaximumZoom
            });
        };
        _map.OnError += (Exception ex) =>
        {   // Error during map initialization
            Logger?.LogError(ex, message: "An Error thrown during a map initialization process.");
        };
    }
}