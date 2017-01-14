using System;
using Azuria.Api.v1.DataModels.Ucp;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// </summary>
    public static class UcpRequestBuilder
    {
        #region Methods

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="favouriteId"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<BookmarkDataModel[]> DeleteFavourite(int favouriteId, Senpai senpai)
        {
            return ApiRequest<BookmarkDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/deletefavorite"))
                .WithCheckLogin(true)
                .WithPostArgument("id", favouriteId.ToString())
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="bookmarkId"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<BookmarkDataModel[]> DeleteReminder(int bookmarkId, Senpai senpai)
        {
            return ApiRequest<BookmarkDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/deletereminder"))
                .WithCheckLogin(true)
                .WithPostArgument("id", bookmarkId.ToString())
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="voteId"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<BookmarkDataModel[]> DeleteVote(int voteId, Senpai senpai)
        {
            return ApiRequest<BookmarkDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/deletevote"))
                .WithCheckLogin(true)
                .WithPostArgument("id", voteId.ToString())
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<HistoryDataModel[]> GetHistory(int page, int limit, Senpai senpai)
        {
            return ApiRequest<HistoryDataModel[]>.Create(
                    new Uri($"{ApiConstants.ApiUrlV1}/ucp/history?p={page}&limit={limit}"))
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="kat"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<int> GetListsum(Senpai senpai, string kat = "anime")
        {
            return ApiRequest<int>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/listsum?kat={kat}"))
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="kat"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<BookmarkDataModel[]> GetReminder(string kat, int page, int limit,
            Senpai senpai)
        {
            return ApiRequest<BookmarkDataModel[]>.Create(
                    new Uri($"{ApiConstants.ApiUrlV1}/ucp/reminder?kat={kat}&p={page}&limit={limit}"))
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<ToptenDataModel[]> GetTopten(Senpai senpai)
        {
            return ApiRequest<ToptenDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/topten"))
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<VoteDataModel[]> GetVotes(Senpai senpai)
        {
            return ApiRequest<VoteDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/votes"))
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="contentIndex"></param>
        /// <param name="language"></param>
        /// <param name="kat"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest SetBookmark(int entryId, int contentIndex, string language, string kat,
            Senpai senpai)
        {
            return ApiRequest.Create(new Uri(
                    $"{ApiConstants.ApiUrlV1}/ucp/setreminder?id={entryId}&episode={contentIndex}&language={language}&kat={kat}"))
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="progress"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
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