using System;
using Autofac;
using Azuria.Api;
using Azuria.Authentication;
using Azuria.Connection;

namespace Azuria
{
    /// <summary>
    /// 
    /// </summary>
    public class ProxerClientOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public ProxerClientOptions()
        {
            RegisterComponents(this.ContainerBuilder);
        }

        #region Properties

        internal ContainerBuilder ContainerBuilder { get; set; } = new ContainerBuilder();

        #endregion

        #region Methods

        private static void RegisterComponents(ContainerBuilder builder)
        {
            builder.RegisterModule<ApiComponentModule>();
            builder.RegisterInstance(new HttpClient()).As<IHttpClient>();
            builder.RegisterInstance(new LoginManager()).As<ILoginManager>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginToken"></param>
        /// <returns></returns>
        public ProxerClientOptions WithAuthorisation(char[] loginToken)
        {
            this.ContainerBuilder.RegisterInstance(new LoginManager(loginToken)).As<ILoginManager>();
            return this;
        }

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