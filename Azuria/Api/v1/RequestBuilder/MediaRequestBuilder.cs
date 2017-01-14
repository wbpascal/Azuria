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
        /// Creates an <see cref="ApiRequest"/> instance that...
        /// 
        /// Api permissions required:
        ///  * Media - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="ApiRequest"/> that returns...</returns>
        public static ApiRequest<HeaderDataModel[]> GetHeaderList()
        {
            return ApiRequest<HeaderDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/media/headerlist"));
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest"/> instance that...
        /// 
        /// Api permissions required:
        ///  * Media - Level 0
        /// </summary>
        /// <param name="style"></param>
        /// <returns>An instance of <see cref="ApiRequest"/> that returns...</returns>
        public static ApiRequest<HeaderDataModel> GetRandomHeader(string style = "gray")
        {
            return ApiRequest<HeaderDataModel>.Create(
                new Uri($"{ApiConstants.ApiUrlV1}/media/randomheader?style={style}"));
        }

        #endregion
    }
}