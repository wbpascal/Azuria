using System;
using Azuria.Api.Builder;
using Azuria.Api.v1.DataModels.Manga;
using Azuria.Enums.Info;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the manga api class.
    /// </summary>
    public class MangaRequestBuilder : IApiClassRequestBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public MangaRequestBuilder(IProxerClient client)
        {
            this.ProxerClient = client;
        }

        /// <inheritdoc />
        public IProxerClient ProxerClient { get; }

        /// <summary>
        /// Builds a request that returns information about a chapter including the pages.
        /// 
        /// Api permissions required (class - permission level):
        /// * Manga - Level 0
        /// </summary>
        /// <param name="id">The id of the manga.</param>
        /// <param name="episode">The number of the chapter.</param>
        /// <param name="language">The language of the chapter.</param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns the chapter.</returns>
        public IRequestBuilderWithResult<ChapterDataModel> GetChapter(int id, int episode, Language language)
        {
            return new RequestBuilder<ChapterDataModel>(
                    new Uri($"{ApiConstants.ApiUrlV1}/manga/chapter"), this.ProxerClient
                ).WithGetParameter("id", id.ToString())
                .WithGetParameter("episode", episode.ToString())
                .WithGetParameter("language", language.ToShortString());
        }
    }
}