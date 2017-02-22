using System;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.User;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// </summary>
    public static class UserRequestBuilder
    {
        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<UserInfoDataModel> GetInfo(Senpai senpai)
        {
            return ApiRequest<UserInfoDataModel>.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/userinfo"))
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static ApiRequest<UserInfoDataModel> GetInfo(int userId)
        {
            return ApiRequest<UserInfoDataModel>.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/userinfo"))
                .WithGetParameter("uid", userId.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static ApiRequest<UserInfoDataModel> GetInfo(string username)
        {
            return ApiRequest<UserInfoDataModel>.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/userinfo"))
                .WithGetParameter("username", username);
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="kat"></param>
        /// <param name="length"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<CommentDataModel[]> GetLatestComments(int userId, int page, int limit, string kat,
            int length, Senpai senpai = null)
        {
            return ApiRequest<CommentDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/comments"))
                .WithGetParameter("uid", userId.ToString())
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithGetParameter("kat", kat)
                .WithGetParameter("length", length.ToString())
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="kat"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<ListDataModel[]> GetList(int userId, string kat, int page, int limit,
            Senpai senpai = null)
        {
            return ApiRequest<ListDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/list"))
                .WithGetParameter("uid", userId.ToString())
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithGetParameter("kat", kat)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="category"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<ToptenDataModel[]> GetTopten(int userId, string category,
            Senpai senpai = null)
        {
            return ApiRequest<ToptenDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/topten"))
                .WithGetParameter("uid", userId.ToString())
                .WithGetParameter("kat", category)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<LoginDataModel> Login(string username, string password, Senpai senpai)
        {
            return ApiRequest<LoginDataModel>.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/login"))
                .WithPostArgument("username", username)
                .WithPostArgument("password", password)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest Logout(Senpai senpai)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/user/logout"))
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        #endregion
    }
}