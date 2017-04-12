using System;
using Azuria.Api.Builder;
using Azuria.Api.v1.DataModels.Media;
using Azuria.Enums.Media;
using Azuria.Helpers.Extensions;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the media api class.
    /// </summary>
    public class MediaRequestBuilder
    {
        private readonly IProxerClient _client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public MediaRequestBuilder(IProxerClient client)
        {
            this._client = client;
        }

        #region Methods

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns an array of all current headers.
        /// 
        /// Api permissions required:
        /// * Media - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of headers.</returns>
        public IUrlBuilderWithResult<HeaderDataModel[]> GetHeaderList()
        {
            return new UrlBuilder<HeaderDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/media/headerlist"), this._client
            );
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns a random header for an optional specified style.
        /// 
        /// Api permissions required:
        /// * Media - Level 0
        /// </summary>
        /// <param name="style">Optional. The style of the returned header.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns a header.</returns>
        public IUrlBuilderWithResult<HeaderDataModel> GetRandomHeader(HeaderStyle style = HeaderStyle.Gray)
        {
            return new UrlBuilder<HeaderDataModel>(
                new Uri($"{ApiConstants.ApiUrlV1}/media/randomheader"), this._client
            ).WithGetParameter("style", style.ToTypeString());
        }

        #endregion
    }
}