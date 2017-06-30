using System;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.User;
using Azuria.Enums;
using Azuria.Enums.User;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the user api class.
    /// </summary>
    public class UserRequestBuilder : IApiClassRequestBuilder
    {
        /// <summary>
        /// </summary>
        /// <param name="client"></param>
        public UserRequestBuilder(IProxerClient client)
        {
            this.ProxerClient = client;
        }

        /// <inheritdoc />
        public IProxerClient ProxerClient { get; }

        private IRequestBuilderWithResult<HistoryDataModel[]> GetHistory(string page, string limit)
        {
            return new RequestBuilder<HistoryDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/user/history"), this.ProxerClient
                ).WithGetParameter("p", page)
                .WithGetParameter("limit", limit);
        }

        /// <summary>
        /// </summary>
        /// <param name="username"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IRequestBuilderWithResult<HistoryDataModel[]> GetHistory(
            string username, int page = 0, int limit = 100)
        {
            return this.GetHistory(page.ToString(), limit.ToString())
                .WithGetParameter("username", username);
        }

        /// <summary>
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IRequestBuilderWithResult<HistoryDataModel[]> GetHistory(
            int uid, int page = 0, int limit = 100)
        {
            return this.GetHistory(page.ToString(), limit.ToString())
                .WithGetParameter("uid", uid.ToString());
        }

        /// <summary>
        /// Builds a request that...
        /// Api permissions required (class - permission level):
        /// * User - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns...</returns>
        public IRequestBuilderWithResult<UserInfoDataModel> GetInfo()
        {
            return new RequestBuilder<UserInfoDataModel>(
                new Uri($"{ApiConstants.ApiUrlV1}/user/userinfo"), this.ProxerClient
            ).WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that...
        /// Api permissions required (class - permission level):
        /// * User - Level 0
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns...</returns>
        public IRequestBuilderWithResult<UserInfoDataModel> GetInfo(int userId)
        {
            return this.GetInfo()
                .WithGetParameter("uid", userId.ToString())
                .WithLoginCheck(false);
        }

        /// <summary>
        /// Builds a request that...
        /// Api permissions required (class - permission level):
        /// * User - Level 0
        /// </summary>
        /// <param name="username"></param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns...</returns>
        public IRequestBuilderWithResult<UserInfoDataModel> GetInfo(string username)
        {
            return this.GetInfo()
                .WithGetParameter("username", username)
                .WithLoginCheck(false);
        }

        private IRequestBuilderWithResult<CommentDataModel[]> GetLatestComments(
            int page = 0, int limit = 25, MediaEntryType category = MediaEntryType.Anime, int length = 300)
        {
            return new RequestBuilder<CommentDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/user/comments"), this.ProxerClient
                ).WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithGetParameter("kat", category.ToString().ToLowerInvariant())
                .WithGetParameter("length", length.ToString());
        }

        /// <summary>
        /// Builds a request that...
        /// Api permissions required (class - permission level):
        /// * User - Level 0
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="category"></param>
        /// <param name="length"></param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns...</returns>
        public IRequestBuilderWithResult<CommentDataModel[]> GetLatestComments(
            int userId, int page = 0, int limit = 25, MediaEntryType category = MediaEntryType.Anime,
            int length = 300)
        {
            return this.GetLatestComments(page, limit, category, length)
                .WithGetParameter("uid", userId.ToString());
        }

        /// <summary>
        /// Builds a request that...
        /// Api permissions required (class - permission level):
        /// * User - Level 0
        /// </summary>
        /// <param name="username"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="category"></param>
        /// <param name="length"></param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns...</returns>
        public IRequestBuilderWithResult<CommentDataModel[]> GetLatestComments(
            string username, int page = 0, int limit = 25, MediaEntryType category = MediaEntryType.Anime,
            int length = 300)
        {
            return this.GetLatestComments(page, limit, category, length)
                .WithGetParameter("username", username);
        }

        private IRequestBuilderWithResult<ListDataModel[]> GetList(
            MediaEntryType category = MediaEntryType.Anime, int page = 0, int limit = 100, string search = "",
            string searchStart = "", UserListSort sort = UserListSort.StateName,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            return new RequestBuilder<ListDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/user/list"), this.ProxerClient
                ).WithGetParameter("kat", category.ToString().ToLowerInvariant())
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithGetParameter("search", search)
                .WithGetParameter("search_start", searchStart)
                .WithGetParameter("sort", sort.GetDescription() + sortDirection.GetDescription());
        }

        /// <summary>
        /// Builds a request that...
        /// Api permissions required (class - permission level):
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
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns...</returns>
        public IRequestBuilderWithResult<ListDataModel[]> GetList(
            int userId, MediaEntryType category = MediaEntryType.Anime, int page = 0, int limit = 100,
            string search = "", string searchStart = "", UserListSort sort = UserListSort.StateName,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            return this.GetList(category, page, limit, search, searchStart, sort, sortDirection)
                .WithGetParameter("uid", userId.ToString());
        }

        /// <summary>
        /// Builds a request that...
        /// Api permissions required (class - permission level):
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
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns...</returns>
        public IRequestBuilderWithResult<ListDataModel[]> GetList(
            string username, MediaEntryType category = MediaEntryType.Anime, int page = 0, int limit = 100,
            string search = "", string searchStart = "", UserListSort sort = UserListSort.StateName,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            return this.GetList(category, page, limit, search, searchStart, sort, sortDirection)
                .WithGetParameter("username", username);
        }

        private IRequestBuilderWithResult<ToptenDataModel[]> GetTopten(
            MediaEntryType category = MediaEntryType.Anime)
        {
            return new RequestBuilder<ToptenDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/user/topten"), this.ProxerClient
            ).WithGetParameter("kat", category.ToString().ToLowerInvariant());
        }

        /// <summary>
        /// Builds a request that...
        /// Api permissions required (class - permission level):
        /// * User - Level 0
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="category"></param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns...</returns>
        public IRequestBuilderWithResult<ToptenDataModel[]> GetTopten(
            int userId, MediaEntryType category = MediaEntryType.Anime)
        {
            return this.GetTopten(category)
                .WithGetParameter("uid", userId.ToString());
        }

        /// <summary>
        /// Builds a request that...
        /// Api permissions required (class - permission level):
        /// * User - Level 0
        /// </summary>
        /// <param name="username"></param>
        /// <param name="category"></param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns...</returns>
        public IRequestBuilderWithResult<ToptenDataModel[]> GetTopten(
            string username, MediaEntryType category = MediaEntryType.Anime)
        {
            return this.GetTopten(category)
                .WithGetParameter("username", username);
        }

        /// <summary>
        /// Builds a request that...
        /// Api permissions required (class - permission level):
        /// * User - Level 0
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns...</returns>
        public IRequestBuilderWithResult<LoginDataModel> Login(string username, string password)
        {
            return new RequestBuilder<LoginDataModel>(
                    new Uri($"{ApiConstants.ApiUrlV1}/user/login"), this.ProxerClient
                ).WithPostParameter("username", username)
                .WithPostParameter("password", password);
        }

        /// <summary>
        /// Builds a request that...
        /// Api permissions required (class - permission level):
        /// * User - Level 0
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="secretKey"></param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns...</returns>
        public IRequestBuilderWithResult<LoginDataModel> Login(string username, string password, string secretKey)
        {
            return this.Login(username, password)
                .WithPostParameter("secretKey", secretKey);
        }

        /// <summary>
        /// Builds a request that...
        /// Api permissions required (class - permission level):
        /// * User - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilder" /> that returns...</returns>
        public IRequestBuilder Logout()
        {
            return new Requests.Builder.RequestBuilder(
                new Uri($"{ApiConstants.ApiUrlV1}/user/logout"), this.ProxerClient);
        }
    }
}