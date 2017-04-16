using System;
using Azuria.Api.Builder;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.User;
using Azuria.Enums;
using Azuria.Enums.User;
using Azuria.Helpers.Extensions;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the user api class.
    /// </summary>
    public class UserRequestBuilder
    {
        private readonly IProxerClient _client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public UserRequestBuilder(IProxerClient client)
        {
            this._client = client;
        }

        #region Methods

        private IUrlBuilderWithResult<HistoryDataModel[]> GetHistory(string page, string limit)
        {
            return new UrlBuilder<HistoryDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/user/history"), this._client
                ).WithGetParameter("p", page)
                .WithGetParameter("limit", limit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IUrlBuilderWithResult<HistoryDataModel[]> GetHistory(
            string username, int page = 0, int limit = 100)
        {
            return this.GetHistory(page.ToString(), limit.ToString())
                .WithGetParameter("username", username);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IUrlBuilderWithResult<HistoryDataModel[]> GetHistory(
            int uid, int page = 0, int limit = 100)
        {
            return this.GetHistory(page.ToString(), limit.ToString())
                .WithGetParameter("username", uid.ToString());
        }

        /// <summary>
        /// Builds a request that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<UserInfoDataModel> GetInfo()
        {
            return new UrlBuilder<UserInfoDataModel>(
                new Uri($"{ApiConstants.ApiUrlV1}/user/userinfo"), this._client
            ).WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<UserInfoDataModel> GetInfo(int userId)
        {
            return this.GetInfo()
                .WithGetParameter("uid", userId.ToString())
                .WithLoginCheck(false);
        }

        /// <summary>
        /// Builds a request that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="username"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<UserInfoDataModel> GetInfo(string username)
        {
            return this.GetInfo()
                .WithGetParameter("username", username)
                .WithLoginCheck(false);
        }

        private IUrlBuilderWithResult<CommentDataModel[]> GetLatestComments(
            int page = 0, int limit = 25, MediaEntryType category = MediaEntryType.Anime, int length = 300)
        {
            return new UrlBuilder<CommentDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/user/comments"), this._client
                ).WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithGetParameter("kat", category.ToString().ToLowerInvariant())
                .WithGetParameter("length", length.ToString());
        }

        /// <summary>
        /// Builds a request that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="category"></param>
        /// <param name="length"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<CommentDataModel[]> GetLatestComments(
            int userId, int page = 0, int limit = 25, MediaEntryType category = MediaEntryType.Anime,
            int length = 300)
        {
            return this.GetLatestComments(page, limit, category, length)
                .WithGetParameter("uid", userId.ToString());
        }

        /// <summary>
        /// Builds a request that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="username"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="category"></param>
        /// <param name="length"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<CommentDataModel[]> GetLatestComments(
            string username, int page = 0, int limit = 25, MediaEntryType category = MediaEntryType.Anime,
            int length = 300)
        {
            return this.GetLatestComments(page, limit, category, length)
                .WithGetParameter("username", username);
        }

        private IUrlBuilderWithResult<ListDataModel[]> GetList(
            MediaEntryType category = MediaEntryType.Anime, int page = 0, int limit = 100, string search = "",
            string searchStart = "", UserListSort sort = UserListSort.StateName,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            return new UrlBuilder<ListDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/user/list"), this._client
                ).WithGetParameter("kat", category.ToString().ToLowerInvariant())
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithGetParameter("search", search)
                .WithGetParameter("search_start", searchStart)
                .WithGetParameter("sort", sort.GetDescription() + sortDirection.GetDescription());
        }

        /// <summary>
        /// Builds a request that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="category"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="search"></param>
        /// <param name="searchStart"></param>
        /// <param name="sort"></param>
        /// <param name="sortDirection"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<ListDataModel[]> GetList(
            int userId, MediaEntryType category = MediaEntryType.Anime, int page = 0, int limit = 100,
            string search = "", string searchStart = "", UserListSort sort = UserListSort.StateName,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            return this.GetList(category, page, limit, search, searchStart, sort, sortDirection)
                .WithGetParameter("uid", userId.ToString());
        }

        /// <summary>
        /// Builds a request that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="username"></param>
        /// <param name="category"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="search"></param>
        /// <param name="searchStart"></param>
        /// <param name="sort"></param>
        /// <param name="sortDirection"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<ListDataModel[]> GetList(
            string username, MediaEntryType category = MediaEntryType.Anime, int page = 0, int limit = 100,
            string search = "", string searchStart = "", UserListSort sort = UserListSort.StateName,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            return this.GetList(category, page, limit, search, searchStart, sort, sortDirection)
                .WithGetParameter("username", username);
        }

        private IUrlBuilderWithResult<ToptenDataModel[]> GetTopten(
            MediaEntryType category = MediaEntryType.Anime)
        {
            return new UrlBuilder<ToptenDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/user/topten"), this._client
            ).WithGetParameter("kat", category.ToString().ToLowerInvariant());
        }

        /// <summary>
        /// Builds a request that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="category"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<ToptenDataModel[]> GetTopten(
            int userId, MediaEntryType category = MediaEntryType.Anime)
        {
            return this.GetTopten(category)
                .WithGetParameter("uid", userId.ToString());
        }

        /// <summary>
        /// Builds a request that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="username"></param>
        /// <param name="category"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<ToptenDataModel[]> GetTopten(
            string username, MediaEntryType category = MediaEntryType.Anime)
        {
            return this.GetTopten(category)
                .WithGetParameter("username", username);
        }

        /// <summary>
        /// Builds a request that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<LoginDataModel> Login(string username, string password)
        {
            return new UrlBuilder<LoginDataModel>(
                    new Uri($"{ApiConstants.ApiUrlV1}/user/login"), this._client
                ).WithPostParameter("username", username)
                .WithPostParameter("password", password);
        }

        /// <summary>
        /// Builds a request that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="secretKey"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<LoginDataModel> Login(string username, string password, string secretKey)
        {
            return this.Login(username, password)
                .WithPostParameter("secretKey", secretKey);
        }

        /// <summary>
        /// Builds a request that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilder Logout()
        {
            return new UrlBuilder(new Uri($"{ApiConstants.ApiUrlV1}/user/logout"), this._client);
        }

        #endregion
    }
}