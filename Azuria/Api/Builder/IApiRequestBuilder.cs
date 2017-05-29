using System;
using Azuria.Requests.Builder;

namespace Azuria.Api.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApiRequestBuilder
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        IProxerClient ProxerClient { get; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        IRequestBuilder FromUrl(Uri baseUri);

        #endregion
    }
}