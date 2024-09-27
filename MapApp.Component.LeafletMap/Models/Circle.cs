namespace MapApp.Component.LeafletMap.Models
{
    /// <summary>
    /// https://leafletjs.com/reference.html#map-example
    /// </summary>
    public class Circle : Path
    {
        /// <summary>
        /// Center of the circle.
        /// </summary>
        public LatLng? Position { get; set; }

        /// <summary>
        /// Radius of the circle, in meters.
        /// </summary>
        public float Radius { get; set; }
    }
}
