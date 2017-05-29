using System;
using Azuria.Requests.Builder;

namespace Azuria.Api.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestBuilder : IApiRequestBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public ApiRequestBuilder(IProxerClient client)
        {
            this.ProxerClient = client;
        }

        #region Properties

        /// <inheritdoc />
        public IProxerClient ProxerClient { get; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public IRequestBuilder FromUrl(Uri baseUri)
        {
            return new RequestBuilder(baseUri, this.ProxerClient);
        }

        #endregion
    }
}