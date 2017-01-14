using System;
using Azuria.Api.v1.DataModels.Manga;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// </summary>
    public static class MangaRequestBuilder
    {
        #region Methods

        /// <summary>
        /// Creates an <see cref="ApiRequest"/> instance that...
        /// 
        /// Api permissions required:
        ///  * Manga - Level 0
        /// </summary>
        /// <param name="id"></param>
        /// <param name="episode"></param>
        /// <param name="language"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest"/> that returns...</returns>
        public static ApiRequest<ChapterDataModel> GetChapter(int id, int episode, string language,
            Senpai senpai = null)
        {
            return ApiRequest<ChapterDataModel>.Create(
                    new Uri($"{ApiConstants.ApiUrlV1}/manga/chapter?id={id}&episode={episode}&language={language}"))
                .WithSenpai(senpai);
        }

        #endregion
    }
}