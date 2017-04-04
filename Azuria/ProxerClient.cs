using System;
using Autofac;

namespace Azuria
{
    /// <summary>
    /// 
    /// </summary>
    public class ProxerClient : IProxerClient
    {
        private ProxerClient(char[] apiKey)
        {
            this.ApiKey = apiKey;
        }

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public char[] ApiKey { get; }

        /// <summary>
        /// 
        /// </summary>
        public IContainer Container { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="optionsFactory"></param>
        /// <returns></returns>
        public static IProxerClient Create(char[] apiKey, Action<ProxerClientOptions> optionsFactory = null)
        {
            ProxerClient lClient = new ProxerClient(apiKey);
            ProxerClientOptions lOptions = new ProxerClientOptions(lClient);
            optionsFactory?.Invoke(lOptions);
            lClient.ProcessOptions(lOptions);
            return lClient;
        }

        private void ProcessOptions(ProxerClientOptions options)
        {
            options.ContainerBuilder.RegisterInstance(this).As<IProxerClient>();
            this.Container = options.ContainerBuilder.Build();
        }

        #endregion
    }
}