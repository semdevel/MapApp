namespace MapApp.Component.LeafletMap.Models.Events
{
    /// <summary>
    /// https://leafletjs.com/reference.html#map-example
    /// </summary>
    public class TooltipEvent : Event
    {
        public Tooltip? Tooltip { get; set; }
    }
}
