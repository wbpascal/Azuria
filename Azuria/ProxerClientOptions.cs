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
        public ProxerClientOptions(IProxerClient client)
        {
            this.Client = client;
            this.RegisterComponents(this.ContainerBuilder);
        }

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public IProxerClient Client { get; }

        /// <summary>
        /// 
        /// </summary>
        public ContainerBuilder ContainerBuilder { get; } = new ContainerBuilder();

        #endregion

        #region Methods

        private void RegisterComponents(ContainerBuilder builder)
        {
            builder.RegisterModule<ApiComponentModule>();
            builder.RegisterInstance(this.Client);
            builder.RegisterInstance(new HttpClient()).As<IHttpClient>();
            builder.RegisterInstance(new LoginManager(this.Client)).As<ILoginManager>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginToken"></param>
        /// <returns></returns>
        public ProxerClientOptions WithAuthorisation(char[] loginToken)
        {
            this.ContainerBuilder.RegisterInstance(new LoginManager(this.Client, loginToken))
                .As<ILoginManager>();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public ProxerClientOptions WithCustomLoginManager(Func<IProxerClient, ILoginManager> factory)
        {
            ILoginManager lLoginManager = factory?.Invoke(this.Client);
            if (lLoginManager != null) this.ContainerBuilder.RegisterInstance(lLoginManager);
            return this;
        }

        #endregion
    }
}