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
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<UserInfoDataModel> GetInfo(Senpai senpai)
        {
            return ApiRequest<UserInfoDataModel>.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/userinfo"))
                .WithSenpai(senpai);
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
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<CommentDataModel[]> GetLatestComments(int userId, int page = 0, int limit = 25,
            string kat = "anime", int length = 300, Senpai senpai = null)
        {
            return GetLatestComments(senpai, page, limit, kat, length)
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
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<CommentDataModel[]> GetLatestComments(string username, int page = 0, int limit = 25,
            string kat = "anime", int length = 300, Senpai senpai = null)
        {
            return GetLatestComments(senpai, page, limit, kat, length)
                .WithGetParameter("username", username);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="kat"></param>
        /// <param name="length"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<CommentDataModel[]> GetLatestComments(Senpai senpai, int page = 0, int limit = 25,
            string kat = "anime", int length = 300)
        {
            return ApiRequest<CommentDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/comments"))
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithGetParameter("kat", kat)
                .WithGetParameter("length", length.ToString())
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="kat"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="search"></param>
        /// <param name="searchStart"></param>
        /// <param name="sort"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<ListDataModel[]> GetList(Senpai senpai, string kat = "anime", int page = 0,
            int limit = 100, string search = "", string searchStart = "", string sort = "")
        {
            return ApiRequest<ListDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/list"))
                .WithGetParameter("kat", kat)
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithGetParameter("search", search)
                .WithGetParameter("search_start", searchStart)
                .WithGetParameter("sort", sort)
                .WithSenpai(senpai);
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
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<ListDataModel[]> GetList(int userId, string kat = "anime", int page = 0,
            int limit = 100, string search = "", string searchStart = "", string sort = "", Senpai senpai = null)
        {
            return GetList(senpai, kat, page, limit, search, searchStart, sort)
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
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<ListDataModel[]> GetList(string username, string kat = "anime", int page = 0,
            int limit = 100, string search = "", string searchStart = "", string sort = "", Senpai senpai = null)
        {
            return GetList(senpai, kat, page, limit, search, searchStart, sort)
                .WithGetParameter("username", username);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="category"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<ToptenDataModel[]> GetTopten(Senpai senpai, string category = "anime")
        {
            return ApiRequest<ToptenDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/topten"))
                .WithGetParameter("kat", category)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="senpai"></param>
        /// <param name="category"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<ToptenDataModel[]> GetTopten(int userId, string category = "anime",
            Senpai senpai = null)
        {
            return GetTopten(senpai, category)
                .WithGetParameter("uid", userId.ToString());
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="username"></param>
        /// <param name="senpai"></param>
        /// <param name="category"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<ToptenDataModel[]> GetTopten(string username, string category = "anime",
            Senpai senpai = null)
        {
            return GetTopten(senpai, category)
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
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<LoginDataModel> Login(string username, string password, Senpai senpai = null)
        {
            return ApiRequest<LoginDataModel>.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/login"))
                .WithPostArgument("username", username)
                .WithPostArgument("password", password)
                .WithSenpai(senpai);
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
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<LoginDataModel> Login(string username, string password, string secretKey,
            Senpai senpai = null)
        {
            return Login(username, username, senpai)
                .WithGetParameter("secretKey", secretKey);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * User - Level 0
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest Logout(Senpai senpai)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/logout"))
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        #endregion
    }
}