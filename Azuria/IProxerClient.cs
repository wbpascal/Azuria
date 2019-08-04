namespace Azuria
{
    /// <summary>
    /// Represents a client that is used to connect to the api.
    /// </summary>
    public interface IProxerClient
    {
        /// <summary>
        /// Gets the client options for this client.
        /// </summary>
        IReadOnlyClientOptions ClientOptions { get; }
    }
}