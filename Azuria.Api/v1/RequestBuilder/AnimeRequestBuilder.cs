using System;
using Azuria.Api.v1.DataModels.Anime;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the anime api class.
    /// </summary>
    public static class AnimeRequestBuilder
    {
        #region Methods

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns the link of a specified stream.
        /// 
        /// Api permissions required:
        /// * Anime - Level 2
        /// </summary>
        /// <param name="id">The id of the stream.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns a link as a string.</returns>
        public static ApiRequest<string> GetLink(int id)
        {
            return ApiRequest<string>.Create(new Uri($"{ApiConstants.ApiUrlV1}/anime/link"))
                .WithGetParameter("id", id.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns all streams of a specified episode.
        /// 
        /// Api permissions required:
        /// * Anime - Level 2
        /// </summary>
        /// <param name="id">The id of the anime.</param>
        /// <param name="episode">The number of the episode.</param>
        /// <param name="language">The language of the episode.</param>
        /// <param name="user">
        /// Optional. The user that creates the request. If passed and logged in, the user will recieve anime
        /// points. Default: null
        /// </param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of streams.</returns>
        public static ApiRequest<StreamDataModel[]> GetStreams(int id, int episode, string language,
            IProxerUser user = null)
        {
            return ApiRequest<StreamDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/anime/streams"))
                .WithGetParameter("id", id.ToString())
                .WithGetParameter("episode", episode.ToString())
                .WithGetParameter("language", language).WithUser(user);
        }

        #endregion
    }
}