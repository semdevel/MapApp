namespace MapApp.Component.LeafletMap.Models
{
    /// <summary>
    /// Shapefile layer - Requires Leaflet.Shapefile plugin
    /// https://leafletjs.com/reference.html#map-example
    /// </summary>
    public class ShapefileLayer : Layer
    {
        /// <summary>
        /// Instantiates a tile layer object given a URL template.
        /// </summary>
        public string? UrlTemplate { get; set; }
    }
}
