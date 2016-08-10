using System;
using System.Collections.Generic;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.Enums;

namespace Azuria.Api.v1
{
    internal static class ApiRequestBuilder
    {
        private const string ApiAddress = "https://proxer.me/api/v1";

        #region

        internal static ApiRequest<CommentDataModel[]> BuildForGetComments(int entryId, int page, int limit, string sort,
            Senpai senpai)
        {
            return new ApiRequest<CommentDataModel[]>(senpai)
            {
                Address = new Uri($"{ApiAddress}/info/comments?id={entryId}&p={page}&limit={limit}&sort={sort}")
            };
        }

        internal static ApiRequest<EntryDataModel> BuildForGetEntry(int entryId, Senpai senpai)
        {
            return new ApiRequest<EntryDataModel>(senpai)
            {
                Address = new Uri($"{ApiAddress}/info/entry?id={entryId}")
            };
        }

        internal static ApiRequest<EntryTagDataModel[]> BuildForGetEntryTags(int entryId, Senpai senpai)
        {
            return new ApiRequest<EntryTagDataModel[]>(senpai)
            {
                Address = new Uri($"{ApiAddress}/info/entrytags?id={entryId}")
            };
        }

        internal static ApiRequest<bool> BuildForGetGate(int entryId, Senpai senpai)
        {
            return new ApiRequest<bool>(senpai)
            {
                Address = new Uri($"{ApiAddress}/info/gate?id={entryId}")
            };
        }

        internal static ApiRequest<GroupDataModel[]> BuildForGetGroups(int entryId, Senpai senpai)
        {
            return new ApiRequest<GroupDataModel[]>(senpai)
            {
                Address = new Uri($"{ApiAddress}/info/groups?id={entryId}")
            };
        }

        internal static ApiRequest<AnimeMangaLanguage[]> BuildForGetLanguage(int entryId, Senpai senpai)
        {
            return new ApiRequest<AnimeMangaLanguage[]>(senpai)
            {
                Address = new Uri($"{ApiAddress}/info/lang?id={entryId}")
            };
        }

        internal static ApiRequest<CommentDataModel[]> BuildForGetLatestCommentsUser(int userId, int page, int limit,
            string kat,
            int length, Senpai senpai)
        {
            return new ApiRequest<CommentDataModel[]>(senpai)
            {
                Address =
                    new Uri($"{ApiAddress}/user/comments?uid={userId}&p={page}&limit={limit}&kat={kat}&length={length}")
            };
        }

        internal static ApiRequest<ListInfoDataModel> BuildForGetListInfo(int entryId, int limit, Senpai senpai)
        {
            return new ApiRequest<ListInfoDataModel>(senpai)
            {
                Address = new Uri($"{ApiAddress}/info/listinfo?id={entryId}&limit={limit}")
            };
        }

        internal static ApiRequest<int> BuildForGetListsum(Senpai senpai, string kat = "anime")
        {
            return new ApiRequest<int>(senpai)
            {
                Address = new Uri($"{ApiAddress}/ucp/listsum?kat={kat}")
            };
        }

        internal static ApiRequest<NameDataModel[]> BuildForGetName(int entryId, Senpai senpai)
        {
            return new ApiRequest<NameDataModel[]>(senpai)
            {
                Address = new Uri($"{ApiAddress}/info/names?id={entryId}")
            };
        }

        internal static ApiRequest<PublisherDataModel[]> BuildForGetPublisher(int entryId, Senpai senpai)
        {
            return new ApiRequest<PublisherDataModel[]>(senpai)
            {
                Address = new Uri($"{ApiAddress}/info/publisher?id={entryId}")
            };
        }

        internal static ApiRequest<RelationDataModel[]> BuildForGetRelations(int entryId, Senpai senpai)
        {
            return new ApiRequest<RelationDataModel[]>(senpai)
            {
                Address = new Uri($"{ApiAddress}/info/relations?id={entryId}")
            };
        }

        internal static ApiRequest<SeasonDataModel[]> BuildForGetSeason(int entryId, Senpai senpai)
        {
            return new ApiRequest<SeasonDataModel[]>(senpai)
            {
                Address = new Uri($"{ApiAddress}/info/season?id={entryId}")
            };
        }

        internal static ApiRequest<ToptenDataModel[]> BuildForGetTopten(int userId, string category, Senpai senpai)
        {
            return new ApiRequest<ToptenDataModel[]>(senpai)
            {
                Address = new Uri($"{ApiAddress}/user/topten?uid={userId}&kat={category}")
            };
        }

        internal static ApiRequest<UserInfoDataModel> BuildForGetUserInfo(int? userId, Senpai senpai)
        {
            return new ApiRequest<UserInfoDataModel>(senpai)
            {
                Address = new Uri($"{ApiAddress}/user/userinfo?uid={userId?.ToString() ?? ""}")
            };
        }

        internal static ApiRequest<ListDataModel[]> BuildForGetUserList(int userId, string kat, int page, int limit,
            Senpai senpai)
        {
            return new ApiRequest<ListDataModel[]>(senpai)
            {
                Address = new Uri($"{ApiAddress}/user/list?uid={userId}&kat={kat}&p={page}&limit={limit}")
            };
        }

        internal static ApiRequest<LoginDataModel> BuildForLogin(string username, string password, Senpai senpai)
        {
            return new ApiRequest<LoginDataModel>(senpai)
            {
                Address = new Uri($"{ApiAddress}/user/login"),
                PostArguments = new Dictionary<string, string> {{"username", username}, {"password", password}}
            };
        }

        internal static ApiRequest BuildForLogout(Senpai senpai)
        {
            return new ApiRequest(senpai)
            {
                Address = new Uri($"{ApiAddress}/user/logout"),
                CheckLogin = true
            };
        }

        #endregion
    }
}