using MapApp.Component.LeafletMap.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Collections.Concurrent;
using System.Drawing;
using Rectangle = MapApp.Component.LeafletMap.Models.Rectangle;

namespace MapApp.Component.LeafletMap
{
    public static class LeafletInterops
    {
        private static ConcurrentDictionary<string, (IDisposable, string, Layer)> LayerReferences { get; }
            = new ConcurrentDictionary<string, (IDisposable, string, Layer)>();

        private const string _baseObjectContainer = "window.leaflet";

        public static ValueTask Create(IJSRuntime jsRuntime, Map map) =>
            jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.create", map, DotNetObjectReference.Create(map));

        private static DotNetObjectReference<T> CreateLayerReference<T>(string mapId, T layer) where T : Layer
        {
            var result = DotNetObjectReference.Create(layer);
            LayerReferences.TryAdd(layer.Id, (result, mapId, layer));
            return result;
        }

        private static void DisposeLayerReference(string layerId)
        {
            if (LayerReferences.TryRemove(layerId, out var value))
                value.Item1.Dispose();
        }

        public static ValueTask AddLayer(IJSRuntime jsRuntime, string mapId, Layer? layer)
        {
            return layer switch
            {
                TileLayer tileLayer => jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.addTilelayer", mapId, tileLayer, CreateLayerReference(mapId, tileLayer)),
                MbTilesLayer mbTilesLayer => jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.addMbTilesLayer", mapId, mbTilesLayer, CreateLayerReference(mapId, mbTilesLayer)),
                ShapefileLayer shapefileLayer => jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.addShapefileLayer", mapId, shapefileLayer, CreateLayerReference(mapId, shapefileLayer)),
                Marker marker => jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.addMarker", mapId, marker, CreateLayerReference(mapId, marker)),
                Rectangle rectangle => jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.addRectangle", mapId, rectangle, CreateLayerReference(mapId, rectangle)),
                Circle circle => jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.addCircle", mapId, circle, CreateLayerReference(mapId, circle)),
                Polygon polygon => jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.addPolygon", mapId, polygon, CreateLayerReference(mapId, polygon)),
                Polyline polyline => jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.addPolyline", mapId, polyline, CreateLayerReference(mapId, polyline)),
                ImageLayer image => jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.addImageLayer", mapId, image, CreateLayerReference(mapId, image)),
                GeoJsonDataLayer geo => jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.addGeoJsonLayer", mapId, geo, CreateLayerReference(mapId, geo)),

                _ => throw new NotImplementedException($"The layer {typeof(Layer).Name} has not been implemented."),
            };
        }

        public static async ValueTask RemoveLayer(IJSRuntime jsRuntime, string mapId, string layerId)
        {
            await jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.removeLayer", mapId, layerId);
            DisposeLayerReference(layerId);
        }

        public static ValueTask UpdatePopupContent(IJSRuntime jsRuntime, string mapId, Layer layer) =>
            jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.updatePopupContent", mapId, layer.Id, layer.Popup?.Content);

        public static ValueTask UpdateTooltipContent(IJSRuntime jsRuntime, string mapId, Layer layer) =>
            jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.updateTooltipContent", mapId, layer.Id, layer.Tooltip?.Content);

        public static ValueTask UpdateShape(IJSRuntime jsRuntime, string mapId, Layer layer) =>
            layer switch
            {
                Rectangle rectangle => jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.updateRectangle", mapId, rectangle),
                Circle circle => jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.updateCircle", mapId, circle),
                Polygon polygon => jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.updatePolygon", mapId, polygon),
                Polyline polyline => jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.updatePolyline", mapId, polyline),
                _ => throw new NotImplementedException($"The layer {typeof(Layer).Name} has not been implemented."),
            };

        public static ValueTask FitBounds(IJSRuntime jsRuntime, string mapId, PointF corner1, PointF corner2, PointF? padding, float? maxZoom) =>
            jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.fitBounds", mapId, corner1, corner2, padding, maxZoom);

        public static ValueTask PanTo(IJSRuntime jsRuntime, string mapId, PointF position, bool animate, float duration, float easeLinearity, bool noMoveStart) =>
            jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.panTo", mapId, position, animate, duration, easeLinearity, noMoveStart);

        public static ValueTask<LatLng> GetCenter(IJSRuntime jsRuntime, string mapId) =>
            jsRuntime.InvokeAsync<LatLng>($"{_baseObjectContainer}.getCenter", mapId);

        public static ValueTask<float> GetZoom(IJSRuntime jsRuntime, string mapId) =>
            jsRuntime.InvokeAsync<float>($"{_baseObjectContainer}.getZoom", mapId);

        public static ValueTask ZoomIn(IJSRuntime jsRuntime, string mapId, MouseEventArgs e) =>
            jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.zoomIn", mapId, e);

        public static ValueTask ZoomOut(IJSRuntime jsRuntime, string mapId, MouseEventArgs e) =>
            jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.zoomOut", mapId, e);

        public static ValueTask SetView(IJSRuntime jsRuntime, string id, LatLng center, float zoom) =>
            jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.setView", id, center, zoom);

        public static ValueTask ClearMap(IJSRuntime jsRuntime) =>
            jsRuntime.InvokeVoidAsync($"{_baseObjectContainer}.clearMap");
    }
}
