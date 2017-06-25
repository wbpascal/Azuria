using System;
using Azuria.Requests.Builder;

namespace Azuria.Api.Builder
{
    /// <summary>
    /// </summary>
    public interface IApiRequestBuilder
    {
        /// <summary>
        /// </summary>
        IProxerClient ProxerClient { get; }

        /// <summary>
        /// </summary>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        IRequestBuilder FromUrl(Uri baseUri);
    }
}