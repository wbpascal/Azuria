using System;
using System.Collections.Generic;
using Azuria.Authentication;
using Azuria.Helpers;
using Azuria.Middleware;
using Azuria.Requests.Http;
using Azuria.Serialization;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace Azuria
{
    /// <summary>
    /// Represents the options that are used to create a <see cref="IProxerClient">proxer client</see>.
    /// TODO: Check documentation for changes
    /// </summary>
    public class ProxerClientOptions
    {
        private IHttpClient _httpClient = new HttpClient();
        private IJsonDeserializer _jsonDeserializer = new JsonDeserializer();

        /// <summary>
        /// Creates a new instance of <see cref="ProxerClientOptions" />.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="client"></param>
        protected internal ProxerClientOptions(char[] apiKey, IProxerClient client)
        {
            this.Client = client;
            this.ApiKey = apiKey;
            this.Pipeline = CreateDefaultPipeline(apiKey, client);
        }

        /// <summary>
        /// 
        /// </summary>
        public char[] ApiKey { get; }

        /// <summary>
        /// 
        /// </summary>
        public IProxerClient Client { get; }

        /// <summary>
        /// 
        /// </summary>
        public IPipeline Pipeline { get; set; }

        /// <summary>
        /// Inserts or overwrites the default <see cref="LoginMiddleware">login middleware</see> with a new
        /// <see cref="DefaultLoginManager">login manager</see> that may contain an optional login token.
        /// Overwrites result of <see cref="WithCustomLoginManager" />.
        /// </summary>
        /// <param name="loginToken">
        /// Optional. The login token the <see cref="DefaultLoginManager">login manager</see> will be inserted with. If none or
        /// <code>null</code> was given, the <see cref="DefaultLoginManager">login manager</see> needs to be authenticated
        /// first before it can work.
        /// TODO: How to authenticate
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown if the <paramref name="loginToken">login token</paramref> is not null and less then 255 characters long.
        /// </exception>
        public ProxerClientOptions WithAuthentication(char[] loginToken = null)
        {
            if (loginToken != null && loginToken.Length != 255)
                throw new ArgumentException("A valid login token must be 255 characters long", nameof(loginToken));
            return this.WithCustomLoginManager(new DefaultLoginManager(this.Client, loginToken));
        }

        /// <summary>
        /// Overwrites the default <see cref="HttpJsonRequestMiddleware" /> with one that contains the given custom
        /// <see cref="IHttpClient">http client</see> that is then used to make all requests of that client.
        /// Overwrites result of <see cref="WithCustomHttpClient(int)" />.
        /// If <see cref="Pipeline"/> does not contain any instances of <see cref="HttpJsonRequestMiddleware" />, nothing 
        /// is done.
        /// </summary>
        /// <param name="client">The http client.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="client" /> ist null.</exception>
        public ProxerClientOptions WithCustomHttpClient(IHttpClient client)
        {
            this._httpClient = client ?? throw new ArgumentNullException(nameof(client));
            this.Pipeline.ReplaceMiddleware(typeof(HttpJsonRequestMiddleware),
                new HttpJsonRequestMiddleware(this._httpClient, this._jsonDeserializer));
            return this;
        }

        /// <summary>
        /// Overwrites the default <see cref="HttpJsonRequestMiddleware" /> with one that contains a http client
        /// with the given custom timeout.
        /// Overwrites result of <see cref="WithCustomHttpClient(IHttpClient)" />.
        /// If <see cref="Pipeline"/> does not contain any instances of <see cref="HttpJsonRequestMiddleware" />, nothing 
        /// is done.
        /// </summary>
        /// <param name="timeout">Optional. The custom timeout of the http client.</param>
        public ProxerClientOptions WithCustomHttpClient(int timeout)
        {
            return this.WithCustomHttpClient(new HttpClient(timeout));
        }

        /// <summary>
        /// Overwrites the default <see cref="HttpJsonRequestMiddleware" /> with one that contains the given custom
        /// <see cref="IJsonDeserializer" />.
        /// If <see cref="Pipeline"/> does not contain any instances of <see cref="HttpJsonRequestMiddleware" />, nothing 
        /// is done.
        /// </summary>
        /// <param name="deserializer">The new deserializer.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="deserializer" /> ist null.</exception>
        public ProxerClientOptions WithCustomJsonDeserializer(IJsonDeserializer deserializer)
        {
            this._jsonDeserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
            this.Pipeline.ReplaceMiddleware(typeof(HttpJsonRequestMiddleware),
                new HttpJsonRequestMiddleware(this._httpClient, this._jsonDeserializer));
            return this;
        }

        /// <summary>
        /// Inserts or overwrites the default <see cref="LoginMiddleware">login middleware</see> with a given
        /// <see cref="ILoginManager" />.
        /// Overwrites result of <see cref="WithAuthentication" />.
        /// </summary>
        /// <param name="loginManager">The login manager.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="loginManager" /> ist null.</exception>
        public ProxerClientOptions WithCustomLoginManager(ILoginManager loginManager)
        {
            if (loginManager == null) throw new ArgumentNullException(nameof(loginManager));

            // Try to replace the login middleware first, if not possible insert new middleware after StaticHeaderMiddleware
            if (!this.Pipeline.ReplaceMiddleware(typeof(LoginMiddleware), new LoginMiddleware(loginManager)))
                this.Pipeline.InsertMiddlewareAfter(typeof(StaticHeaderMiddleware), new LoginMiddleware(loginManager));

            return this;
        }

        /// <summary>
        /// Overwrites the default <see cref="StaticHeaderMiddleware"/> so that it contains the new user agent.
        /// If <see cref="Pipeline"/> does not contain any instances of <see cref="StaticHeaderMiddleware" />, nothing 
        /// is done.
        /// </summary>
        /// <param name="userAgentExtra">Extra string that should be appended to the standard user agent.</param>
        public ProxerClientOptions WithExtraUserAgent(string userAgentExtra)
        {
            var middleware = new StaticHeaderMiddleware(CreateDefaultHeaders(this.ApiKey, userAgentExtra));
            this.Pipeline.ReplaceMiddleware(typeof(StaticHeaderMiddleware), middleware);
            return this;
        }

        private IPipeline CreateDefaultPipeline(char[] apiKey, IProxerClient client)
        {
            var middlewares = new List<IMiddleware>
            {
                new StaticHeaderMiddleware(CreateDefaultHeaders(apiKey)), // First to start execution
                new ErrorMiddleware(),
                new HttpJsonRequestMiddleware(this._httpClient, this._jsonDeserializer) // Last to start execution
            };

            return new Pipeline(middlewares);
        }

        private static IDictionary<string, string> CreateDefaultHeaders(char[] apiKey, string userAgentExtra = "")
        {
            return new Dictionary<string, string>()
            {
                {"proxer-api-key", apiKey.ToString()},
                {
                    "User-Agent",
                    $"Azuria/{VersionHelper.GetAssemblyVersion(typeof(HttpClient))} {userAgentExtra}".TrimEnd()
                }
            };
        }
    }
}