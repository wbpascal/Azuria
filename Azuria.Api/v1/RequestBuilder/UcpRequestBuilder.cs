using System;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.DataModels.User;
using ToptenDataModel = Azuria.Api.v1.DataModels.Ucp.ToptenDataModel;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the ucp api class.
    /// </summary>
    public static class UcpRequestBuilder
    {
        #region Methods

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that removes an entry from a users topten.
        /// 
        /// Api permissions required:
        /// * UCP - Level 1
        /// </summary>
        /// <param name="favouriteId">The id of the entry that should be removed from the topten (see <see cref="GetTopten" />).</param>
        /// <param name="user">The user of which the entry should be removed from the topten. Must be logged in.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that removes an entry from a users topten.</returns>
        /// <seealso cref="GetTopten" />
        public static ApiRequest DeleteFavourite(int favouriteId, IProxerUser user)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/deletefavorite"))
                .WithLoginCheck(true)
                .WithPostParameter("id", favouriteId.ToString())
                .WithUser(user);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that deletes a reminder of a user.
        /// 
        /// Api permissions required:
        /// * UCP - Level 1
        /// </summary>
        /// <param name="reminderId">The id of the reminder that should be deleted (see <see cref="GetReminder" />).</param>
        /// <param name="user">The user of which the reminder will be deleted. Must be logged in.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that deletes a reminder.</returns>
        /// <seealso cref="GetReminder" />
        public static ApiRequest DeleteReminder(int reminderId, IProxerUser user)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/deletereminder"))
                .WithLoginCheck(true)
                .WithPostParameter("id", reminderId.ToString())
                .WithUser(user);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that removes a users comment upvote.
        /// 
        /// Api permissions required:
        /// * UCP - Level 1
        /// </summary>
        /// <param name="voteId">The id of the comment upvote that should be removed (see <see cref="GetVotes" />).</param>
        /// <param name="user">The user of which the comment upvote should be removed. Must be logged in.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that removes a comment upvote.</returns>
        /// <seealso cref="GetVotes" />
        public static ApiRequest DeleteVote(int voteId, IProxerUser user)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/deletevote"))
                .WithLoginCheck(true)
                .WithPostParameter("id", voteId.ToString())
                .WithUser(user);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns the history of all watched episodes and read chapters of a
        /// user.
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="user">The user of which the history will be returned. Must be logged in.</param>
        /// <param name="page">Optional. The index of the page that should be loaded. Default: 0</param>
        /// <param name="limit">Optional. The number of entries that should be loaded per page. Default: 50</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of episodes and chapters.</returns>
        public static ApiRequest<HistoryDataModel[]> GetHistory(IProxerUser user, int page = 0, int limit = 50)
        {
            return ApiRequest<HistoryDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/history"))
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithLoginCheck(true).WithUser(user);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns a list of all anime or manga a user has listed in their ucp.
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="user">The user of which the anime or manga are returned. Must be logged in.</param>
        /// <param name="kat">Optional. The category that should be loaded. Possible values: "anime", "manga". Default: "anime"</param>
        /// <param name="page">Optional. The index of the page that should be loaded. Default: 0</param>
        /// <param name="limit">Optional. The number of entries that should be returned per page. Default: 100</param>
        /// <param name="search">Optional. The string that all returned entries should contain. Default: ""</param>
        /// <param name="searchStart">Optional. The string that all returned entries should start with. Default: ""</param>
        /// <param name="sort">
        /// Optional. The order in which the returned entries should be returned. Possible values:
        /// * "nameASC": Order by entry name ascending.
        /// * "nameDESC": Order by entry name descending.
        /// * "stateNameASC": Order first by status of the entry and then by entry name ascending.
        /// * "stateNameDESC": Order first by status of the entry and then by entry name descending.
        /// * "changeDateASC": Order by last changed ascending.
        /// * "changeDateDESC": Order by last changed descending.
        /// * "stateChangeDateASC": Order first by status of the entry and then by last changed ascending.
        /// * "stateChangeDateDESC": Order first by status of the entry and then by last changed descending.
        /// 
        /// Default: "stateNameASC"
        /// </param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of anime or manga entries.</returns>
        public static ApiRequest<ListDataModel[]> GetList(IProxerUser user, string kat = "anime", int page = 0,
            int limit = 100, string search = "", string searchStart = "", string sort = "stateNameAsc")
        {
            return ApiRequest<ListDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/list"))
                .WithGetParameter("kat", kat)
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithGetParameter("search", search)
                .WithGetParameter("search_start", searchStart)
                .WithGetParameter("sort", sort)
                .WithLoginCheck(true).WithUser(user);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns the sum of a users watched episodes or read chapters.
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="user">The user of which the sum will be returned. Must be logged in.</param>
        /// <param name="kat">
        /// Optional. Whether only watched episodes or read chapters should be counted. Possible values: "anime",
        /// "manga". Default: "anime"
        /// </param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns a sum of watched episodes or read chapters.</returns>
        public static ApiRequest<int> GetListsum(IProxerUser user, string kat = "anime")
        {
            return ApiRequest<int>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/listsum"))
                .WithGetParameter("kat", kat)
                .WithLoginCheck(true).WithUser(user);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns all reminders of a user.
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="user">The user of which the reminders should be returned. Must be logged in.</param>
        /// <param name="kat">
        /// Optional. The category that should be loaded. If empty or not given both categories are loaded.
        /// Possible values: "anime", "manga". Default: ""
        /// </param>
        /// <param name="page">Optional. The index of the page that should be loaded. Default: 0</param>
        /// <param name="limit">Optional. The number of entries that should be loaded per page. Default: 100</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of reminders.</returns>
        public static ApiRequest<BookmarkDataModel[]> GetReminder(IProxerUser user, string kat = "", int page = 0,
            int limit = 100)
        {
            return ApiRequest<BookmarkDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/reminder"))
                .WithGetParameter("kat", kat)
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithLoginCheck(true).WithUser(user);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns the topten of a user (anime and manga).
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="user">The user of which the topten will be returned. Must be logged in.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of anime and manga.</returns>
        public static ApiRequest<ToptenDataModel[]> GetTopten(IProxerUser user)
        {
            return ApiRequest<ToptenDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/topten"))
                .WithLoginCheck(true).WithUser(user);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns all comments a user has voted for.
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="user">The user of which the comment votes will be returned. Must be logged in.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of comments.</returns>
        public static ApiRequest<VoteDataModel[]> GetVotes(IProxerUser user)
        {
            return ApiRequest<VoteDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/votes"))
                .WithLoginCheck(true).WithUser(user);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that sets the number of a users watched episodes/read chapters of an
        /// anime/manga. If the number is set to a value greater or equal of the amount of episodes/chapter the anime/manga has,
        /// the status of the entry will be set to "Finished" aswell.
        /// 
        /// Api permissions required:
        /// * UCP - Level 1
        /// </summary>
        /// <param name="id">The id of the entry that should be edited (see <see cref="GetList" />).</param>
        /// <param name="progress">The amount of watched episodes/read chapters.</param>
        /// <param name="user">The user of which the progress will be edited. Must be logged in.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> sets a users progress of an anime/manga.</returns>
        public static ApiRequest SetCommentState(int id, int progress, IProxerUser user)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/setcommentstate"))
                .WithLoginCheck(true)
                .WithPostParameter("id", id.ToString())
                .WithPostParameter("value", progress.ToString())
                .WithUser(user);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that adds a reminder for an episode/chapter to a users control-panel.
        /// 
        /// Api permissions required:
        /// * UCP - Level 1
        /// </summary>
        /// <param name="entryId">The id of the anime/manga that contains the episode/chapter.</param>
        /// <param name="contentIndex">The episode/chapter number that should be added a reminder for.</param>
        /// <param name="language">
        /// The language of the episode/chapter. Possible values: "gersub" (anime), "gerdub" (anime),
        /// "engsub" (anime), "engdub" (anime), "de" (manga), "en" (manga)
        /// </param>
        /// <param name="kat">
        /// A string indicating whether the reminder is from an anime or manga (I don't know why we need this).
        /// Possible values: "anime", "manga"
        /// </param>
        /// <param name="user">The user that creates the reminder. Must be logged in.</param>
        /// <returns>An instance of <see cref="ApiRequest" />.</returns>
        public static ApiRequest SetReminder(int entryId, int contentIndex, string language, string kat,
            IProxerUser user)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/ucp/setreminder"))
                .WithGetParameter("id", entryId.ToString())
                .WithGetParameter("episode", contentIndex.ToString())
                .WithGetParameter("language", language)
                .WithGetParameter("kat", kat)
                .WithLoginCheck(true)
                .WithUser(user);
        }

        #endregion
    }
}