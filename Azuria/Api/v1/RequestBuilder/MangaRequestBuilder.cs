using System;
using Azuria.Api.Builder;
using Azuria.Api.v1.DataModels.Manga;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the manga api class.
    /// </summary>
    public class MangaRequestBuilder
    {
        private readonly IProxerClient _client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public MangaRequestBuilder(IProxerClient client)
        {
            this._client = client;
        }

        #region Methods

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns information about a chapter including the pages.
        /// 
        /// Api permissions required:
        /// * Manga - Level 0
        /// </summary>
        /// <param name="id">The id of the manga.</param>
        /// <param name="episode">The number of the chapter.</param>
        /// <param name="language">The language of the chapter.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns the chapter.</returns>
        public IUrlBuilderWithResult<ChapterDataModel> GetChapter(int id, int episode, string language)
        {
            return new UrlBuilder<ChapterDataModel>(
                    new Uri($"{ApiConstants.ApiUrlV1}/manga/chapter"), this._client
                ).WithGetParameter("id", id.ToString())
                .WithGetParameter("episode", episode.ToString())
                .WithGetParameter("language", language);
        }

        #endregion
    }
}