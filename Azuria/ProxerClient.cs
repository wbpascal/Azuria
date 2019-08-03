using System;
using Autofac;

namespace Azuria
{
    /// <summary>
    /// Represents a client that is used to connect to the api.
    /// </summary>
    public class ProxerClient : IProxerClient
    {
        private ProxerClient(char[] apiKey)
        {
            this.ApiKey = apiKey;
        }

        /// <inheritdoc />
        public char[] ApiKey { get; }

        /// <inheritdoc />
        public IContainer Container { get; private set; }

        /// <summary>
        /// Creates a new client with the specified api key and additional options.
        /// </summary>
        /// <param name="apiKey">The api key used by the created client.</param>
        /// <param name="optionsFactory">Optional. Additional creation options for the client.</param>
        /// <returns>A client with the specified api key and options.</returns>
        public static IProxerClient Create(char[] apiKey, Action<ProxerClientOptions> optionsFactory = null)
        {
            ProxerClient lClient = new ProxerClient(apiKey);
            ProxerClientOptions lOptions = new ProxerClientOptions(apiKey);
            optionsFactory?.Invoke(lOptions);
            lClient.ProcessOptions(lOptions);
            return lClient;
        }

        private void ProcessOptions(ProxerClientOptions options)
        {
            options.ContainerBuilder.RegisterInstance(this).As<IProxerClient>();
            this.Container = options.ContainerBuilder.Build();
        }
    }
}