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
            return new ApiRequest<LoginDataModel>(senpai)
            {
                Address = new Uri(ApiAddress + "/user/login"),
                PostArguments = new Dictionary<string, string> {{"username", username}, {"password", password}}
            };
        }

        internal static ApiRequest BuildForLogout(Senpai senpai)
        {
            return new ApiRequest(senpai)
            {
                Address = new Uri(ApiAddress + "/user/logout"),
                CheckLogin = true
            };
        }

        internal static ApiRequest<EntryDataModel> BuildForGetEntry(int entryId, Senpai senpai)
        {
            return new ApiRequest<EntryDataModel>(senpai)
            {
                Address = new Uri(ApiAddress + "/info/entry?id=" + entryId)
            };
        }

        internal static ApiRequest<NameDataModel[]> BuildForGetName(int entryId, Senpai senpai)
        {
            return new ApiRequest<NameDataModel[]>(senpai)
            {
                Address = new Uri(ApiAddress + "/info/names?id=" + entryId)
            };
        }

        internal static ApiRequest<GroupDataModel[]> BuildForGetGroups(int entryId, Senpai senpai)
        {
            return new ApiRequest<GroupDataModel[]>(senpai)
            {
                Address = new Uri(ApiAddress + "/info/groups?id=" + entryId)
            };
        }

        internal static ApiRequest<bool> BuildForGetGate(int entryId, Senpai senpai)
        {
            return new ApiRequest<bool>(senpai)
            {
                Address = new Uri(ApiAddress + "/info/gate?id=" + entryId)
            };
        }

        internal static ApiRequest<PublisherDataModel[]> BuildForGetPublisher(int entryId, Senpai senpai)
        {
            return new ApiRequest<PublisherDataModel[]>(senpai)
            {
                Address = new Uri(ApiAddress + "/info/publisher?id=" + entryId)
            };
        }

        #endregion
    }
}