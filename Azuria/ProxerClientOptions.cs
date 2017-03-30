using System;
using Autofac;
using Azuria.Connection;

namespace Azuria
{
    /// <summary>
    /// 
    /// </summary>
    public class ProxerClientOptions
    {
        #region Properties

        internal ContainerBuilder ContainerBuilder { get; set; } = new ContainerBuilder();

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public ProxerClientOptions WithCustomHttpClient(IHttpClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            this.ContainerBuilder.RegisterInstance(client);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="userAgentExtra"></param>
        /// <returns></returns>
        public ProxerClientOptions WithCustomHttpClient(int timeout = 5000, string userAgentExtra = "")
        {
            this.ContainerBuilder.RegisterInstance(new HttpClient(timeout, userAgentExtra)).As<IHttpClient>();
            return this;
        }

        #endregion
    }
}