using System.Drawing;

namespace MapApp.Component.LeafletMap.Models.Events
{
    /// <summary>
    /// https://leafletjs.com/reference.html#map-example
    /// </summary>
    public class ResizeEvent : Event
    {
        public PointF OldSize { get; set; }
        public PointF NewSize { get; set; }
    }
}
