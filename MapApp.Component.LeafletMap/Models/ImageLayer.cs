using System.Drawing;

namespace MapApp.Component.LeafletMap.Models
{
    /// <summary>
    /// https://leafletjs.com/reference.html#map-example
    /// </summary>
    public class ImageLayer(string url, PointF corner1, PointF corner2) : InteractiveLayer
    {
        /// <summary>
        /// The opacity of the image overlay.
        /// </summary>
        public float Opacity { get; set; } = 1.0f;
        /// <summary>
        /// Text for the alt attribute of the image (useful for accessibility).
        /// </summary>
        public string Alt { get; set; } = string.Empty;
        /// <summary>
        /// Whether the crossOrigin attribute will be added to the image. If a String is provided, the image will have its crossOrigin attribute set to the String provided. This is needed if you want to access image pixel data. Refer to CORS Settings for valid String values.
        /// </summary>
        public string? CrossOrigin { get; set; }
        /// <summary>
        /// URL to the overlay image to show in place of the overlay that failed to load.
        /// </summary>
        public string ErrorOverlayUrl { get; set; } = string.Empty;
        /// <summary>
        /// The explicit zIndex of the overlay layer.
        /// </summary>
        public int ZIndex { get; set; } = 1;
        /// <summary>
        /// A custom class name to assign to the image. Empty by default.
        /// </summary>
        public string ClassName { get; set; } = string.Empty;
        /// <summary>
        ///  Image url.
        /// </summary>
        public string Url { get; } = url;
        public PointF Corner1 { get; } = corner1;
        public PointF Corner2 { get; } = corner2;
    }
}