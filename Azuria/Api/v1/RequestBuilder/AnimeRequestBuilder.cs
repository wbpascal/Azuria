using System;
using Azuria.Api.Builder;
using Azuria.Api.v1.DataModels.Anime;
using Azuria.Enums.Info;
using Azuria.Requests.Builder;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the anime api class.
    /// </summary>
    public class AnimeRequestBuilder : IApiClassRequestBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public AnimeRequestBuilder(IProxerClient client)
        {
            this.ProxerClient = client;
        }

        /// <inheritdoc />
        public IProxerClient ProxerClient { get; }

        /// <summary>
        /// Builds a request that returns the link of a specified stream.
        /// 
        /// Api permissions required (class - permission level):
        /// * Anime - Level 2
        /// </summary>
        /// <param name="id">The id of the stream.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns a link as a string.</returns>
        public IRequestBuilderWithResult<string> GetLink(int id)
        {
            return new RequestBuilder<string>(new Uri($"{ApiConstants.ApiUrlV1}/anime/link"), this.ProxerClient)
                .WithGetParameter("id", id.ToString());
        }

        /// <summary>
        /// Builds a request that returns all streams (including the Proxerstream) of a specified episode.
        /// 
        /// Api permissions required (class - permission level):
        /// * Anime - Level 3
        /// </summary>
        /// <param name="id">The id of the anime.</param>
        /// <param name="episode">The number of the episode.</param>
        /// <param name="language">The language of the episode.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of streams.</returns>
        public IRequestBuilderWithResult<StreamDataModel[]> GetProxerStreams(
            int id, int episode, AnimeLanguage language)
        {
            return new RequestBuilder<StreamDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/anime/proxerstreams"), this.ProxerClient)
                .WithGetParameter("id", id.ToString())
                .WithGetParameter("episode", episode.ToString())
                .WithGetParameter("language", language.ToString().ToLowerInvariant());
        }

        /// <summary>
        /// Builds a request that returns all streams (without the Proxerstream) of a specified episode.
        /// 
        /// Api permissions required (class - permission level):
        /// * Anime - Level 2
        /// </summary>
        /// <param name="id">The id of the anime.</param>
        /// <param name="episode">The number of the episode.</param>
        /// <param name="language">The language of the episode.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of streams.</returns>
        public IRequestBuilderWithResult<StreamDataModel[]> GetStreams(int id, int episode, AnimeLanguage language)
        {
            return new RequestBuilder<StreamDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/anime/streams"), this.ProxerClient)
                .WithGetParameter("id", id.ToString())
                .WithGetParameter("episode", episode.ToString())
                .WithGetParameter("language", language.ToString().ToLowerInvariant());
        }
    }
}