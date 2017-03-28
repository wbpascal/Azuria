using System;
using Azuria.Api.v1.DataModels.Manga;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the manga api class.
    /// </summary>
    public static class MangaRequestBuilder
    {
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
        /// <param name="user">
        /// Optional. The user that creates the request. If passed and logged in, the user will recieve manga
        /// points. Default: null
        /// </param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns the chapter.</returns>
        public static ApiRequest<ChapterDataModel> GetChapter(int id, int episode, string language,
            IProxerUser user = null)
        {
            return ApiRequest<ChapterDataModel>.Create(new Uri($"{ApiConstants.ApiUrlV1}/manga/chapter"))
                .WithGetParameter("id", id.ToString())
                .WithGetParameter("episode", episode.ToString())
                .WithGetParameter("language", language).WithUser(user);
        }

        #endregion
    }
}