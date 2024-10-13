namespace MapApp.Component.LeafletMap.Models;

/// <summary>
/// An alternative object as replacement for Tuple used for bounds definition.
/// </summary>
public class LatLngPair
{
    /// <summary>
    /// Lat, Lng of the first bound.
    /// </summary>
    public LatLng? First { get; set; }

    /// <summary>
    /// Lat, Lng of the second bound.
    /// </summary>
    public LatLng? Second { get; set; }
}
