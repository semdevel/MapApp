using System.Drawing;

namespace MapApp.Component.LeafletMap.Models
{
    /// <summary>
    /// https://leafletjs.com/reference.html#map-example
    /// </summary>
    public class LatLng
    {
        public float Lat { get; set; }
        public float Lng { get; set; }
        public float Alt { get; set; }
        public float Acc { get; set; }
        public PointF ToPointF() => new(Lat, Lng);
        public LatLng() { }
        public LatLng(PointF position) : this(position.X, position.Y) { }
        public LatLng(float lat, float lng)
        {
            Lat = lat;
            Lng = lng;
        }
        //public LatLng(float lat, float lng, float alt) : this(lat, lng)
        //{
        //    Alt = alt;
        //}
        public LatLng(float lat, float lng, float acc) : this(lat, lng)
        {
            Acc = acc;
        }
    }
}
