namespace MapApp.Component.LeafletMap.Models.Events
{
    /// <summary>
    /// https://leafletjs.com/reference.html#map-example
    /// </summary>
    public class PopupEvent : Event
    {
        public Popup? Popup { get; set; }
    }
}
