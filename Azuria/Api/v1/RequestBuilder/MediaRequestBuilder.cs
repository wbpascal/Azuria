using System;
using Azuria.Api.v1.DataModels.Media;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// </summary>
    public static class MediaRequestBuilder
    {
        #region Methods

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns an array of all current headers.
        /// 
        /// Api permissions required:
        /// * Media - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of headers.</returns>
        public static ApiRequest<HeaderDataModel[]> GetHeaderList()
        {
            return ApiRequest<HeaderDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/media/headerlist"));
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns a random header for an optional specified style.
        /// 
        /// Api permissions required:
        /// * Media - Level 0
        /// </summary>
        /// <param name="style">
        /// Optional. The style of the returned header. Possible values: "gray", "black", "old_blue", "pantsu".
        /// Default: "gray"
        /// </param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns a header.</returns>
        public static ApiRequest<HeaderDataModel> GetRandomHeader(string style = "gray")
        {
            return ApiRequest<HeaderDataModel>.Create(
                new Uri($"{ApiConstants.ApiUrlV1}/media/randomheader?style={style}"));
        }

        #endregion
    }
}