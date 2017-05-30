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

        /// <inheritdoc />
        public IProxerClient ProxerClient { get; }

        /// <inheritdoc />
        public IRequestBuilder FromUrl(Uri baseUri)
        {
            return new RequestBuilder(baseUri, this.ProxerClient);
        }
    }
}