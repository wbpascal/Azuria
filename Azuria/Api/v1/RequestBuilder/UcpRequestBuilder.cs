using System;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.DataModels.User;
using ToptenDataModel = Azuria.Api.v1.DataModels.Ucp.ToptenDataModel;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// </summary>
    public static class UcpRequestBuilder
    {
        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="favouriteId"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<BookmarkDataModel[]> DeleteFavourite(int favouriteId, Senpai senpai)
        {
            return ApiRequest<BookmarkDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/deletefavorite"))
                .WithCheckLogin(true)
                .WithPostArgument("id", favouriteId.ToString())
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="bookmarkId"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<BookmarkDataModel[]> DeleteReminder(int bookmarkId, Senpai senpai)
        {
            return ApiRequest<BookmarkDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/deletereminder"))
                .WithCheckLogin(true)
                .WithPostArgument("id", bookmarkId.ToString())
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="voteId"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<BookmarkDataModel[]> DeleteVote(int voteId, Senpai senpai)
        {
            return ApiRequest<BookmarkDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/deletevote"))
                .WithCheckLogin(true)
                .WithPostArgument("id", voteId.ToString())
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<HistoryDataModel[]> GetHistory(int page, int limit, Senpai senpai)
        {
            return ApiRequest<HistoryDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/history"))
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="kat"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="search"></param>
        /// <param name="searchStart"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static ApiRequest<ListDataModel[]> GetList(Senpai senpai, string kat = "anime", int page = 0,
            int limit = 100, string search = "", string searchStart = "", string sort = "stateNameAsc")
        {
            return ApiRequest<ListDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/list"))
                .WithGetParameter("kat", kat)
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithGetParameter("search", search)
                .WithGetParameter("search_start", searchStart)
                .WithGetParameter("sort", sort)
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="kat"></param>
        /// <returns></returns>
        public static ApiRequest<int> GetListsum(Senpai senpai, string kat = "anime")
        {
            return ApiRequest<int>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/listsum"))
                .WithGetParameter("kat", kat)
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="kat"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<BookmarkDataModel[]> GetReminder(string kat, int page, int limit,
            Senpai senpai)
        {
            return ApiRequest<BookmarkDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/reminder"))
                .WithGetParameter("kat", kat)
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<ToptenDataModel[]> GetTopten(Senpai senpai)
        {
            return ApiRequest<ToptenDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/topten"))
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<VoteDataModel[]> GetVotes(Senpai senpai)
        {
            return ApiRequest<VoteDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/votes"))
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="contentIndex"></param>
        /// <param name="language"></param>
        /// <param name="kat"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest SetBookmark(int entryId, int contentIndex, string language, string kat,
            Senpai senpai)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/setreminder"))
                .WithGetParameter("id", entryId.ToString())
                .WithGetParameter("episode", contentIndex.ToString())
                .WithGetParameter("language", language)
                .WithGetParameter("kat", kat)
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="progress"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest SetProgress(int commentId, int progress, Senpai senpai)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/setcommentstate"))
                .WithCheckLogin(true)
                .WithPostArgument("id", commentId.ToString())
                .WithPostArgument("value", progress.ToString())
                .WithSenpai(senpai);
        }

        #endregion
    }
}