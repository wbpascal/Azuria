using System;
using Autofac;
using Azuria.Connection;

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
            RegisterComponents(options.ContainerBuilder);
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
        public static IProxerClient Create(char[] apiKey, Action<ProxerClientOptions> optionsFactory = null)
        {
            ProxerClientOptions lOptions = new ProxerClientOptions();
            optionsFactory?.Invoke(lOptions);
            return new ProxerClient(apiKey, lOptions);
        }

        private static void RegisterComponents(ContainerBuilder builder)
        {
            builder.RegisterInstance(new HttpClient()).As<IHttpClient>();
        }

        #endregion
    }
}