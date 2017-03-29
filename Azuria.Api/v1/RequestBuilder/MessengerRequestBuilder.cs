using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Api.Enums.Messenger;
using Azuria.Api.Helpers.Extensions;
using Azuria.Api.v1.DataModels.Messenger;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the messenger api class.
    /// </summary>
    public static class MessengerRequestBuilder
    {
        #region Methods

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns informations about a specified conference.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 0
        /// </summary>
        /// <param name="conferenceId">The id of the conference.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns information about a conference.</returns>
        public static ApiRequest<ConferenceInfoDataModel> GetConferenceInfo(int conferenceId)
        {
            return ApiRequest<ConferenceInfoDataModel>.Create(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/conferenceinfo"))
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithLoginCheck(true);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns an array of conferences a user participates in.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 0
        /// </summary>
        /// <param name="type">
        /// Optional. The list from which the conferences are returned. Default:
        /// <see cref="ConferenceListType.Default" />
        /// </param>
        /// <param name="page">Optional. The index of the page that will be loaded. Default: 0</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of conferences.</returns>
        public static ApiRequest<ConferenceDataModel[]> GetConferences(
            ConferenceListType type = ConferenceListType.Default, int page = 0)
        {
            return ApiRequest<ConferenceDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/conferences"))
                .WithGetParameter("type", type.ToString().ToLowerInvariant())
                .WithGetParameter("p", page.ToString())
                .WithLoginCheck(true);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns all messenger constants. These values should only change
        /// every few months.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns the messenger constants.</returns>
        public static ApiRequest<ConstantsDataModel> GetConstants()
        {
            return ApiRequest<ConstantsDataModel>.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/constants"));
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns the most recent recieved messages of a conference or a user.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 0
        /// </summary>
        /// <param name="conferenceId">
        /// Optional. The id of the conference that has unread messages. If not specified or 0, a
        /// specified amount (see <see cref="GetConstants" />) of the most recent messages regardless of conference will be
        /// returned. Default: 0
        /// </param>
        /// <param name="messageId">
        /// Optional. The id of the message that will be the oldest returned message. If not specified or
        /// 0, a specified amount (see <see cref="GetConstants" />) of the most recent messages will be returned. Default: 0
        /// </param>
        /// <param name="markAsRead">
        /// Optional. A value indicating whether the conference should be marked as read after returning
        /// the messages. The value of this parameter will be ignored if <paramref name="conferenceId" /> is 0 or not specified.
        /// Default: true
        /// </param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of messages.</returns>
        public static ApiRequest<MessageDataModel[]> GetMessages(int conferenceId = 0,
            int messageId = 0, bool markAsRead = true)
        {
            return ApiRequest<MessageDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/messages"))
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithGetParameter("message_id", messageId.ToString())
                .WithGetParameter("read", markAsRead.ToString().ToLowerInvariant())
                .WithLoginCheck(true);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that creates a new conference between only two users and returns the id.
        /// If a conference between these users is already found the id of the existing conference will be returned.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="username">The username of the user whom the conference will be created with.</param>
        /// <param name="text">The message that will be send to the conference.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns a id of a conference.</returns>
        public static ApiRequest<int> NewConference(string username, string text)
        {
            return ApiRequest<int>.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/newconference"))
                .WithLoginCheck(true)
                .WithPostParameter("username", username)
                .WithPostParameter("text", text);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that creates a new group conference and returns the id.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="participantNames">
        /// An enumeration of all names of the participants. The max amount of participants is
        /// limited by the constants (see <see cref="GetConstants" />).
        /// </param>
        /// <param name="topic">The topic of the new conference.</param>
        /// <param name="text">Optional. A message that will be send to the conference. Default: null</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns a id of a group conference.</returns>
        public static ApiRequest<int> NewConferenceGroup(IEnumerable<string> participantNames, string topic,
            string text = null)
        {
            List<KeyValuePair<string, string>> lPostArgs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("topic", topic)
            };
            lPostArgs.AddIf(new KeyValuePair<string, string>("text", text), pair => !string.IsNullOrEmpty(pair.Value));
            lPostArgs.AddRange(from participantName in participantNames
                select new KeyValuePair<string, string>("users[]", participantName));

            return ApiRequest<int>.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/newconferencegroup"))
                .WithLoginCheck(true)
                .WithPostParameters(lPostArgs);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that blocks a conference.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="conferenceId">The id of the conference that will be blocked.</param>
        /// <returns>An instance of <see cref="ApiRequest" />.</returns>
        public static ApiRequest SetBlock(int conferenceId)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/setblock"))
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithLoginCheck(true);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that marks a conference as a favourite.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="conferenceId">The id of the conference that will be marked as a favourite.</param>
        /// <returns>An instance of <see cref="ApiRequest" />.</returns>
        public static ApiRequest SetFavour(int conferenceId)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/setfavour"))
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithLoginCheck(true);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that sends a message to a conference. If the message was a command the
        /// answer of the server will be returned.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="conferenceId">The id of the conference the message will be send to.</param>
        /// <param name="message">The message that will be send.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns a string.</returns>
        public static ApiRequest<string> SetMessage(int conferenceId, string message)
        {
            return ApiRequest<string>.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/setmessage"))
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithLoginCheck(true)
                .WithPostParameter("text", message);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that marks a conference as read.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 0
        /// </summary>
        /// <param name="conferenceId">The id of the conference.</param>
        /// <returns>An instance of <see cref="ApiRequest" />.</returns>
        public static ApiRequest SetRead(int conferenceId)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/setread"))
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithLoginCheck(true);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that reports a conference to the admins.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 0
        /// </summary>
        /// <param name="conferenceId">The id of the conference that is being reported.</param>
        /// <param name="reason">The reason that the conference is being reported.</param>
        /// <returns>An instance of <see cref="ApiRequest" />.</returns>
        public static ApiRequest SetReport(int conferenceId, string reason)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/report"))
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithLoginCheck(true)
                .WithPostParameter("text", reason);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that unblocks a conference.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="conferenceId">The id of the conference that will be unblocked.</param>
        /// <returns>An instance of <see cref="ApiRequest" />.</returns>
        public static ApiRequest SetUnblock(int conferenceId)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/setunblock"))
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithLoginCheck(true);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that removes a conference from the favourites.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="conferenceId">The id of the conference that will be removed from the favourites.</param>
        /// <returns>An instance of <see cref="ApiRequest" />.</returns>
        public static ApiRequest SetUnfavour(int conferenceId)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/setunfavour"))
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithLoginCheck(true);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that marks a conference as unread.
        /// Requires authentication.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="conferenceId">The id of the conference that will be marked as unread.</param>
        /// <returns>An instance of <see cref="ApiRequest" />.</returns>
        public static ApiRequest SetUnread(int conferenceId)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/messenger/setunread"))
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithLoginCheck(true);
        }

        #endregion
    }
}