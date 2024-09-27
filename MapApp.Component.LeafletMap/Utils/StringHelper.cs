namespace MapApp.Component.LeafletMap.Utils
{
    /// <summary>
    /// Creates an identifier for every created layer.
    /// </summary>
    public static class StringHelper
    {
        private static readonly Random _random = new();
        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
