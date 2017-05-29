using System;
using Azuria.Api.Builder;
using Azuria.Api.v1.Converters.Info;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums.Info;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the info api class.
    /// </summary>
    public class InfoRequestBuilder
    {
        private readonly IProxerClient _client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public InfoRequestBuilder(IProxerClient client)
        {
            this._client = client;
        }

        #region Methods

        /// <summary>
        /// Builds a request that returns all comments of an anime or manga
        /// (with more than 300 characters).
        /// 
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <param name="page">Optional. The index of the page that will be loaded. Default: 0</param>
        /// <param name="limit">
        /// Optional. The amount of comments that will be returned per page. Default: 25
        /// </param>
        /// <param name="sort">
        /// Optional. The order in which the returned array will be sorted. Set it to "rating" to return the
        /// top rated comments first, otherwise the newest comments will be returned first. Default: ""
        /// </param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of comments.</returns>
        public IRequestBuilderWithResult<CommentDataModel[]> GetComments(
            int entryId, int page = 0, int limit = 25, CommentSort sort = CommentSort.Newest)
        {
            return new RequestBuilder<CommentDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/info/comments"), this._client
                ).WithGetParameter("id", entryId.ToString())
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithGetParameter("sort", sort.ToString().ToLowerInvariant());
        }

        /// <summary>
        /// Builds a request that returns the core information of an anime or
        /// manga.
        /// 
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the Anime or Manga.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns the core information.</returns>
        public IRequestBuilderWithResult<EntryDataModel> GetEntry(int entryId)
        {
            return new RequestBuilder<EntryDataModel>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/entry"), this._client
            ).WithGetParameter("id", entryId.ToString());
        }

        /// <summary>
        /// Builds a request that returns all tags of an anime or manga.
        /// 
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of tags.</returns>
        public IRequestBuilderWithResult<MediaTagDataModel[]> GetEntryTags(int entryId)
        {
            return new RequestBuilder<MediaTagDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/entrytags"), this._client
            ).WithGetParameter("id", entryId.ToString());
        }

        /// <summary>
        /// Builds a request that returns all informations of an anime or manga.
        /// 
        /// **Warning!:**
        /// The returned object creates a heavy load on the server if it is used in a request!
        /// Be sure to only use it if you are certain that you need all returned informations!
        /// 
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns the informations.</returns>
        public IRequestBuilderWithResult<FullEntryDataModel> GetFullEntry(int entryId)
        {
            return new RequestBuilder<FullEntryDataModel>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/fullentry?id={entryId}"), this._client
            );
        }

        /// <summary>
        /// Builds a request that returns a boolean indicating if the anime or
        /// manga requires an 18+ age check.
        /// 
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns a boolean.</returns>
        public IRequestBuilderWithResult<bool> GetGate(int entryId)
        {
            return new RequestBuilder<bool>(new Uri($"{ApiConstants.ApiUrlV1}/info/gate"), this._client)
                .WithGetParameter("id", entryId.ToString());
        }

        /// <summary>
        /// Builds a request that returns informations about the translators of
        /// an anime or manga.
        /// 
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of translators.</returns>
        public IRequestBuilderWithResult<TranslatorDataModel[]> GetGroups(int entryId)
        {
            return new RequestBuilder<TranslatorDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/groups"), this._client
            ).WithGetParameter("id", entryId.ToString());
        }

        /// <summary>
        /// Builds a request that returns information about a company.
        /// 
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <param name="id">The id of the company.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns information about a company.</returns>
        public IRequestBuilderWithResult<TranslatorDataModel[]> GetIndustry(int id)
        {
            return new RequestBuilder<TranslatorDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/industry"), this._client
            ).WithGetParameter("id", id.ToString());
        }

        /// <summary>
        /// Builds a request that returns all languages an anime or manga is
        /// available in.
        /// 
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of languages.</returns>
        public IRequestBuilderWithResult<MediaLanguage[]> GetLanguage(int entryId)
        {
            return new RequestBuilder<MediaLanguage[]>(new Uri($"{ApiConstants.ApiUrlV1}/info/lang"), this._client)
                .WithGetParameter("id", entryId.ToString())
                .WithCustomDataConverter(new LanguageCollectionConverter());
        }

        /// <summary>
        /// Builds a request that returns all episodes or chapters of an anime
        /// or manga.
        /// 
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <param name="page">Optional. The index of the page that will be loaded. Default: 0</param>
        /// <param name="limit">
        /// Optional. The amount of episodes or chapters that will be returned. Default: 50
        /// </param>
        /// <returns>
        /// An instance of <see cref="ApiRequest" /> that returns an object containing all chapter/episodes
        /// and information about the returned list.
        /// </returns>
        public IRequestBuilderWithResult<ListInfoDataModel> GetListInfo(int entryId, int page = 0, int limit = 50)
        {
            return new RequestBuilder<ListInfoDataModel>(
                    new Uri($"{ApiConstants.ApiUrlV1}/info/listinfo"), this._client
                ).WithGetParameter("id", entryId.ToString())
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString());
        }

        /// <summary>
        /// Builds a request that returns all names and synonymous of an anime
        /// or manga.
        /// 
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <returns>
        /// An instance of <see cref="ApiRequest" /> that returns an array of object containing the names
        /// and some additional informations.
        /// </returns>
        public IRequestBuilderWithResult<NameDataModel[]> GetName(int entryId)
        {
            return new RequestBuilder<NameDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/names"), this._client
            ).WithGetParameter("id", entryId.ToString());
        }

        /// <summary>
        /// Builds a request that returns returns all organisations that were
        /// involved with creating or publishing the an anime or manga.
        /// 
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <returns>
        /// An instance of <see cref="ApiRequest" /> that returns an array of organisations.
        /// </returns>
        public IRequestBuilderWithResult<PublisherDataModel[]> GetPublisher(int entryId)
        {
            return new RequestBuilder<PublisherDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/publisher"), this._client
            ).WithGetParameter("id", entryId.ToString());
        }

        /// <summary>
        /// Builds a request that returns all relations of an anime or manga.
        /// 
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of relations.</returns>
        public IRequestBuilderWithResult<RelationDataModel[]> GetRelations(int entryId)
        {
            return new RequestBuilder<RelationDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/relations"), this._client
            ).WithGetParameter("id", entryId.ToString());
        }

        /// <summary>
        /// Builds a request that returns the seasons an anime or manga aired in.
        /// 
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of seasons.</returns>
        public IRequestBuilderWithResult<SeasonDataModel[]> GetSeason(int entryId)
        {
            return new RequestBuilder<SeasonDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/season"), this._client
            ).WithGetParameter("id", entryId.ToString());
        }

        /// <summary>
        /// Builds a request that returns information about a translator group.
        /// 
        /// Api permissions required (class - permission level):
        /// * Info - Level 0
        /// </summary>
        /// <param name="id">The id of the translator group.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns information about a translator group.</returns>
        public IRequestBuilderWithResult<TranslatorDataModel[]> GetTranslatorGroup(int id)
        {
            return new RequestBuilder<TranslatorDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/info/translatorgroup"), this._client
            ).WithGetParameter("id", id.ToString());
        }

        /// <summary>
        /// Builds a request that adds an anime or manga to a list of a logged
        /// in user.
        /// Requires authentication.
        /// 
        /// Api permissions required (class - permission level):
        /// * Info - Level 1
        /// </summary>
        /// <param name="entryId">The id of the anime or manga.</param>
        /// <param name="list">
        /// The list to which the anime or manga will be added. Possible values: "note", "favor", "finish"
        /// </param>
        /// <returns>An instance of <see cref="ApiRequest" />.</returns>
        public IRequestBuilder SetUserInfo(int entryId, UserList list)
        {
            return new Requests.Builder.RequestBuilder(new Uri($"{ApiConstants.ApiUrlV1}/info/setuserinfo"), this._client)
                .WithPostParameter("id", entryId.ToString())
                .WithPostParameter("type", list.ToTypeString())
                .WithLoginCheck();
        }

        #endregion
    }
}