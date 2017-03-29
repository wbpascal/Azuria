using System;
using Autofac;

namespace Azuria.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ProxerClient : IProxerClient
    {
        private ProxerClient(char[] apiKey, ProxerClientOptions options)
        {
            this.ApiKey = apiKey;
            ContainerBuilder lContainerBuilder = new ContainerBuilder();
            this.ProcessOptions(options, lContainerBuilder);
            this.Container = lContainerBuilder.Build();
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

        /// <summary>
        /// 
        /// </summary>
        public bool IsAuthenticated { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="optionsFactory"></param>
        /// <returns></returns>
        public IProxerClient Create(char[] apiKey, Action<ProxerClientOptions> optionsFactory)
        {
            ProxerClientOptions lOptions = new ProxerClientOptions();
            optionsFactory(lOptions);
            return new ProxerClient(apiKey, lOptions);
        }

        private void ProcessOptions(ProxerClientOptions options, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterInstance(options.HttpClient);
            containerBuilder.RegisterInstance(this).As<IProxerClient>();
        }

        #endregion
    }
}