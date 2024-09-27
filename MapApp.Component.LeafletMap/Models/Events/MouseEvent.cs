using System.Drawing;

namespace MapApp.Component.LeafletMap.Models.Events
{
    /// <summary>
    /// https://leafletjs.com/reference.html#map-example
    /// </summary>
    public class MouseEvent : Event
    {
        public LatLng? LatLng { get; set; }
        public PointF LayerPoint { get; set; }
        public PointF ContainerPoint { get; set; }

    }
}
