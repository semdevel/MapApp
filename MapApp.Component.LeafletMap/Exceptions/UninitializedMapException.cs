namespace MapApp.Component.LeafletMap.Exceptions
{
    /// <summary>
    /// Exception thrown when the user tried to manipulate the map before it has been initialized.
    /// https://leafletjs.com/reference.html#map-example
    /// </summary>
    public class UninitializedMapException : Exception
    {
        public UninitializedMapException()
        {
        }
        public UninitializedMapException(string message) : base(message)
        {
        }

        public UninitializedMapException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}