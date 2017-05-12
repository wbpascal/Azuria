using System;
using Autofac;
using Azuria.Authentication;
using Azuria.Connection;

namespace Azuria
{
    /// <summary>
    /// Represents the options that are used to create a <see cref="IProxerClient">proxer client</see>.
    /// </summary>
    public class ProxerClientOptions
    {
        /// <summary>
        /// Creates a new instance of <see cref="ProxerClientOptions"/>.
        /// </summary>
        /// <param name="client">The client that is created with the options.</param>
        public ProxerClientOptions(IProxerClient client)
        {
            this.Client = client;
            this.RegisterDefaultComponents(this.ContainerBuilder);
        }

        #region Properties

        /// <summary>
        /// Gets or sets the client that is created with the options.
        /// </summary>
        public IProxerClient Client { get; }

        /// <summary>
        /// Gets or sets the builder that is used to register dependencies.
        /// </summary>
        public ContainerBuilder ContainerBuilder { get; } = new ContainerBuilder();

        #endregion

        #region Methods

        private void RegisterDefaultComponents(ContainerBuilder builder)
        {
            builder.RegisterInstance(this.Client);
            builder.RegisterInstance(new HttpClient()).As<IHttpClient>();
            builder.RegisterInstance(new LoginManager(this.Client)).As<ILoginManager>();
        }

        /// <summary>
        /// Registers a <see cref="ILoginManager">login manager</see> with the specified login token to the client
        /// that will authenticate on the first request made.
        ///
        /// Overrides <see cref="WithCustomLoginManager" />.
        /// </summary>
        /// <param name="loginToken">
        /// The login token the <see cref="ILoginManager">login manager</see> is registered with.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown if the <paramref name="loginToken">login token</paramref> is null or less then 255 characters long.
        /// </exception>
        public ProxerClientOptions WithAuthorisation(char[] loginToken)
        {
            if (loginToken?.Length != 255)
                throw new ArgumentException("A valid login token must be 255 characters long", nameof(loginToken));
            this.ContainerBuilder.RegisterInstance(new LoginManager(this.Client, loginToken))
                .As<ILoginManager>();
            return this;
        }

        /// <summary>
        /// Registers a custom <see cref="IHttpClient">http client</see> with the client that is used to make all
        /// request of that client.
        ///
        /// Overrides <see cref="WithCustomHttpClient(int, string)" />.
        /// </summary>
        /// <param name="client">The custom http client that should be registered with the client.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="client" /> ist null.</exception>
        public ProxerClientOptions WithCustomHttpClient(IHttpClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            this.ContainerBuilder.RegisterInstance(client);
            return this;
        }

        /// <summary>
        /// Registers a <see cref="IHttpClient">http client</see> with custom timeout and/or user-agent to the client.
        ///
        /// Overrides <see cref="WithCustomHttpClient(IHttpClient)" />.
        /// </summary>
        /// <param name="timeout">Optional. The custom timeout of the http client.</param>
        /// <param name="userAgentExtra">Optional. A string that is appended to the user-agent of the http client.</param>
        public ProxerClientOptions WithCustomHttpClient(int timeout = 5000, string userAgentExtra = "")
        {
            this.ContainerBuilder.RegisterInstance(new HttpClient(timeout, userAgentExtra)).As<IHttpClient>();
            return this;
        }

        /// <summary>
        /// Registers a custom <see cref="ILoginManager">login manager</see> with the client.
        ///
        /// Overrides <see cref="WithAuthorisation" />.
        /// </summary>
        /// <param name="factory">The factory that is used to create the custom login manager.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <see cref="ILoginManager" /> created through the <paramref name="factory" /> is null.
        /// </exception>
        public ProxerClientOptions WithCustomLoginManager(Func<IProxerClient, ILoginManager> factory)
        {
            ILoginManager lLoginManager = factory?.Invoke(this.Client);
            if (lLoginManager == null) throw new ArgumentNullException();
            this.ContainerBuilder.RegisterInstance(lLoginManager);
            return this;
        }

        #endregion
    }
}