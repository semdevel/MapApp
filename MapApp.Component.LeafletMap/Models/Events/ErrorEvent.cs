namespace MapApp.Component.LeafletMap.Models.Events
{
    /// <summary>
    /// https://leafletjs.com/reference.html#map-example
    /// </summary>
    public class ErrorEvent : Event
    {
        public string? Message { get; set; }
        public int Code { get; set; }
    }
}
