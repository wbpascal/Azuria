using Autofac;

namespace Azuria
{
    /// <summary>
    /// Represents a client that is used to connect to the api.
    /// </summary>
    public interface IProxerClient
    {
        /// <summary>
        /// Gets the api key the client uses.
        /// </summary>
        char[] ApiKey { get; }

        /// <summary>
        /// Gets the container that is used to resolve dependencies for this client.
        /// </summary>
        IContainer Container { get; }
    }
}