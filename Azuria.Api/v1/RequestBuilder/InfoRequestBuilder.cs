using System;
using Azuria.Api.Enums.Info;
using Azuria.Api.v1.Converters.Info;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.Info;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the info api class.
    /// </summary>
    public static class InfoRequestBuilder
    {
        #region Methods

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns all comments of an anime or manga (with more than 300
        /// characters).
        /// 
        /// Api permissions required:
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <param name="page">Optional. The index of the page that will be loaded. Default: 0</param>
        /// <param name="limit">Optional. The amount of comments that will be returned per page. Default: 25</param>
        /// <param name="sort">
        /// Optional. The order in which the returned array will be sorted. Set it to "rating" to return the top
        /// rated comments first, otherwise the newest comments will be returned first. Default: ""
        /// </param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of comments.</returns>
        public static ApiRequest<CommentDataModel[]> GetComments(int entryId, int page = 0, int limit = 25,
            string sort = "")
        {
            return ApiRequest<CommentDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/comments"))
                .WithGetParameter("id", entryId.ToString())
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithGetParameter("sort", sort);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns the core information of an anime or manga.
        /// 
        /// Api permissions required:
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the Anime or Manga.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns the core information.</returns>
        public static ApiRequest<EntryDataModel> GetEntry(int entryId)
        {
            return ApiRequest<EntryDataModel>.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/entry"))
                .WithGetParameter("id", entryId.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns all tags of an anime or manga.
        /// 
        /// Api permissions required:
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of tags.</returns>
        public static ApiRequest<MediaTagDataModel[]> GetEntryTags(int entryId)
        {
            return ApiRequest<MediaTagDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/entrytags"))
                .WithGetParameter("id", entryId.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns all informations of an anime or manga.
        /// 
        /// **Warning!:**
        /// The returned object creates a heavy load on the server if it is used in a request!
        /// Be sure to only use it if you are certain that you need all returned informations!
        /// 
        /// Api permissions required:
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns the informations.</returns>
        public static ApiRequest<FullEntryDataModel> GetFullEntry(int entryId)
        {
            return ApiRequest<FullEntryDataModel>.Create(new Uri(
                $"{ApiConstants.ApiUrlV1}/info/fullentry?id={entryId}"));
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns a boolean indicating if the anime or manga requires an 18+
        /// age check.
        /// 
        /// Api permissions required:
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns a boolean.</returns>
        public static ApiRequest<bool> GetGate(int entryId)
        {
            return ApiRequest<bool>.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/gate"))
                .WithGetParameter("id", entryId.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns informations about the translators of an anime or manga.
        /// 
        /// Api permissions required:
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of translators.</returns>
        public static ApiRequest<TranslatorDataModel[]> GetGroups(int entryId)
        {
            return ApiRequest<TranslatorDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/groups"))
                .WithGetParameter("id", entryId.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns all languages an anime or manga is available in.
        /// 
        /// Api permissions required:
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of languages.</returns>
        public static ApiRequest<MediaLanguage[]> GetLanguage(int entryId)
        {
            return ApiRequest<MediaLanguage[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/lang"))
                .WithGetParameter("id", entryId.ToString())
                .WithCustomDataConverter(new LanguageCollectionConverter());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns all episodes or chapters of an anime or manga.
        /// 
        /// Api permissions required:
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <param name="page">Optional. The index of the page that will be loaded. Default: 0</param>
        /// <param name="limit">Optional. The amount of episodes or chapters that will be returned. Default: 50</param>
        /// <returns>
        /// An instance of <see cref="ApiRequest" /> that returns an object containing all chapter/episodes and
        /// information about the returned list.
        /// </returns>
        public static ApiRequest<ListInfoDataModel> GetListInfo(int entryId, int page = 0, int limit = 50)
        {
            return ApiRequest<ListInfoDataModel>.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/listinfo"))
                .WithGetParameter("id", entryId.ToString())
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns all names and synonymous of an anime or manga.
        /// 
        /// Api permissions required:
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <returns>
        /// An instance of <see cref="ApiRequest" /> that returns an array of object containing the names and some
        /// additional informations.
        /// </returns>
        public static ApiRequest<NameDataModel[]> GetName(int entryId)
        {
            return ApiRequest<NameDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/names"))
                .WithGetParameter("id", entryId.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns returns all organisations that were involved with creating
        /// or publishing the an anime or manga.
        /// 
        /// Api permissions required:
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of organisations.</returns>
        public static ApiRequest<PublisherDataModel[]> GetPublisher(int entryId)
        {
            return ApiRequest<PublisherDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/publisher"))
                .WithGetParameter("id", entryId.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns all relations of an anime or manga.
        /// 
        /// Api permissions required:
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of relations.</returns>
        public static ApiRequest<RelationDataModel[]> GetRelations(int entryId)
        {
            return ApiRequest<RelationDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/relations"))
                .WithGetParameter("id", entryId.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns the seasons an anime or manga aired in.
        /// 
        /// Api permissions required:
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of seasons.</returns>
        public static ApiRequest<SeasonDataModel[]> GetSeason(int entryId)
        {
            return ApiRequest<SeasonDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/season"))
                .WithGetParameter("id", entryId.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that adds an anime or manga to a list of a logged in user.
        /// 
        /// Api permissions required:
        /// * Info - Level 1
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <param name="type">The list to which the anime or manga will be added. Possible values: "note", "favor", "finish"</param>
        /// <param name="user">The logged in user.</param>
        /// <returns>An instance of <see cref="ApiRequest" />.</returns>
        public static ApiRequest SetUserInfo(int entryId, string type, IProxerUser user)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/setuserinfo"))
                .WithLoginCheck(true)
                .WithPostParameter("id", entryId.ToString())
                .WithPostParameter("type", type)
                .WithUser(user);
        }

        #endregion
    }
}