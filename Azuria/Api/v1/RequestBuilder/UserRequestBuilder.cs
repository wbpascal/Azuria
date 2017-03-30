using System;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.User;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the user api class.
    /// </summary>
    public static class UserRequestBuilder
    {
        #region Methods

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<UserInfoDataModel> GetInfo()
        {
            return ApiRequest<UserInfoDataModel>.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/userinfo"));
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<UserInfoDataModel> GetInfo(int userId)
        {
            return ApiRequest<UserInfoDataModel>.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/userinfo"))
                .WithGetParameter("uid", userId.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="username"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<UserInfoDataModel> GetInfo(string username)
        {
            return ApiRequest<UserInfoDataModel>.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/userinfo"))
                .WithGetParameter("username", username);
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
        public static ApiRequest<CommentDataModel[]> GetLatestComments(int userId, int page = 0, int limit = 25,
            string kat = "anime", int length = 300)
        {
            return GetLatestComments(page, limit, kat, length)
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
        public static ApiRequest<CommentDataModel[]> GetLatestComments(string username, int page = 0, int limit = 25,
            string kat = "anime", int length = 300)
        {
            return GetLatestComments(page, limit, kat, length)
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
        public static ApiRequest<CommentDataModel[]> GetLatestComments(int page = 0, int limit = 25,
            string kat = "anime", int length = 300)
        {
            return ApiRequest<CommentDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/comments"))
                .WithGetParameter("p", page.ToString())
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
        public static ApiRequest<ListDataModel[]> GetList(string kat = "anime", int page = 0,
            int limit = 100, string search = "", string searchStart = "", string sort = "")
        {
            return ApiRequest<ListDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/list"))
                .WithGetParameter("kat", kat)
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
        public static ApiRequest<ListDataModel[]> GetList(int userId, string kat = "anime", int page = 0,
            int limit = 100, string search = "", string searchStart = "", string sort = "")
        {
            return GetList(kat, page, limit, search, searchStart, sort)
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
        public static ApiRequest<ListDataModel[]> GetList(string username, string kat = "anime", int page = 0,
            int limit = 100, string search = "", string searchStart = "", string sort = "")
        {
            return GetList(kat, page, limit, search, searchStart, sort)
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
        public static ApiRequest<ToptenDataModel[]> GetTopten(string category = "anime")
        {
            return ApiRequest<ToptenDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/topten"))
                .WithGetParameter("kat", category);
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
        public static ApiRequest<ToptenDataModel[]> GetTopten(int userId, string category = "anime")
        {
            return GetTopten(category)
                .WithGetParameter("uid", userId.ToString());
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
        public static ApiRequest<ToptenDataModel[]> GetTopten(string username, string category = "anime")
        {
            return GetTopten(category)
                .WithGetParameter("username", username);
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
        public static ApiRequest<LoginDataModel> Login(string username, string password)
        {
            return ApiRequest<LoginDataModel>.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/login"))
                .WithPostParameter("username", username)
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
        public static ApiRequest<LoginDataModel> Login(string username, string password, string secretKey)
        {
            return Login(username, username)
                .WithGetParameter("secretKey", secretKey);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest Logout()
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/logout"))
                .WithLoginCheck(true);
        }

        #endregion
    }
}