using MapApp.Component.LeafletMap.Exceptions;
using MapApp.Component.LeafletMap.Models;
using MapApp.Component.LeafletMap.Models.Events;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;

namespace MapApp.Component.LeafletMap
{
    /// <summary>
    /// Represents a map in the Leaflet library.
    /// https://leafletjs.com/reference.html#map-example
    /// </summary>
    public class Map : IDisposable
    {
        private readonly ObservableCollection<Layer> _layers = [];
        private readonly IJSRuntime _jsRuntime;
        private bool _isInitialized;
        private bool _disposedValue;
        /// <summary>
        /// Initial geographic center of the map
        /// </summary>
        public LatLng Center { get; set; } = new LatLng();
        /// <summary>
        /// Initial map zoom level
        /// </summary>
        public float Zoom { get; set; }
        /// <summary>
        /// Minimum zoom level of the map. If not specified and at least one
        /// GridLayer or TileLayer is in the map, the lowest of their minZoom
        /// options will be used instead.
        /// </summary>
        public float? MinZoom { get; set; }
        /// <summary>
        /// Maximum zoom level of the map. If not specified and at least one
        /// GridLayer or TileLayer is in the map, the highest of their maxZoom
        /// options will be used instead.
        /// </summary>
        public float? MaxZoom { get; set; }
        /// <summary>
        /// When this option is set, the map restricts the view to the given
        /// geographical bounds, bouncing the user back if the user tries to pan
        /// outside the view.
        /// </summary>
        public LatLngPair? MaxBounds { get; set; }
        /// <summary>
        /// Whether a zoom control is added to the map by default.
        /// <para/>
        /// Defaults to true.
        /// </summary>
        public bool ZoomControl { get; set; } = true;
        /// <summary>
        /// Event raised when the component has finished its first render.
        /// </summary>
        public event Action? OnInitialized;
        /// <summary>
        /// Event raised when the component has no internet connection.
        /// </summary>
        public event Action<Exception>? OnError;
        /// <summary>
        /// Unique identifier of the map.
        /// </summary>
        public string Id { get; }

        public Map(string id, IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
            //Id = StringHelper.GetRandomString(10);
            Id = id;
            _layers.CollectionChanged += OnLayersChanged;
        }

        /// <summary>
        /// This method MUST be called only once by the Blazor component upon rendering, and never by the user.
        /// </summary>
        public void RaiseOnInitialized()
        {
            _isInitialized = true;
            OnInitialized?.Invoke();
        }

        /// <summary>
        /// Raised when there is no internet connection.
        /// </summary>
        public void RaiseOnError(Exception exception)
        {
           OnError?.Invoke(exception);
        }

        /// <summary>
        /// Add a layer to the map.
        /// </summary>
        /// <param name="layer">The layer to be added.</param>
        /// <exception cref="System.ArgumentNullException">Throws when the layer is null.</exception>
        /// <exception cref="UninitializedMapException">Throws when the map has not been yet initialized.</exception>
        public void AddLayer(Layer layer)
        {
            ArgumentNullException.ThrowIfNull(layer);

            if (!_isInitialized)
            {
                throw new UninitializedMapException();
            }

            _layers.Add(layer);
        }

        /// <summary>
        /// Remove a layer from the map.
        /// </summary>
        /// <param name="layer">The layer to be removed.</param>
        /// <exception cref="System.ArgumentNullException">Throws when the layer is null.</exception>
        /// <exception cref="UninitializedMapException">Throws when the map has not been yet initialized.</exception>
        public void RemoveLayer(Layer layer)
        {
            ArgumentNullException.ThrowIfNull(layer);

            if (!_isInitialized)
            {
                throw new UninitializedMapException();
            }

            _layers.Remove(layer);
        }

        /// <summary>
        /// Get a read only collection of the current layers.
        /// </summary>
        /// <returns>A read only collection of layers.</returns>
        public IReadOnlyCollection<Layer> GetLayers()
        {
            return _layers.ToList().AsReadOnly();
        }

        private async void OnLayersChanged(object? sender, NotifyCollectionChangedEventArgs args)
        {
            if (args is not null)
            {
                if (args.Action == NotifyCollectionChangedAction.Add)
                {
                    if (args.NewItems is not null)
                    {
                        foreach (var item in args.NewItems)
                        {
                            var layer = item as Layer;
                            if (layer is not null)
                            {
                                await LeafletInterops.AddLayer(_jsRuntime, Id, layer);
                            }
                        }
                    }
                }
                else if (args.Action == NotifyCollectionChangedAction.Remove)
                {
                    if (args.OldItems is not null)
                    {
                        foreach (var item in args.OldItems)
                        {
                            if (item is Layer layer)
                            {
                                await LeafletInterops.RemoveLayer(_jsRuntime, Id, layer.Id);
                            }
                        }
                    }
                }
                else if (args.Action == NotifyCollectionChangedAction.Replace
                         || args.Action == NotifyCollectionChangedAction.Move)
                {
                    if (args.OldItems is not null)
                    {
                        foreach (var oldItem in args.OldItems)
                        {
                            if (oldItem is Layer layer)
                                await LeafletInterops.RemoveLayer(_jsRuntime, Id, layer.Id);
                        }
                    }

                    if (args.NewItems is not null)
                    {
                        foreach (var newItem in args.NewItems)
                        {
                            await LeafletInterops.AddLayer(_jsRuntime, Id, newItem as Layer);
                        }
                    }
                }
            }
        }

        public async void FitBounds(PointF corner1, PointF corner2, PointF? padding = null, float? maxZoom = null)
        {
            await LeafletInterops.FitBounds(_jsRuntime, Id, corner1, corner2, padding, maxZoom);
        }

        public async void PanTo(PointF position, bool animate = false, float duration = 0.25f, float easeLinearity = 0.25f, bool noMoveStart = false)
        {
            await LeafletInterops.PanTo(_jsRuntime, Id, position, animate, duration, easeLinearity, noMoveStart);
        }

        public async Task<LatLng> GetCenter() => await LeafletInterops.GetCenter(_jsRuntime, Id);
        public async Task<float> GetZoom() => await LeafletInterops.GetZoom(_jsRuntime, Id);

        /// <summary>
        /// Increases the zoom level by one notch.
        /// If <c>shift</c> is held down, increases it by three.
        /// </summary>
        public async Task ZoomIn(MouseEventArgs e) => await LeafletInterops.ZoomIn(_jsRuntime, Id, e);

        /// <summary>
        /// Decreases the zoom level by one notch.
        /// If <c>shift</c> is held down, decreases it by three.
        /// </summary>
        public async Task ZoomOut(MouseEventArgs e) => await LeafletInterops.ZoomOut(_jsRuntime, Id, e);

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(disposing: true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                    _layers.CollectionChanged -= OnLayersChanged;
                }

                // Free unmanaged resources (unmanaged objects) and override finalizer
                // Set large fields to null
                _disposedValue = true;
            }
        }
        #region events

        public delegate void MapEventHandler(object sender, Event e);
        public delegate void MapResizeEventHandler(object sender, ResizeEvent e);
        public event MapEventHandler? OnZoomLevelsChange;
        [JSInvokable]
        public void NotifyZoomLevelsChange(Event e) => OnZoomLevelsChange?.Invoke(this, e);

        public event MapResizeEventHandler? OnResize;
        [JSInvokable]
        public void NotifyResize(ResizeEvent e) => OnResize?.Invoke(this, e);

        public event MapEventHandler? OnUnload;
        [JSInvokable]
        public void NotifyUnload(Event e) => OnUnload?.Invoke(this, e);

        public event MapEventHandler? OnViewReset;
        [JSInvokable]
        public void NotifyViewReset(Event e) => OnViewReset?.Invoke(this, e);

        public event MapEventHandler? OnLoad;
        [JSInvokable]
        public void NotifyLoad(Event e) => OnLoad?.Invoke(this, e);

        public event MapEventHandler? OnZoomStart;
        [JSInvokable]
        public void NotifyZoomStart(Event e) => OnZoomStart?.Invoke(this, e);

        public event MapEventHandler? OnMoveStart;
        [JSInvokable]
        public void NotifyMoveStart(Event e) => OnMoveStart?.Invoke(this, e);

        public event MapEventHandler? OnZoom;
        [JSInvokable]
        public void NotifyZoom(Event e) => OnZoom?.Invoke(this, e);

        public event MapEventHandler? OnMove;
        [JSInvokable]
        public void NotifyMove(Event e) => OnMove?.Invoke(this, e);

        public event MapEventHandler? OnZoomEnd;
        [JSInvokable]
        public void NotifyZoomEnd(Event e) => OnZoomEnd?.Invoke(this, e);

        public event MapEventHandler? OnMoveEnd;
        [JSInvokable]
        public void NotifyMoveEnd(Event e) => OnMoveEnd?.Invoke(this, e);

        public event MouseEventHandler? OnMouseMove;
        [JSInvokable]
        public void NotifyMouseMove(MouseEvent eventArgs) => OnMouseMove?.Invoke(this, eventArgs);

        public event MapEventHandler? OnKeyPress;
        [JSInvokable]
        public void NotifyKeyPress(Event eventArgs) => OnKeyPress?.Invoke(this, eventArgs);

        public event MapEventHandler? OnKeyDown;
        [JSInvokable]
        public void NotifyKeyDown(Event eventArgs) => OnKeyDown?.Invoke(this, eventArgs);

        public event MapEventHandler? OnKeyUp;
        [JSInvokable]
        public void NotifyKeyUp(Event eventArgs) => OnKeyUp?.Invoke(this, eventArgs);

        public event MouseEventHandler? OnPreClick;
        [JSInvokable]
        public void NotifyPreClick(MouseEvent eventArgs) => OnPreClick?.Invoke(this, eventArgs);
        #endregion events

        #region InteractiveLayerEvents
        // Has the same events as InteractiveLayer, but it is not a layer. 
        // Could place this code in its own class and make Layer inherit from that, but not every layer is interactive...
        // Is there a way to not duplicate this code?

        public delegate void MouseEventHandler(Map sender, MouseEvent e);

        /// <summary>
        /// A Delegate for the event handler.
        /// </summary>
        /// <param name="elementFirst">A first string element</param>
        /// <param name="elementSecond">A second string element</param>
        public delegate void ElementClickedEventHandler(string? elementFirst, string? elementSecond);

        public event MouseEventHandler? OnClick;
        [JSInvokable]
        public void NotifyClick(MouseEvent eventArgs) => OnClick?.Invoke(this, eventArgs);

        public event MouseEventHandler? OnDblClick;
        [JSInvokable]
        public void NotifyDblClick(MouseEvent eventArgs) => OnDblClick?.Invoke(this, eventArgs);

        public event MouseEventHandler? OnMouseDown;
        [JSInvokable]
        public void NotifyMouseDown(MouseEvent eventArgs) => OnMouseDown?.Invoke(this, eventArgs);

        public event MouseEventHandler? OnMouseUp;
        [JSInvokable]
        public void NotifyMouseUp(MouseEvent eventArgs) => OnMouseUp?.Invoke(this, eventArgs);

        public event MouseEventHandler? OnMouseOver;
        [JSInvokable]
        public void NotifyMouseOver(MouseEvent eventArgs) => OnMouseOver?.Invoke(this, eventArgs);

        public event MouseEventHandler? OnMouseOut;
        [JSInvokable]
        public void NotifyMouseOut(MouseEvent eventArgs) => OnMouseOut?.Invoke(this, eventArgs);

        public event MouseEventHandler? OnContextMenu;
        [JSInvokable]
        public void NotifyContextMenu(MouseEvent eventArgs) => OnContextMenu?.Invoke(this, eventArgs);
        #endregion InteractiveLayerEvents
    }
}
