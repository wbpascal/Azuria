using System;
using Azuria.Api.Builder;
using Azuria.Api.v1.DataModels.Anime;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the anime api class.
    /// </summary>
    public class AnimeRequestBuilder
    {
        private readonly IProxerClient _client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public AnimeRequestBuilder(IProxerClient client)
        {
            this._client = client;
        }

        #region Methods

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns the link of a specified stream.
        /// 
        /// Api permissions required:
        /// * Anime - Level 2
        /// </summary>
        /// <param name="id">The id of the stream.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns a link as a string.</returns>
        public IUrlBuilderWithResult<string> GetLink(int id)
        {
            return new UrlBuilder<string>(new Uri($"{ApiConstants.ApiUrlV1}/anime/link"), this._client)
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
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of streams.</returns>
        public IUrlBuilderWithResult<StreamDataModel[]> GetStreams(int id, int episode, string language)
        {
            return new UrlBuilder<StreamDataModel[]>(new Uri($"{ApiConstants.ApiUrlV1}/anime/streams"), this._client)
                .WithGetParameter("id", id.ToString())
                .WithGetParameter("episode", episode.ToString())
                .WithGetParameter("language", language);
        }

        #endregion
    }
}