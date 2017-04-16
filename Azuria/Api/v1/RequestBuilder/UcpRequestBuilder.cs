using System;
using Azuria.Api.Builder;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.DataModels.User;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.Enums.User;
using Azuria.Helpers.Extensions;
using HistoryDataModel = Azuria.Api.v1.DataModels.Ucp.HistoryDataModel;
using ToptenDataModel = Azuria.Api.v1.DataModels.Ucp.ToptenDataModel;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the ucp api class.
    /// </summary>
    public class UcpRequestBuilder
    {
        private readonly IProxerClient _client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public UcpRequestBuilder(IProxerClient client)
        {
            this._client = client;
        }

        #region Methods

        /// <summary>
        /// Builds a request that removes an entry from a users topten.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * UCP - Level 1
        /// </summary>
        /// <param name="favouriteId">
        /// The id of the entry that should be removed from the topten (see <see cref="GetTopten" />).
        /// </param>
        /// <returns>An instance of <see cref="ApiRequest" /> that removes an entry from a users topten.</returns>
        /// <seealso cref="GetTopten" />
        public IUrlBuilder DeleteFavourite(int favouriteId)
        {
            return new UrlBuilder(new Uri($"{ApiConstants.ApiUrlV1}/ucp/deletefavorite"), this._client)
                .WithPostParameter("id", favouriteId.ToString())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that deletes a reminder of a user.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * UCP - Level 1
        /// </summary>
        /// <param name="reminderId">The id of the reminder that should be deleted (see <see cref="GetReminder" />).</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that deletes a reminder.</returns>
        /// <seealso cref="GetReminder" />
        public IUrlBuilder DeleteReminder(int reminderId)
        {
            return new UrlBuilder(new Uri($"{ApiConstants.ApiUrlV1}/ucp/deletereminder"), this._client)
                .WithPostParameter("id", reminderId.ToString())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that removes a users comment upvote.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * UCP - Level 1
        /// </summary>
        /// <param name="voteId">The id of the comment upvote that should be removed (see <see cref="GetVotes" />).</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that removes a comment upvote.</returns>
        /// <seealso cref="GetVotes" />
        public IUrlBuilder DeleteVote(int voteId)
        {
            return new UrlBuilder(new Uri($"{ApiConstants.ApiUrlV1}/ucp/deletevote"), this._client)
                .WithPostParameter("id", voteId.ToString())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that returns the history of all watched episodes and read chapters of a
        /// user.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="page">Optional. The index of the page that should be loaded. Default: 0</param>
        /// <param name="limit">Optional. The number of entries that should be loaded per page. Default: 50</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of episodes and chapters.</returns>
        public IUrlBuilderWithResult<HistoryDataModel[]> GetHistory(int page = 0, int limit = 50)
        {
            return new UrlBuilder<HistoryDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/ucp/history"), this._client
                ).WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that returns a list of all anime or manga a user has listed in their ucp.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="category">Optional. The category that should be loaded.</param>
        /// <param name="page">Optional. The index of the page that should be loaded. Default: 0</param>
        /// <param name="limit">Optional. The number of entries that should be returned per page. Default: 100</param>
        /// <param name="search">Optional. The string that all returned entries should contain. Default: ""</param>
        /// <param name="searchStart">Optional. The string that all returned entries should start with. Default: ""</param>
        /// <param name="sort">Optional. The order in which the returned entries should be returned.</param>
        /// <param name="sortDirection">
        /// TODO: Add description here
        /// </param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of anime or manga entries.</returns>
        public IUrlBuilderWithResult<ListDataModel[]> GetList(
            MediaEntryType category = MediaEntryType.Anime, int page = 0, int limit = 100, string search = "",
            string searchStart = "", UserListSort sort = UserListSort.StateName,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            return new UrlBuilder<ListDataModel[]>(new Uri($"{ApiConstants.ApiUrlV1}/ucp/list"), this._client)
                .WithGetParameter("kat", category.ToString().ToLowerInvariant())
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithGetParameter("search", search)
                .WithGetParameter("search_start", searchStart)
                .WithGetParameter("sort", sort.GetDescription() + sortDirection.GetDescription())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that returns the sum of a users watched episodes or read chapters.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="category">Optional. Whether only watched episodes or read chapters should be counted.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns a sum of watched episodes or read chapters.</returns>
        public IUrlBuilderWithResult<int> GetListsum(MediaEntryType category = MediaEntryType.Anime)
        {
            return new UrlBuilder<int>(new Uri($"{ApiConstants.ApiUrlV1}/ucp/listsum"), this._client)
                .WithGetParameter("kat", category.ToString().ToLowerInvariant())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that returns all reminders of a user.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <param name="category">Optional. The category that should be loaded. If null or not given both categories are loaded.</param>
        /// <param name="page">Optional. The index of the page that should be loaded. Default: 0</param>
        /// <param name="limit">Optional. The number of entries that should be loaded per page. Default: 100</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of reminders.</returns>
        public IUrlBuilderWithResult<BookmarkDataModel[]> GetReminder(
            MediaEntryType? category = null, int page = 0, int limit = 100)
        {
            return new UrlBuilder<BookmarkDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/ucp/reminder"), this._client
                ).WithGetParameter("kat", category.ToString().ToLowerInvariant())
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that returns the topten of a user (anime and manga).
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of anime and manga.</returns>
        public IUrlBuilderWithResult<ToptenDataModel[]> GetTopten()
        {
            return new UrlBuilder<ToptenDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/ucp/topten"), this._client
            ).WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that returns all comments a user has voted for.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * UCP - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of comments.</returns>
        public IUrlBuilderWithResult<VoteDataModel[]> GetVotes()
        {
            return new UrlBuilder<VoteDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/ucp/votes"), this._client
            ).WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that sets the number of a users watched episodes/read chapters of an
        /// anime/manga. If the number is set to a value greater or equal of the amount of episodes/chapter the anime/manga has,
        /// the status of the entry will be set to "Finished" as well.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * UCP - Level 1
        /// </summary>
        /// <param name="id">The id of the entry that should be edited (see <see cref="GetList" />).</param>
        /// <param name="progress">The amount of watched episodes/read chapters.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> sets a users progress of an anime/manga.</returns>
        public IUrlBuilder SetCommentState(int id, int progress)
        {
            return new UrlBuilder(new Uri($"{ApiConstants.ApiUrlV1}/ucp/setcommentstate"), this._client)
                .WithPostParameter("id", id.ToString())
                .WithPostParameter("value", progress.ToString())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that adds a reminder for an episode/chapter to a users control-panel.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * UCP - Level 1
        /// </summary>
        /// <param name="entryId">The id of the anime/manga that contains the episode/chapter.</param>
        /// <param name="contentIndex">The episode/chapter number that should be added a reminder for.</param>
        /// <param name="language">The language of the episode/chapter.</param>
        /// <param name="category">
        /// A value indicating whether the reminder is from an anime or manga (I don't know why we need
        /// this).
        /// </param>
        /// <returns>An instance of <see cref="ApiRequest" />.</returns>
        public IUrlBuilder SetReminder(int entryId, int contentIndex, MediaLanguage language, MediaEntryType category)
        {
            return new UrlBuilder(new Uri($"{ApiConstants.ApiUrlV1}/ucp/setreminder"), this._client)
                .WithGetParameter("id", entryId.ToString())
                .WithGetParameter("episode", contentIndex.ToString())
                .WithGetParameter("language", language.ToTypeString())
                .WithGetParameter("kat", category.ToString().ToLowerInvariant())
                .WithLoginCheck();
        }

        #endregion
    }
}