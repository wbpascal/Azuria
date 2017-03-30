using System;

namespace Azuria.Api.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestBuilder : IApiRequestBuilder
    {
        private readonly IProxerClient _client;

        internal ApiRequestBuilder(IProxerClient client)
        {
            this._client = client;
        }

        #region Methods

        /// <inheritdoc />
        public IUrlBuilder FromUrl(Uri baseUri)
        {
            return new UrlBuilder(baseUri, this._client);
        }

        #endregion
    }
}