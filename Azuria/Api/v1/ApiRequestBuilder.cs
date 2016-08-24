using System;
using System.Collections.Generic;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.Enums;
using ToptenDataModel = Azuria.Api.v1.DataModels.Ucp.ToptenDataModel;

namespace Azuria.Api.v1
{
    internal static class ApiRequestBuilder
    {
        private const string ApiAddress = "https://proxer.me/api/v1";

        #region

        internal static ApiRequest<CommentDataModel[]> InfoGetComments(int entryId, int page, int limit, string sort)
        {
            return
                new ApiRequest<CommentDataModel[]>(
                    new Uri($"{ApiAddress}/info/comments?id={entryId}&p={page}&limit={limit}&sort={sort}"));
        }

        internal static ApiRequest<EntryDataModel> InfoGetEntry(int entryId)
        {
            return new ApiRequest<EntryDataModel>(new Uri($"{ApiAddress}/info/entry?id={entryId}"));
        }

        internal static ApiRequest<EntryTagDataModel[]> InfoGetEntryTags(int entryId)
        {
            return new ApiRequest<EntryTagDataModel[]>(new Uri($"{ApiAddress}/info/entrytags?id={entryId}"));
        }

        internal static ApiRequest<bool> InfoGetGate(int entryId)
        {
            return new ApiRequest<bool>(new Uri($"{ApiAddress}/info/gate?id={entryId}"));
        }

        internal static ApiRequest<GroupDataModel[]> InfoGetGroups(int entryId)
        {
            return new ApiRequest<GroupDataModel[]>(new Uri($"{ApiAddress}/info/groups?id={entryId}"));
        }

        internal static ApiRequest<AnimeMangaLanguage[]> InfoGetLanguage(int entryId)
        {
            return new ApiRequest<AnimeMangaLanguage[]>(new Uri($"{ApiAddress}/info/lang?id={entryId}"));
        }

        internal static ApiRequest<ListInfoDataModel> InfoGetListInfo(int entryId, int limit)
        {
            return new ApiRequest<ListInfoDataModel>(new Uri($"{ApiAddress}/info/listinfo?id={entryId}&limit={limit}"));
        }

        internal static ApiRequest<NameDataModel[]> InfoGetName(int entryId)
        {
            return new ApiRequest<NameDataModel[]>(new Uri($"{ApiAddress}/info/names?id={entryId}"));
        }

        internal static ApiRequest<PublisherDataModel[]> InfoGetPublisher(int entryId)
        {
            return new ApiRequest<PublisherDataModel[]>(new Uri($"{ApiAddress}/info/publisher?id={entryId}"));
        }

        internal static ApiRequest<RelationDataModel[]> InfoGetRelations(int entryId)
        {
            return new ApiRequest<RelationDataModel[]>(new Uri($"{ApiAddress}/info/relations?id={entryId}"));
        }

        internal static ApiRequest<SeasonDataModel[]> InfoGetSeason(int entryId)
        {
            return new ApiRequest<SeasonDataModel[]>(new Uri($"{ApiAddress}/info/season?id={entryId}"));
        }

        internal static ApiRequest<BookmarkDataModel[]> UcpDeleteFavourite(int favouriteId, Senpai senpai)
        {
            return new ApiRequest<BookmarkDataModel[]>(new Uri($"{ApiAddress}/ucp/deletefavorite"))
            {
                CheckLogin = true,
                PostArguments = new Dictionary<string, string> {{"id", favouriteId.ToString()}},
                Senpai = senpai
            };
        }

        internal static ApiRequest<BookmarkDataModel[]> UcpDeleteReminder(int bookmarkId, Senpai senpai)
        {
            return new ApiRequest<BookmarkDataModel[]>(new Uri($"{ApiAddress}/ucp/deletereminder"))
            {
                CheckLogin = true,
                PostArguments = new Dictionary<string, string> {{"id", bookmarkId.ToString()}},
                Senpai = senpai
            };
        }

        internal static ApiRequest<HistoryDataModel[]> UcpGetHistory(int page, int limit, Senpai senpai)
        {
            return new ApiRequest<HistoryDataModel[]>(new Uri($"{ApiAddress}/ucp/history?p={page}&limit={limit}"))
            {
                CheckLogin = true,
                Senpai = senpai
            };
        }

        internal static ApiRequest<int> UcpGetListsum(Senpai senpai, string kat = "anime")
        {
            return new ApiRequest<int>(new Uri($"{ApiAddress}/ucp/listsum?kat={kat}"))
            {
                CheckLogin = true,
                Senpai = senpai
            };
        }

        internal static ApiRequest<BookmarkDataModel[]> UcpGetReminder(string kat, int page, int limit,
            Senpai senpai)
        {
            return
                new ApiRequest<BookmarkDataModel[]>(
                    new Uri($"{ApiAddress}/ucp/reminder?kat={kat}&p={page}&limit={limit}"))
                {
                    CheckLogin = true,
                    Senpai = senpai
                };
        }

        internal static ApiRequest<ToptenDataModel[]> UcpGetTopten(Senpai senpai)
        {
            return new ApiRequest<ToptenDataModel[]>(new Uri($"{ApiAddress}/ucp/topten"))
            {
                CheckLogin = true,
                Senpai = senpai
            };
        }

        internal static ApiRequest<VoteDataModel[]> UcpGetVotes(Senpai senpai)
        {
            return new ApiRequest<VoteDataModel[]>(new Uri($"{ApiAddress}/ucp/votes"))
            {
                CheckLogin = true,
                Senpai = senpai
            };
        }

        internal static ApiRequest UcpSetBookmark(int entryId, int contentIndex, string language, string kat,
            Senpai senpai)
        {
            return
                new ApiRequest(
                    new Uri(
                        $"{ApiAddress}/ucp/setreminder?id={entryId}&episode={contentIndex}&language={language}&kat={kat}"))
                {
                    CheckLogin = true,
                    Senpai = senpai
                };
        }

        internal static ApiRequest UcpSetProgress(int commentId, int progress, Senpai senpai)
        {
            return new ApiRequest(new Uri($"{ApiAddress}/ucp/setcommentstate"))
            {
                CheckLogin = true,
                PostArguments =
                    new Dictionary<string, string> {{"id", commentId.ToString()}, {"value", progress.ToString()}},
                Senpai = senpai
            };
        }

        internal static ApiRequest<UserInfoDataModel> UserGetInfo(int? userId, Senpai senpai = null)
        {
            return
                new ApiRequest<UserInfoDataModel>(new Uri($"{ApiAddress}/user/userinfo?uid={userId?.ToString() ?? ""}"))
                {
                    Senpai = senpai
                };
        }

        internal static ApiRequest<CommentDataModel[]> UserGetLatestComments(int userId, int page, int limit, string kat,
            int length)
        {
            return
                new ApiRequest<CommentDataModel[]>(
                    new Uri($"{ApiAddress}/user/comments?uid={userId}&p={page}&limit={limit}&kat={kat}&length={length}"));
        }

        internal static ApiRequest<ListDataModel[]> UserGetList(int userId, string kat, int page, int limit)
        {
            return
                new ApiRequest<ListDataModel[]>(
                    new Uri($"{ApiAddress}/user/list?uid={userId}&kat={kat}&p={page}&limit={limit}"));
        }

        internal static ApiRequest<DataModels.User.ToptenDataModel[]> UserGetTopten(int userId, string category)
        {
            return
                new ApiRequest<DataModels.User.ToptenDataModel[]>(
                    new Uri($"{ApiAddress}/user/topten?uid={userId}&kat={category}"));
        }

        internal static ApiRequest<LoginDataModel> UserLogin(string username, string password, Senpai senpai)
        {
            return new ApiRequest<LoginDataModel>(new Uri($"{ApiAddress}/user/login"))
            {
                PostArguments = new Dictionary<string, string> {{"username", username}, {"password", password}},
                Senpai = senpai
            };
        }

        internal static ApiRequest UserLogout(Senpai senpai)
        {
            return new ApiRequest(new Uri($"{ApiAddress}/user/logout"))
            {
                CheckLogin = true,
                Senpai = senpai
            };
        }

        #endregion
    }
}