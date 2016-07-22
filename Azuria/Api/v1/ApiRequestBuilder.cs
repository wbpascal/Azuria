using System;
using System.Collections.Generic;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.Info;

namespace Azuria.Api.v1
{
    internal static class ApiRequestBuilder
    {
        private const string ApiAddress = "https://proxer.me/api/v1";

        #region

        internal static ApiRequest<LoginDataModel> BuildForLogin(string username, string password, Senpai senpai)
        {
            return new ApiRequest<LoginDataModel>
            {
                Address = new Uri(ApiAddress + "/user/login"),
                PostArguments = new Dictionary<string, string> {{"username", username}, {"password", password}},
                Senpai = senpai
            };
        }

        internal static ApiRequest BuildForLogout(Senpai senpai)
        {
            return new ApiRequest
            {
                Address = new Uri(ApiAddress + "/user/logout"),
                CheckLogin = true,
                Senpai = senpai
            };
        }

        internal static ApiRequest<EntryDataModel> BuildForGetEntry(int entryId, Senpai senpai)
        {
            return new ApiRequest<EntryDataModel>
            {
                Address = new Uri(ApiAddress + "/info/entry"),
                PostArguments = new Dictionary<string, string> {{"id", entryId.ToString()}},
                Senpai = senpai
            };
        }

        internal static ApiRequest<NameDataModel[]> BuildForGetName(int entryId, Senpai senpai)
        {
            return new ApiRequest<NameDataModel[]>
            {
                Address = new Uri(ApiAddress + "/info/names"),
                PostArguments = new Dictionary<string, string> {{"id", entryId.ToString()}},
                Senpai = senpai
            };
        }

        internal static ApiRequest<GroupDataModel[]> BuildForGetGroups(int entryId, Senpai senpai)
        {
            return new ApiRequest<GroupDataModel[]>
            {
                Address = new Uri(ApiAddress + "/info/groups"),
                PostArguments = new Dictionary<string, string> {{"id", entryId.ToString()}},
                Senpai = senpai
            };
        }

        #endregion
    }
}