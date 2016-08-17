using System;
using System.Collections.Generic;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.Enums;

namespace Azuria.Api.v1
{
    internal static class ApiRequestBuilder
    {
        private const string ApiAddress = "https://proxer.me/api/v1";

        #region

        internal static ApiRequest<CommentDataModel[]> InfoGetComments(int entryId, int page, int limit, string sort,
            Senpai senpai)
        {
            return new ApiRequest<CommentDataModel[]>(senpai,
                new Uri($"{ApiAddress}/info/comments?id={entryId}&p={page}&limit={limit}&sort={sort}"));
        }

        internal static ApiRequest<EntryDataModel> InfoGetEntry(int entryId, Senpai senpai)
        {
            return new ApiRequest<EntryDataModel>(senpai, new Uri($"{ApiAddress}/info/entry?id={entryId}"));
        }

        internal static ApiRequest<EntryTagDataModel[]> InfoGetEntryTags(int entryId, Senpai senpai)
        {
            return new ApiRequest<EntryTagDataModel[]>(senpai, new Uri($"{ApiAddress}/info/entrytags?id={entryId}"));
        }

        internal static ApiRequest<bool> InfoGetGate(int entryId, Senpai senpai)
        {
            return new ApiRequest<bool>(senpai, new Uri($"{ApiAddress}/info/gate?id={entryId}"));
        }

        internal static ApiRequest<GroupDataModel[]> InfoGetGroups(int entryId, Senpai senpai)
        {
            return new ApiRequest<GroupDataModel[]>(senpai, new Uri($"{ApiAddress}/info/groups?id={entryId}"));
        }

        internal static ApiRequest<AnimeMangaLanguage[]> InfoGetLanguage(int entryId, Senpai senpai)
        {
            return new ApiRequest<AnimeMangaLanguage[]>(senpai, new Uri($"{ApiAddress}/info/lang?id={entryId}"));
        }

        internal static ApiRequest<ListInfoDataModel> InfoGetListInfo(int entryId, int limit, Senpai senpai)
        {
            return new ApiRequest<ListInfoDataModel>(senpai,
                new Uri($"{ApiAddress}/info/listinfo?id={entryId}&limit={limit}"));
        }

        internal static ApiRequest<NameDataModel[]> InfoGetName(int entryId, Senpai senpai)
        {
            return new ApiRequest<NameDataModel[]>(senpai, new Uri($"{ApiAddress}/info/names?id={entryId}"));
        }

        internal static ApiRequest<PublisherDataModel[]> InfoGetPublisher(int entryId, Senpai senpai)
        {
            return new ApiRequest<PublisherDataModel[]>(senpai, new Uri($"{ApiAddress}/info/publisher?id={entryId}"));
        }

        internal static ApiRequest<RelationDataModel[]> InfoGetRelations(int entryId, Senpai senpai)
        {
            return new ApiRequest<RelationDataModel[]>(senpai, new Uri($"{ApiAddress}/info/relations?id={entryId}"));
        }

        internal static ApiRequest<SeasonDataModel[]> InfoGetSeason(int entryId, Senpai senpai)
        {
            return new ApiRequest<SeasonDataModel[]>(senpai, new Uri($"{ApiAddress}/info/season?id={entryId}"));
        }

        internal static ApiRequest<HistoryDataModel[]> UcpGetHistory(int page, int limit, Senpai senpai)
        {
            return new ApiRequest<HistoryDataModel[]>(senpai,
                new Uri($"{ApiAddress}/ucp/history?p={page}&limit={limit}"))
            {
                CheckLogin = true
            };
        }

        internal static ApiRequest<int> UcpGetListsum(Senpai senpai, string kat = "anime")
        {
            return new ApiRequest<int>(senpai, new Uri($"{ApiAddress}/ucp/listsum?kat={kat}"))
            {
                CheckLogin = true
            };
        }

        internal static ApiRequest<BookmarkDataModel[]> UcpGetReminder(string kat, int page, int limit,
            Senpai senpai)
        {
            return new ApiRequest<BookmarkDataModel[]>(senpai,
                new Uri($"{ApiAddress}/ucp/reminder?kat={kat}&p={page}&limit={limit}"))
            {
                CheckLogin = true
            };
        }

        internal static ApiRequest<UserInfoDataModel> UserGetInfo(int? userId, Senpai senpai)
        {
            return new ApiRequest<UserInfoDataModel>(senpai,
                new Uri($"{ApiAddress}/user/userinfo?uid={userId?.ToString() ?? ""}"));
        }

        internal static ApiRequest<CommentDataModel[]> UserGetLatestComments(int userId, int page, int limit,
            string kat, int length, Senpai senpai)
        {
            return new ApiRequest<CommentDataModel[]>(senpai,
                new Uri($"{ApiAddress}/user/comments?uid={userId}&p={page}&limit={limit}&kat={kat}&length={length}"));
        }

        internal static ApiRequest<ListDataModel[]> UserGetList(int userId, string kat, int page, int limit,
            Senpai senpai)
        {
            return new ApiRequest<ListDataModel[]>(senpai,
                new Uri($"{ApiAddress}/user/list?uid={userId}&kat={kat}&p={page}&limit={limit}"));
        }

        internal static ApiRequest<ToptenDataModel[]> UserGetTopten(int userId, string category, Senpai senpai)
        {
            return new ApiRequest<ToptenDataModel[]>(senpai,
                new Uri($"{ApiAddress}/user/topten?uid={userId}&kat={category}"));
        }

        internal static ApiRequest<LoginDataModel> UserLogin(string username, string password, Senpai senpai)
        {
            return new ApiRequest<LoginDataModel>(senpai, new Uri($"{ApiAddress}/user/login"))
            {
                PostArguments = new Dictionary<string, string> {{"username", username}, {"password", password}}
            };
        }

        internal static ApiRequest UserLogout(Senpai senpai)
        {
            return new ApiRequest(senpai, new Uri($"{ApiAddress}/user/logout"))
            {
                CheckLogin = true
            };
        }

        #endregion
    }
}