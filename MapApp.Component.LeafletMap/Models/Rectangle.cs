namespace MapApp.Component.LeafletMap.Models
{
    /// <summary>
    /// A class for drawing rectangle overlays on a map. Extends Polygon.
    /// <example>
    /// Example:
    /// <code>
    /// [[54.559322, -5.767822], [56.1210604, -3.021240]]
    /// </code>
    /// </example>
    /// https://leafletjs.com/reference.html#map-example
    /// </summary>
    public class Rectangle : Polyline<System.Drawing.RectangleF>;
}
