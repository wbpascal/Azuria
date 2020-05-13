using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Helpers.Extensions;

namespace Azuria.Requests.Builder
{
    /// <summary>
    /// </summary>
    public abstract class RequestBuilderBase : IRequestBuilderBase
    {
        private readonly Uri _baseUri;

        private readonly List<KeyValuePair<string, string>> _postArguments
            = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="client"></param>
        protected RequestBuilderBase(Uri baseUri, IProxerClient client)
        {
            this._baseUri = baseUri;
            this.Client = client;
        }

        /// <summary>
        /// </summary>
        /// <param name="builderBase"></param>
        protected RequestBuilderBase(RequestBuilderBase builderBase)
        {
            this._baseUri = builderBase._baseUri;
            this._postArguments = builderBase._postArguments;

            this.Client = builderBase.Client;
            this.CheckLogin = builderBase.CheckLogin;
            this.GetParameters = builderBase.GetParameters;
            this.Headers = builderBase.Headers;
        }

        /// <summary>
        /// </summary>
        public bool CheckLogin { get; private set; }

        /// <summary>
        /// </summary>
        public IProxerClient Client { get; }

        /// <summary>
        /// </summary>
        public IDictionary<string, string> GetParameters { get; } = new Dictionary<string, string>();

        /// <summary>
        /// </summary>
        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        /// <summary>
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> PostParameter => this._postArguments;

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void AddGetParameter(string key, string value)
        {
            this.GetParameters[key] = value;
        }

        /// <summary>
        /// TODO: Change to AddGetParameter(s)
        /// </summary>
        /// <param name="parameters"></param>
        protected void AddGetParameter(IDictionary<string, string> parameters)
        {
            this.GetParameters.AddOrUpdateRange(parameters);
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void AddHeader(string key, string value)
        {
            this.Headers[key] = value;
        }

        /// <summary>
        /// </summary>
        /// <param name="headers"></param>
        protected void AddHeader(IDictionary<string, string> headers)
        {
            this.Headers.AddOrUpdateRange(headers);
        }

        /// <summary>
        /// </summary>
        protected void AddLoginCheck(bool check)
        {
            this.CheckLogin = check;
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void AddPostArgument(string key, string value)
        {
            this._postArguments.Add(new KeyValuePair<string, string>(key, value));
        }

        /// <summary>
        /// </summary>
        /// <param name="args"></param>
        protected void AddPostArgument(IEnumerable<KeyValuePair<string, string>> args)
        {
            this._postArguments.AddRange(args);
        }

        /// <inheritdoc />
        public Uri BuildUri()
        {
            var lUriBuilder = new UriBuilder(this._baseUri);
            lUriBuilder.Query += this.GetParameters.Aggregate(
                    string.Empty, (s, pair) => $"&{pair.Key}={pair.Value}"
                )
                .RemoveIfNotEmpty(0, 1);
            return lUriBuilder.Uri;
        }
    }
}