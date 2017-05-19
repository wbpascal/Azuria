using System;
using Autofac;
using Azuria.Api;
using Azuria.Api.Builder;
using Azuria.Authentication;
using Azuria.ErrorHandling;
using Azuria.Requests;
using Azuria.Requests.Http;
using Azuria.Serialization;

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
        /// <param name="apiKey"></param>
        public ProxerClientOptions(char[] apiKey)
        {
            this.ApiKey = apiKey;
            this.RegisterDefaultComponents(this.ContainerBuilder);
        }

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public char[] ApiKey { get; }

        /// <summary>
        /// Gets or sets the builder that is used to register dependencies.
        /// </summary>
        public ContainerBuilder ContainerBuilder { get; } = new ContainerBuilder();

        #endregion

        #region Methods

        private void RegisterDefaultComponents(ContainerBuilder builder)
        {
            builder.RegisterType<HttpClient>().As<IHttpClient>().SingleInstance();
            builder.RegisterType<LoginManager>()
                .As<ILoginManager>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(char[]), null));

            builder.RegisterType<ApiRequestBuilder>().As<IApiRequestBuilder>();
            builder.RegisterType<RequestHandler>().As<IRequestHandler>();
            builder.RegisterType<RequestErrorHandler>().As<IRequestErrorHandler>();
            builder.RegisterType<JsonDeserializer>().As<IJsonDeserializer>();
            builder.RegisterType<RequestHeaderManager>()
                .As<IRequestHeaderManager>()
                .WithParameter(new TypedParameter(typeof(char[]), this.ApiKey));

            builder.RegisterModule<ApiComponentsModule>();
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
            this.ContainerBuilder.RegisterType<LoginManager>()
                .As<ILoginManager>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(char[]), loginToken));
            return this;
        }

        ///  <summary>
        ///  Registers a custom <see cref="IHttpClient">http client</see> with the client that is used to make all
        ///  request of that client.
        ///  Overrides <see cref="WithCustomHttpClient(int, string)" />.
        ///  </summary>
        ///  <param name="factory">The factory that is used to create the http client.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="factory" /> ist null.</exception>
        public ProxerClientOptions WithCustomHttpClient(Func<IComponentContext, IHttpClient> factory)
        {
            if(factory == null) throw new ArgumentNullException(nameof(factory));
            this.ContainerBuilder.Register(factory).As<IHttpClient>().SingleInstance();
            return this;
        }

        /// <summary>
        /// Registers a <see cref="IHttpClient">http client</see> with custom timeout and/or user-agent to the client.
        ///
        /// Overrides <see cref="WithCustomHttpClient(System.Func{Autofac.IComponentContext,Azuria.Requests.Http.IHttpClient})" />.
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
        /// <param name="factory">The factory that is used to create the login manager.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="factory" /> ist null.</exception>
        public ProxerClientOptions WithCustomLoginManager(Func<IComponentContext, ILoginManager> factory)
        {
            if(factory == null) throw new ArgumentNullException(nameof(factory));
            this.ContainerBuilder.Register(factory).As<ILoginManager>().SingleInstance();
            return this;
        }

        #endregion
    }
}