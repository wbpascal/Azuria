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
            builder.RegisterInstance(this.Client);
            builder.RegisterInstance(new HttpClient()).As<IHttpClient>();
            builder.RegisterInstance(new LoginManager(this.Client)).As<ILoginManager>();
        }

        /// <summary>
        /// Overrides <see cref="WithCustomLoginManager" />.
        /// </summary>
        /// <param name="loginToken"></param>
        /// <exception cref="ArgumentException">
        /// Thrown if the <paramref name="loginToken">login token</paramref> is null or less then 255 characters long.
        /// </exception>
        /// <returns></returns>
        public ProxerClientOptions WithAuthorisation(char[] loginToken)
        {
            if (loginToken?.Length != 255)
                throw new ArgumentException("A valid login token must be 255 characters long", nameof(loginToken));
            this.ContainerBuilder.RegisterInstance(new LoginManager(this.Client, loginToken))
                .As<ILoginManager>();
            return this;
        }

        /// <summary>
        /// Overrides <see cref="WithCustomHttpClient(int, string)" />.
        /// </summary>
        /// <param name="client"></param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="client"/> ist null.</exception>
        /// <returns></returns>
        public ProxerClientOptions WithCustomHttpClient(IHttpClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            this.ContainerBuilder.RegisterInstance(client);
            return this;
        }

        /// <summary>
        /// Overrides <see cref="WithCustomHttpClient(IHttpClient)" />.
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
        /// Overrides <see cref="WithAuthorisation" />.
        /// </summary>
        /// <param name="factory"></param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <see cref="ILoginManager"/> created through the <paramref name="factory"/> is null.
        /// </exception>
        /// <returns></returns>
        public ProxerClientOptions WithCustomLoginManager(Func<IProxerClient, ILoginManager> factory)
        {
            ILoginManager lLoginManager = factory?.Invoke(this.Client);
            if(lLoginManager == null) throw new ArgumentNullException();
            this.ContainerBuilder.RegisterInstance(lLoginManager);
            return this;
        }

        #endregion
    }
}