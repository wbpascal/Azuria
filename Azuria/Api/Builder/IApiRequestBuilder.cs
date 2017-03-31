using System;

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
        IUrlBuilder FromUrl(Uri baseUri);

        #endregion
    }
}