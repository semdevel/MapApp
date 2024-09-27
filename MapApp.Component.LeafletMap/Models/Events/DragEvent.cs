namespace MapApp.Component.LeafletMap.Models.Events
{
    /// <summary>
    /// https://leafletjs.com/reference.html#map-example
    /// </summary>
    public class DragEvent : Event
    {
        public LatLng? LatLng { get; set; }
        public LatLng? OldLatLng { get; set; }
    }
}
