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
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * Messenger - Level 0
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<ConferenceInfoDataModel> GetConferenceInfo(int conferenceId, Senpai senpai)
        {
            return ApiRequest<ConferenceInfoDataModel>.Create(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/conferenceinfo?conference_id={conferenceId}"))
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * Messenger - Level 0
        /// </summary>
        /// <param name="type"></param>
        /// <param name="page"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<ConferenceDataModel[]> GetConferences(ConferenceListType type, int page,
            Senpai senpai)
        {
            return ApiRequest<ConferenceDataModel[]>.Create(new Uri(
                    $"{ApiConstants.ApiUrlV1}/messenger/conferences?type={type.ToString().ToLowerInvariant()}&p={page}"))
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * Messenger - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<ConstantsDataModel> GetConstants()
        {
            return ApiRequest<ConstantsDataModel>.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/constants"));
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * Messenger - Level 0
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="conferenceId"></param>
        /// <param name="messageId"></param>
        /// <param name="markAsRead"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<MessageDataModel[]> GetMessages(Senpai senpai, int conferenceId = 0,
            int messageId = 0, bool markAsRead = true)
        {
            return ApiRequest<MessageDataModel[]>.Create(new Uri(
                    $"{ApiConstants.ApiUrlV1}/messenger/messages?conference_id={conferenceId}" +
                    $"&message_id={messageId}&read={markAsRead.ToString().ToLowerInvariant()}"))
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="username"></param>
        /// <param name="text"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<int> NewConference(string username, string text, Senpai senpai)
        {
            return ApiRequest<int>.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/newconference"))
                .WithCheckLogin(true)
                .WithPostArgument("username", username)
                .WithPostArgument("text", text)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="participantNames"></param>
        /// <param name="topic"></param>
        /// <param name="senpai"></param>
        /// <param name="text"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
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
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<int> SetBlock(int conferenceId, Senpai senpai)
        {
            return ApiRequest<int>.Create(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setblock?conference_id={conferenceId}"))
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<int> SetFavour(int conferenceId, Senpai senpai)
        {
            return ApiRequest<int>.Create(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setfavour?conference_id={conferenceId}"))
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="message"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<string> SetMessage(int conferenceId, string message, Senpai senpai)
        {
            return ApiRequest<string>.Create(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setmessage?conference_id={conferenceId}"))
                .WithCheckLogin(true)
                .WithPostArgument("text", message)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * Messenger - Level 0
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="reason"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<int> SetReport(int conferenceId, string reason, Senpai senpai)
        {
            return ApiRequest<int>.Create(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/report?conference_id={conferenceId}"))
                .WithCheckLogin(true)
                .WithPostArgument("text", reason)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<int> SetUnblock(int conferenceId, Senpai senpai)
        {
            return ApiRequest<int>.Create(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setunblock?conference_id={conferenceId}"))
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<int> SetUnfavour(int conferenceId, Senpai senpai)
        {
            return ApiRequest<int>.Create(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setunfavour?conference_id={conferenceId}"))
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest SetUnread(int conferenceId, Senpai senpai)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/setunread?conference_id={conferenceId}"))
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        #endregion
    }
}