using System;
using System.Collections.Generic;
using Azuria.Api.v1.DataModels;

namespace Azuria.Api.v1
{
    internal static class ApiRequestBuilder
    {
        #region

        internal static ApiRequest<LoginDataModel> BuildForLogin(string username, string password, Senpai senpai)
        {
            return new ApiRequest<LoginDataModel>
            {
                Address = new Uri("https://proxer.me/api/v1/user/login"),
                PostArguments = new Dictionary<string, string> {{"username", username}, {"password", password}},
                Senpai = senpai
            };
        }

        internal static ApiRequest BuildForLogout(Senpai senpai)
        {
            return new ApiRequest
            {
                Address = new Uri("http://proxer.me/api/v1/user/logout"),
                Senpai = senpai
            };
        }

        #endregion
    }
}