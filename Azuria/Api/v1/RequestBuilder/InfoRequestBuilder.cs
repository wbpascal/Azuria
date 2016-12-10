using System;
using Azuria.Api.v1.Converters.Info;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Media.Properties;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// </summary>
    public static class InfoRequestBuilder
    {
        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static ApiRequest<CommentDataModel[]> GetComments(int entryId, int page, int limit, string sort)
        {
            return ApiRequest<CommentDataModel[]>.Create(
                new Uri($"{ApiConstants.ApiUrlV1}/info/comments?id={entryId}&p={page}&limit={limit}&sort={sort}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<EntryDataModel> GetEntry(int entryId)
        {
            return ApiRequest<EntryDataModel>.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/entry?id={entryId}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<MediaTagDataModel[]> GetEntryTags(int entryId)
        {
            return
                ApiRequest<MediaTagDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/entrytags?id={entryId}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<FullEntryDataModel> GetFullEntry(int entryId)
        {
            return ApiRequest<FullEntryDataModel>.Create(new Uri(
                $"{ApiConstants.ApiUrlV1}/info/fullentry?id={entryId}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<bool> GetGate(int entryId)
        {
            return ApiRequest<bool>.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/gate?id={entryId}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<TranslatorDataModel[]> GetGroups(int entryId)
        {
            return ApiRequest<TranslatorDataModel[]>.Create(
                new Uri($"{ApiConstants.ApiUrlV1}/info/groups?id={entryId}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<MediaLanguage[]> GetLanguage(int entryId)
        {
            return ApiRequest<MediaLanguage[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/lang?id={entryId}"))
                .WithCustomDataConverter(new LanguageCollectionConverter());
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static ApiRequest<ListInfoDataModel> GetListInfo(int entryId, int page, int limit)
        {
            return ApiRequest<ListInfoDataModel>.Create(
                new Uri($"{ApiConstants.ApiUrlV1}/info/listinfo?id={entryId}&p={page}&limit={limit}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<NameDataModel[]> GetName(int entryId)
        {
            return ApiRequest<NameDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/names?id={entryId}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<PublisherDataModel[]> GetPublisher(int entryId)
        {
            return
                ApiRequest<PublisherDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/publisher?id={entryId}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<RelationDataModel[]> GetRelations(int entryId)
        {
            return
                ApiRequest<RelationDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/relations?id={entryId}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<SeasonDataModel[]> GetSeason(int entryId)
        {
            return ApiRequest<SeasonDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/season?id={entryId}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="type"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest SetUserInfo(int entryId, string type, Senpai senpai)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/info/setuserinfo"))
                .WithCheckLogin(true)
                .WithPostArgument("id", entryId.ToString())
                .WithPostArgument("type", type)
                .WithSenpai(senpai);
        }

        #endregion
    }
}