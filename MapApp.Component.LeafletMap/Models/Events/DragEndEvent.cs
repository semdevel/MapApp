namespace MapApp.Component.LeafletMap.Models.Events
{
    /// <summary>
    /// https://leafletjs.com/reference.html#map-example
    /// </summary>
    public class DragEndEvent : Event
    {
        public float Distance { get; set; }
    }
}
