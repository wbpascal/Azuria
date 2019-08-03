using System;
using Azuria.Api.v1.DataModels.Anime;
using Azuria.Api.v1.Input.Anime;
using Azuria.Enums.Info;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <inheritdoc />
    /// <summary>
    /// Represents the anime api class.
    /// </summary>
    public class AnimeRequestBuilder : ApiClassRequestBuilderBase
    {
        /// <inheritdoc />
        public AnimeRequestBuilder(IProxerClient proxerClient) : base(proxerClient)
        {
        }

        /// <summary>
        /// Builds a request that returns the link of a specified stream.
        /// Api permissions required (class - permission level):
        /// * Anime - Level 2
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns a link as a string.</returns>
        public IRequestBuilderWithResult<string> GetLink(GetLinkInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<string>(new Uri($"{ApiConstants.ApiUrlV1}/anime/link"), this.ProxerClient)
                .WithGetParameter(input.BuildDictionary());
        }

        /// <summary>
        /// Builds a request that returns all streams (including the Proxerstream) of a specified episode.
        /// Api permissions required (class - permission level):
        /// * Anime - Level 3
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of streams.</returns>
        public IRequestBuilderWithResult<StreamDataModel[]> GetProxerStreams(StreamListInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<StreamDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/anime/proxerstreams"), this.ProxerClient)
                .WithGetParameter(input.BuildDictionary());
        }

        /// <summary>
        /// Builds a request that returns all streams (without the Proxerstream) of a specified episode.
        /// Api permissions required (class - permission level):
        /// * Anime - Level 2
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of streams.</returns>
        public IRequestBuilderWithResult<StreamDataModel[]> GetStreams(StreamListInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<StreamDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/anime/streams"), this.ProxerClient)
                .WithGetParameter(input.BuildDictionary());
        }
    }
}