using System;

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
        public IReadOnlyClientOptions ClientOptions { get; private set; }

        /// <summary>
        /// Creates a new client with the specified api key and additional options.
        /// </summary>
        /// <param name="apiKey">The api key used by the created client.</param>
        /// <param name="optionsFactory">Optional. Additional creation options for the client.</param>
        /// <returns>A client with the specified api key and options.</returns>
        public static IProxerClient Create(char[] apiKey, Action<ProxerClientOptions> optionsFactory = null)
        {
            ProxerClient lClient = new ProxerClient();
            ProxerClientOptions lOptions = new ProxerClientOptions(apiKey);
            optionsFactory?.Invoke(lOptions);
            lClient.ClientOptions = lOptions;
            return lClient;
        }
    }
}