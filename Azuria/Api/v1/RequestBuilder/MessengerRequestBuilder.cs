using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.Community;
using Azuria.Utilities.Extensions;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// </summary>
    public static class MessengerRequestBuilder
    {
        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<ConferenceInfoDataModel> GetConferenceInfo(int conferenceId, Senpai senpai)
        {
            return ApiRequest<ConferenceInfoDataModel>.Create(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/conferenceinfo"))
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="page"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<ConferenceDataModel[]> GetConferences(ConferenceListType type, int page,
            Senpai senpai)
        {
            return ApiRequest<ConferenceDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/conferences"))
                .WithGetParameter("type", type.ToString().ToLowerInvariant())
                .WithGetParameter("p", page.ToString())
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static ApiRequest<ConstantsDataModel> GetConstants()
        {
            return ApiRequest<ConstantsDataModel>.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/constants"));
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="conferenceId"></param>
        /// <param name="messageId"></param>
        /// <param name="markAsRead"></param>
        /// <returns></returns>
        public static ApiRequest<MessageDataModel[]> GetMessages(Senpai senpai, int conferenceId = 0,
            int messageId = 0, bool markAsRead = true)
        {
            return ApiRequest<MessageDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/messages"))
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithGetParameter("message_id", messageId.ToString())
                .WithGetParameter("read", markAsRead.ToString().ToLowerInvariant())
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="username"></param>
        /// <param name="text"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<int> NewConference(string username, string text, Senpai senpai)
        {
            return ApiRequest<int>.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/newconference"))
                .WithCheckLogin(true)
                .WithPostArgument("username", username)
                .WithPostArgument("text", text)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="participantNames"></param>
        /// <param name="topic"></param>
        /// <param name="senpai"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static ApiRequest<int> NewConferenceGroup(IEnumerable<string> participantNames, string topic,
            Senpai senpai, string text = null)
        {
            List<KeyValuePair<string, string>> lPostArgs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("topic", topic)
            };
            lPostArgs.AddIf(new KeyValuePair<string, string>("text", text), pair => !string.IsNullOrEmpty(pair.Value));
            lPostArgs.AddRange(from participantName in participantNames
                select new KeyValuePair<string, string>("users[]", participantName));

            return ApiRequest<int>.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/newconferencegroup"))
                .WithCheckLogin(true)
                .WithPostArguments(lPostArgs)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest SetBlock(int conferenceId, Senpai senpai)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/setblock"))
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest SetFavour(int conferenceId, Senpai senpai)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/setfavour"))
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="message"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<string> SetMessage(int conferenceId, string message, Senpai senpai)
        {
            return ApiRequest<string>.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/setmessage"))
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithCheckLogin(true)
                .WithPostArgument("text", message)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="reason"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest<int> SetReport(int conferenceId, string reason, Senpai senpai)
        {
            return ApiRequest<int>.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/report"))
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithCheckLogin(true)
                .WithPostArgument("text", reason)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest SetUnblock(int conferenceId, Senpai senpai)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/setunblock"))
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest SetUnfavour(int conferenceId, Senpai senpai)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/setunfavour"))
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static ApiRequest SetUnread(int conferenceId, Senpai senpai)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/setunread"))
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        #endregion
    }
}