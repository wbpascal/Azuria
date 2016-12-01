using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1.Converters.Info;
using Azuria.Api.v1.Converters.Notifications;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.Anime;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.DataModels.Manga;
using Azuria.Api.v1.DataModels.Media;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.Api.v1.DataModels.Notifications;
using Azuria.Api.v1.DataModels.Search;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.DataModels.User;
using Azuria.Community;
using Azuria.Media.Properties;
using Azuria.Search;
using Azuria.Search.Input;
using Azuria.Utilities.Extensions;
using ToptenDataModel = Azuria.Api.v1.DataModels.Ucp.ToptenDataModel;

namespace Azuria.Api.v1
{
    /// <summary>
    /// </summary>
    public static class ApiRequestBuilder
    {
        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ApiRequest<string> AnimeGetLink(int id)
        {
            return new ApiRequest<string>(new Uri($"{ApiConstants.ApiUrlV1}/anime/link?id={id}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="episode"></param>
        /// <param name="language"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<StreamDataModel[]> AnimeGetStreams(int id, int episode, string language,
            Senpai senpai = null)
        {
            return
                new ApiRequest<StreamDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/anime/streams?id={id}&episode={episode}&language={language}"))
                {
                    Senpai = senpai
                };
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static ApiRequest<CommentDataModel[]> InfoGetComments(int entryId, int page, int limit, string sort)
        {
            return
                new ApiRequest<CommentDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/info/comments?id={entryId}&p={page}&limit={limit}&sort={sort}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<EntryDataModel> InfoGetEntry(int entryId)
        {
            return new ApiRequest<EntryDataModel>(new Uri($"{ApiConstants.ApiUrlV1}/info/entry?id={entryId}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<MediaTagDataModel[]> InfoGetEntryTags(int entryId)
        {
            return new ApiRequest<MediaTagDataModel[]>(new Uri($"{ApiConstants.ApiUrlV1}/info/entrytags?id={entryId}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<FullEntryDataModel> InfoGetFullEntry(int entryId)
        {
            return new ApiRequest<FullEntryDataModel>(new Uri(
                $"{ApiConstants.ApiUrlV1}/info/fullentry?id={entryId}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<bool> InfoGetGate(int entryId)
        {
            return new ApiRequest<bool>(new Uri($"{ApiConstants.ApiUrlV1}/info/gate?id={entryId}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<TranslatorDataModel[]> InfoGetGroups(int entryId)
        {
            return new ApiRequest<TranslatorDataModel[]>(new Uri(
                $"{ApiConstants.ApiUrlV1}/info/groups?id={entryId}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<MediaLanguage[]> InfoGetLanguage(int entryId)
        {
            return new ApiRequest<MediaLanguage[]>(new Uri($"{ApiConstants.ApiUrlV1}/info/lang?id={entryId}"))
            {
                CustomDataConverter = new LanguageCollectionConverter()
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static ApiRequest<ListInfoDataModel> InfoGetListInfo(int entryId, int page, int limit)
        {
            return
                new ApiRequest<ListInfoDataModel>(
                    new Uri($"{ApiConstants.ApiUrlV1}/info/listinfo?id={entryId}&p={page}&limit={limit}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<NameDataModel[]> InfoGetName(int entryId)
        {
            return new ApiRequest<NameDataModel[]>(new Uri($"{ApiConstants.ApiUrlV1}/info/names?id={entryId}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<PublisherDataModel[]> InfoGetPublisher(int entryId)
        {
            return new ApiRequest<PublisherDataModel[]>(new Uri($"{ApiConstants.ApiUrlV1}/info/publisher?id={entryId}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<RelationDataModel[]> InfoGetRelations(int entryId)
        {
            return new ApiRequest<RelationDataModel[]>(new Uri($"{ApiConstants.ApiUrlV1}/info/relations?id={entryId}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public static ApiRequest<SeasonDataModel[]> InfoGetSeason(int entryId)
        {
            return new ApiRequest<SeasonDataModel[]>(new Uri($"{ApiConstants.ApiUrlV1}/info/season?id={entryId}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="type"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest InfoSetUserInfo(int entryId, string type, Senpai senpai)
        {
            return new ApiRequest(new Uri($"{ApiConstants.ApiUrlV1}/info/setuserinfo"))
            {
                CheckLogin = true,
                PostArguments = new Dictionary<string, string>
                {
                    {"id", entryId.ToString()},
                    {"type", type}
                },
                Senpai = senpai
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="kat"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static ApiRequest<SearchDataModel[]> ListEntryList(EntryListInput input, string kat, int limit,
            int page)
        {
            return
                new ApiRequest<SearchDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/entrylist?limit={limit}&p={page}&kat={kat}"))
                {
                    PostArguments = SearchQueryBuilder.Build(input)
                };
        }

        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static ApiRequest<SearchDataModel[]> ListEntrySearch(SearchInput input, int limit, int page)
        {
            return
                new ApiRequest<SearchDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/list/entrysearch?limit={limit}&p={page}"))
                {
                    PostArguments = SearchQueryBuilder.Build(input)
                };
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="episode"></param>
        /// <param name="language"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<ChapterDataModel> MangaGetChapter(int id, int episode, string language,
            Senpai senpai = null)
        {
            return
                new ApiRequest<ChapterDataModel>(
                    new Uri($"{ApiConstants.ApiUrlV1}/manga/chapter?id={id}&episode={episode}&language={language}"))
                {
                    Senpai = senpai
                };
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static ApiRequest<HeaderDataModel[]> MediaGetHeaderList()
        {
            return
                new ApiRequest<HeaderDataModel[]>(new Uri($"{ApiConstants.ApiUrlV1}/media/headerlist"));
        }

        /// <summary>
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public static ApiRequest<HeaderDataModel> MediaGetRandomHeader(string style = "gray")
        {
            return
                new ApiRequest<HeaderDataModel>(new Uri($"{ApiConstants.ApiUrlV1}/media/randomheader?style={style}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<ConferenceInfoDataModel> MessengerGetConferenceInfo(int conferenceId, Senpai senpai)
        {
            return
                new ApiRequest<ConferenceInfoDataModel>(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/conferenceinfo?conference_id={conferenceId}"))
                {
                    CheckLogin = true,
                    Senpai = senpai
                };
        }

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="page"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<ConferenceDataModel[]> MessengerGetConferences(ConferenceListType type, int page,
            Senpai senpai)
        {
            return
                new ApiRequest<ConferenceDataModel[]>(
                    new Uri(
                        $"{ApiConstants.ApiUrlV1}/messenger/conferences?type={type.ToString().ToLowerInvariant()}&p={page}"))
                {
                    CheckLogin = true,
                    Senpai = senpai
                };
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static ApiRequest<ConstantsDataModel> MessengerGetConstants()
        {
            return new ApiRequest<ConstantsDataModel>(new Uri($"{ApiConstants.ApiUrlV1}/messenger/constants"));
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="conferenceId"></param>
        /// <param name="messageId"></param>
        /// <param name="markAsRead"></param>
        /// <returns></returns>
        public static ApiRequest<MessageDataModel[]> MessengerGetMessages(Senpai senpai, int conferenceId = 0,
            int messageId = 0, bool markAsRead = true)
        {
            return new ApiRequest<MessageDataModel[]>(new Uri(
                $"{ApiConstants.ApiUrlV1}/messenger/messages?conference_id={conferenceId}" +
                $"&message_id={messageId}&read={markAsRead.ToString().ToLowerInvariant()}"))
            {
                CheckLogin = true,
                Senpai = senpai
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="username"></param>
        /// <param name="text"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<int> MessengerNewConference(string username, string text, Senpai senpai)
        {
            return new ApiRequest<int>(new Uri($"{ApiConstants.ApiUrlV1}/messenger/newconference"))
            {
                CheckLogin = true,
                PostArguments = new Dictionary<string, string> {{"username", username}, {"text", text}},
                Senpai = senpai
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="participantNames"></param>
        /// <param name="topic"></param>
        /// <param name="senpai"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static ApiRequest<int> MessengerNewConferenceGroup(IEnumerable<string> participantNames, string topic,
            Senpai senpai, string text = null)
        {
            List<KeyValuePair<string, string>> lPostArgs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("topic", topic)
            };
            lPostArgs.AddIf(new KeyValuePair<string, string>("text", text), pair => !string.IsNullOrEmpty(pair.Value));
            lPostArgs.AddRange(from participantName in participantNames
                select new KeyValuePair<string, string>("users[]", participantName));

            return new ApiRequest<int>(new Uri($"{ApiConstants.ApiUrlV1}/messenger/newconferencegroup"))
            {
                CheckLogin = true,
                PostArguments = lPostArgs,
                Senpai = senpai
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<int> MessengerSetBlock(int conferenceId, Senpai senpai)
        {
            return
                new ApiRequest<int>(new Uri($"{ApiConstants.ApiUrlV1}/messenger/setblock?conference_id={conferenceId}"))
                {
                    CheckLogin = true,
                    Senpai = senpai
                };
        }

        /// <summary>
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<int> MessengerSetFavour(int conferenceId, Senpai senpai)
        {
            return
                new ApiRequest<int>(new Uri($"{ApiConstants.ApiUrlV1}/messenger/setfavour?conference_id={conferenceId}"))
                {
                    CheckLogin = true,
                    Senpai = senpai
                };
        }

        /// <summary>
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="message"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<string> MessengerSetMessage(int conferenceId, string message, Senpai senpai)
        {
            return
                new ApiRequest<string>(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setmessage?conference_id={conferenceId}"))
                {
                    CheckLogin = true,
                    PostArguments = new Dictionary<string, string> {{"text", message}},
                    Senpai = senpai
                };
        }

        /// <summary>
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="reason"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<int> MessengerSetReport(int conferenceId, string reason, Senpai senpai)
        {
            return new ApiRequest<int>(new Uri($"{ApiConstants.ApiUrlV1}/messenger/report?conference_id={conferenceId}"))
            {
                CheckLogin = true,
                PostArguments = new Dictionary<string, string> {{"text", reason}},
                Senpai = senpai
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<int> MessengerSetUnblock(int conferenceId, Senpai senpai)
        {
            return
                new ApiRequest<int>(new Uri($"{ApiConstants.ApiUrlV1}/messenger/setunblock?conference_id={conferenceId}"))
                {
                    CheckLogin = true,
                    Senpai = senpai
                };
        }

        /// <summary>
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<int> MessengerSetUnfavour(int conferenceId, Senpai senpai)
        {
            return
                new ApiRequest<int>(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setunfavour?conference_id={conferenceId}"))
                {
                    CheckLogin = true,
                    Senpai = senpai
                };
        }

        /// <summary>
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest MessengerSetUnread(int conferenceId, Senpai senpai)
        {
            return
                new ApiRequest(new Uri($"{ApiConstants.ApiUrlV1}/messenger/setunread?conference_id={conferenceId}"))
                {
                    CheckLogin = true,
                    Senpai = senpai
                };
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="nid"></param>
        /// <returns></returns>
        public static ApiRequest NotificationDelete(Senpai senpai, int nid = 0)
        {
            return new ApiRequest(new Uri($"{ApiConstants.ApiUrlV1}/notifications/delete"))
            {
                CheckLogin = true,
                PostArguments = new Dictionary<string, string> {{"nid", nid.ToString()}},
                Senpai = senpai
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<NotificationCountDataModel> NotificationGetCount(Senpai senpai)
        {
            return new ApiRequest<NotificationCountDataModel>(new Uri($"{ApiConstants.ApiUrlV1}/notifications/count"))
            {
                CheckLogin = true,
                CustomDataConverter = new NotificationCountConverter(),
                Senpai = senpai
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<NewsNotificationDataModel[]> NotificationGetNews(int page, int limit, Senpai senpai)
        {
            return
                new ApiRequest<NewsNotificationDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/notifications/news?p={page}&limit={limit}"))
                {
                    CheckLogin = true,
                    Senpai = senpai
                };
        }

        /// <summary>
        /// </summary>
        /// <param name="favouriteId"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<BookmarkDataModel[]> UcpDeleteFavourite(int favouriteId, Senpai senpai)
        {
            return new ApiRequest<BookmarkDataModel[]>(new Uri($"{ApiConstants.ApiUrlV1}/ucp/deletefavorite"))
            {
                CheckLogin = true,
                PostArguments = new Dictionary<string, string> {{"id", favouriteId.ToString()}},
                Senpai = senpai
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="bookmarkId"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<BookmarkDataModel[]> UcpDeleteReminder(int bookmarkId, Senpai senpai)
        {
            return new ApiRequest<BookmarkDataModel[]>(new Uri($"{ApiConstants.ApiUrlV1}/ucp/deletereminder"))
            {
                CheckLogin = true,
                PostArguments = new Dictionary<string, string> {{"id", bookmarkId.ToString()}},
                Senpai = senpai
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="voteId"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<BookmarkDataModel[]> UcpDeleteVote(int voteId, Senpai senpai)
        {
            return new ApiRequest<BookmarkDataModel[]>(new Uri($"{ApiConstants.ApiUrlV1}/ucp/deletevote"))
            {
                CheckLogin = true,
                PostArguments = new Dictionary<string, string> {{"id", voteId.ToString()}},
                Senpai = senpai
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<HistoryDataModel[]> UcpGetHistory(int page, int limit, Senpai senpai)
        {
            return
                new ApiRequest<HistoryDataModel[]>(new Uri($"{ApiConstants.ApiUrlV1}/ucp/history?p={page}&limit={limit}"))
                {
                    CheckLogin = true,
                    Senpai = senpai
                };
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="kat"></param>
        /// <returns></returns>
        public static ApiRequest<int> UcpGetListsum(Senpai senpai, string kat = "anime")
        {
            return new ApiRequest<int>(new Uri($"{ApiConstants.ApiUrlV1}/ucp/listsum?kat={kat}"))
            {
                CheckLogin = true,
                Senpai = senpai
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="kat"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<BookmarkDataModel[]> UcpGetReminder(string kat, int page, int limit,
            Senpai senpai)
        {
            return
                new ApiRequest<BookmarkDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/ucp/reminder?kat={kat}&p={page}&limit={limit}"))
                {
                    CheckLogin = true,
                    Senpai = senpai
                };
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<ToptenDataModel[]> UcpGetTopten(Senpai senpai)
        {
            return new ApiRequest<ToptenDataModel[]>(new Uri($"{ApiConstants.ApiUrlV1}/ucp/topten"))
            {
                CheckLogin = true,
                Senpai = senpai
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<VoteDataModel[]> UcpGetVotes(Senpai senpai)
        {
            return new ApiRequest<VoteDataModel[]>(new Uri($"{ApiConstants.ApiUrlV1}/ucp/votes"))
            {
                CheckLogin = true,
                Senpai = senpai
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="contentIndex"></param>
        /// <param name="language"></param>
        /// <param name="kat"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest UcpSetBookmark(int entryId, int contentIndex, string language, string kat,
            Senpai senpai)
        {
            return
                new ApiRequest(
                    new Uri(
                        $"{ApiConstants.ApiUrlV1}/ucp/setreminder?id={entryId}&episode={contentIndex}&language={language}&kat={kat}"))
                {
                    CheckLogin = true,
                    Senpai = senpai
                };
        }

        /// <summary>
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="progress"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest UcpSetProgress(int commentId, int progress, Senpai senpai)
        {
            return new ApiRequest(new Uri($"{ApiConstants.ApiUrlV1}/ucp/setcommentstate"))
            {
                CheckLogin = true,
                PostArguments =
                    new Dictionary<string, string> {{"id", commentId.ToString()}, {"value", progress.ToString()}},
                Senpai = senpai
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<UserInfoDataModel> UserGetInfo(Senpai senpai)
        {
            return new ApiRequest<UserInfoDataModel>(new Uri($"{ApiConstants.ApiUrlV1}/user/userinfo"))
            {
                Senpai = senpai
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static ApiRequest<UserInfoDataModel> UserGetInfo(int userId)
        {
            return new ApiRequest<UserInfoDataModel>(new Uri($"{ApiConstants.ApiUrlV1}/user/userinfo?uid={userId}"));
        }

        /// <summary>
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static ApiRequest<UserInfoDataModel> UserGetInfo(string username)
        {
            return
                new ApiRequest<UserInfoDataModel>(new Uri($"{ApiConstants.ApiUrlV1}/user/userinfo?username={username}"));
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
        public static ApiRequest<CommentDataModel[]> UserGetLatestComments(int userId, int page, int limit, string kat,
            int length, Senpai senpai = null)
        {
            return new ApiRequest<CommentDataModel[]>(new Uri(
                $"{ApiConstants.ApiUrlV1}/user/comments?uid={userId}&p={page}&limit={limit}&kat={kat}&length={length}"))
            {
                Senpai = senpai
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="kat"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<ListDataModel[]> UserGetList(int userId, string kat, int page, int limit,
            Senpai senpai = null)
        {
            return new ApiRequest<ListDataModel[]>(new Uri(
                $"{ApiConstants.ApiUrlV1}/user/list?uid={userId}&kat={kat}&p={page}&limit={limit}"))
            {
                Senpai = senpai
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="category"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<DataModels.User.ToptenDataModel[]> UserGetTopten(int userId, string category,
            Senpai senpai = null)
        {
            return new ApiRequest<DataModels.User.ToptenDataModel[]>(new Uri(
                $"{ApiConstants.ApiUrlV1}/user/topten?uid={userId}&kat={category}"))
            {
                Senpai = senpai
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<LoginDataModel> UserLogin(string username, string password, Senpai senpai)
        {
            return new ApiRequest<LoginDataModel>(new Uri($"{ApiConstants.ApiUrlV1}/user/login"))
            {
                PostArguments = new Dictionary<string, string> {{"username", username}, {"password", password}},
                Senpai = senpai
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest UserLogout(Senpai senpai)
        {
            return new ApiRequest(new Uri($"{ApiConstants.ApiUrlV1}/user/logout"))
            {
                CheckLogin = true,
                Senpai = senpai
            };
        }

        #endregion
    }
}