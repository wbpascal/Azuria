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
        /// </summary>
        /// <returns></returns>
        public static ApiRequest<HeaderDataModel[]> GetHeaderList()
        {
            return ApiRequest<HeaderDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/media/headerlist"));
        }

        /// <summary>
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public static ApiRequest<HeaderDataModel> GetRandomHeader(string style = "gray")
        {
            return ApiRequest<HeaderDataModel>.Create(
                new Uri($"{ApiConstants.ApiUrlV1}/media/randomheader?style={style}"));
        }

        #endregion
    }
}