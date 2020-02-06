using Azuria.Middleware;

namespace Azuria
{
    /// <summary>
    /// Represents a client that is used to connect to the api.
    /// </summary>
    public interface IProxerClient
    {
        /// <summary>
        /// 
        /// </summary>
        char[] ApiKey { get; }

        /// <summary>
        /// 
        /// </summary>
        IPipeline Pipeline { get; }
    }
}