using System;
using Azuria.Api.v1.DataModels.Anime;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// </summary>
    public static class AnimeRequestBuilder
    {
        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ApiRequest<string> GetLink(int id)
        {
            return ApiRequest<string>.Create(new Uri($"{ApiConstants.ApiUrlV1}/anime/link?id={id}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="episode"></param>
        /// <param name="language"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<StreamDataModel[]> GetStreams(int id, int episode, string language,
            Senpai senpai = null)
        {
            return ApiRequest<StreamDataModel[]>.Create(
                    new Uri($"{ApiConstants.ApiUrlV1}/anime/streams?id={id}&episode={episode}&language={language}"))
                .WithSenpai(senpai);
        }

        #endregion
    }
}