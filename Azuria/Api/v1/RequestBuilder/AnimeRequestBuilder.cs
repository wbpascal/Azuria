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
        /// Creates an <see cref="ApiRequest"/> instance that returns the link of a specified stream.
        /// 
        /// Api permissions required:
        ///  * Anime - Level 2
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An instance of <see cref="ApiRequest"/> that returns a link as a string.</returns>
        public static ApiRequest<string> GetLink(int id)
        {
            return ApiRequest<string>.Create(new Uri($"{ApiConstants.ApiUrlV1}/anime/link?id={id}"));
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest"/> instance that returns all streams of a specified episode.
        /// 
        /// Api permissions required:
        ///  * Anime - Level 2
        /// </summary>
        /// <param name="id"></param>
        /// <param name="episode"></param>
        /// <param name="language"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest"/> that returns an array of streams.</returns>
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