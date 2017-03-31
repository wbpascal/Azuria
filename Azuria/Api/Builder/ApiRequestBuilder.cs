using System;

namespace Azuria.Api.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestBuilder : IApiRequestBuilder
    {
        internal ApiRequestBuilder(IProxerClient client)
        {
            this.ProxerClient = client;
        }

        #region Properties

        /// <inheritdoc />
        public IProxerClient ProxerClient { get; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public IUrlBuilder FromUrl(Uri baseUri)
        {
            return new UrlBuilder(baseUri, this.ProxerClient);
        }

        #endregion
    }
}