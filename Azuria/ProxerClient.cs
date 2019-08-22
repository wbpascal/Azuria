using System;
using Azuria.Middleware.Pipeline;

namespace Azuria
{
    /// <summary>
    /// Represents a client that is used to connect to the api.
    /// </summary>
    public class ProxerClient : IProxerClient
    {
        private ProxerClient()
        {
        }

        /// <inheritdoc />
        public char[] ApiKey { get; private set; }

        /// <inheritdoc />
        public IPipeline Pipeline { get; private set; }

        /// <summary>
        /// Creates a new client with the specified api key and additional options.
        /// </summary>
        /// <param name="apiKey">The api key used by the created client.</param>
        /// <param name="optionsFactory">Optional. Additional creation options for the client.</param>
        /// <returns>A client with the specified api key and options.</returns>
        public static IProxerClient Create(char[] apiKey, Action<ProxerClientOptions> optionsFactory = null)
        {
            ProxerClient client = new ProxerClient();
            ProxerClientOptions lOptions = new ProxerClientOptions(apiKey, client);
            optionsFactory?.Invoke(lOptions);
            client.ApiKey = lOptions.ApiKey;
            client.Pipeline = lOptions.Pipeline;
            return client;
        }
    }
}