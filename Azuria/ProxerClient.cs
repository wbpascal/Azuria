using System;
using Autofac;

namespace Azuria
{
    /// <summary>
    /// 
    /// </summary>
    public class ProxerClient : IProxerClient
    {
        private ProxerClient(char[] apiKey, ProxerClientOptions options)
        {
            this.ApiKey = apiKey;
            options.ContainerBuilder.RegisterInstance(this).As<IProxerClient>();
            this.Container = options.ContainerBuilder.Build();
        }

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public char[] ApiKey { get; }

        /// <summary>
        /// 
        /// </summary>
        public IContainer Container { get; }

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
            ProxerClientOptions lOptions = new ProxerClientOptions();
            optionsFactory?.Invoke(lOptions);
            return new ProxerClient(apiKey, lOptions);
        }

        #endregion
    }
}