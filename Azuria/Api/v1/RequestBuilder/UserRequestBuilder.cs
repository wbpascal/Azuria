using System;
using Azuria.Api.Builder;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.User;

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

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<UserInfoDataModel> GetInfo()
        {
            return new UrlBuilder<UserInfoDataModel>(
                new Uri($"{ApiConstants.ApiUrlV1}/user/userinfo"), this._client
            );
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<UserInfoDataModel> GetInfo(int userId)
        {
            return new UrlBuilder<UserInfoDataModel>(
                new Uri($"{ApiConstants.ApiUrlV1}/user/userinfo"), this._client
            ).WithGetParameter("uid", userId.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="username"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<UserInfoDataModel> GetInfo(string username)
        {
            return new UrlBuilder<UserInfoDataModel>(
                new Uri($"{ApiConstants.ApiUrlV1}/user/userinfo"), this._client
            ).WithGetParameter("username", username);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="kat"></param>
        /// <param name="length"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<CommentDataModel[]> GetLatestComments(
            int userId, int page = 0,
            int limit = 25, string kat = "anime", int length = 300)
        {
            return this.GetLatestComments(page, limit, kat, length)
                       .WithGetParameter("uid", userId.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="username"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="kat"></param>
        /// <param name="length"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<CommentDataModel[]> GetLatestComments(
            string username, int page = 0,
            int limit = 25, string kat = "anime", int length = 300)
        {
            return this.GetLatestComments(page, limit, kat, length)
                       .WithGetParameter("username", username);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="kat"></param>
        /// <param name="length"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<CommentDataModel[]> GetLatestComments(
            int page = 0, int limit = 25,
            string kat = "anime", int length = 300)
        {
            return new UrlBuilder<CommentDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/user/comments"), this._client
                ).WithGetParameter("p", page.ToString())
                 .WithGetParameter("limit", limit.ToString())
                 .WithGetParameter("kat", kat)
                 .WithGetParameter("length", length.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="kat"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="search"></param>
        /// <param name="searchStart"></param>
        /// <param name="sort"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<ListDataModel[]> GetList(
            string kat = "anime", int page = 0,
            int limit = 100, string search = "", string searchStart = "", string sort = "")
        {
            return new UrlBuilder<ListDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/user/list"), this._client
                ).WithGetParameter("kat", kat)
                 .WithGetParameter("p", page.ToString())
                 .WithGetParameter("limit", limit.ToString())
                 .WithGetParameter("search", search)
                 .WithGetParameter("search_start", searchStart)
                 .WithGetParameter("sort", sort);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="kat"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="search"></param>
        /// <param name="searchStart"></param>
        /// <param name="sort"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<ListDataModel[]> GetList(
            int userId, string kat = "anime", int page = 0,
            int limit = 100, string search = "", string searchStart = "", string sort = "")
        {
            return this.GetList(kat, page, limit, search, searchStart, sort)
                       .WithGetParameter("uid", userId.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="username"></param>
        /// <param name="kat"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="search"></param>
        /// <param name="searchStart"></param>
        /// <param name="sort"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<ListDataModel[]> GetList(
            string username, string kat = "anime", int page = 0,
            int limit = 100, string search = "", string searchStart = "", string sort = "")
        {
            return this.GetList(kat, page, limit, search, searchStart, sort)
                       .WithGetParameter("username", username);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="category"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<ToptenDataModel[]> GetTopten(string category = "anime")
        {
            return new UrlBuilder<ToptenDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/user/topten"), this._client
            ).WithGetParameter("kat", category);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="category"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<ToptenDataModel[]> GetTopten(int userId, string category = "anime")
        {
            return this.GetTopten(category).WithGetParameter("uid", userId.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="username"></param>
        /// <param name="category"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public IUrlBuilderWithResult<ToptenDataModel[]> GetTopten(string username, string category = "anime")
        {
            return this.GetTopten(category).WithGetParameter("username", username);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
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
        /// Creates an <see cref="ApiRequest" /> instance that...
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
            return this.Login(username, username).WithGetParameter("secretKey", secretKey);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
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