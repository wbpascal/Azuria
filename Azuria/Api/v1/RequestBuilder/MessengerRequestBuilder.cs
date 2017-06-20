using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Api.Builder;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.Enums.Messenger;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the messenger api class.
    /// </summary>
    public class MessengerRequestBuilder : IApiClassRequestBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public MessengerRequestBuilder(IProxerClient client)
        {
            this.ProxerClient = client;
        }

        /// <inheritdoc />
        public IProxerClient ProxerClient { get; }

        /// <summary>
        /// Builds a request that returns informations about a specified conference.
        /// Requires authentication.
        /// 
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 0
        /// </summary>
        /// <param name="conferenceId">The id of the conference.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns information about a conference.</returns>
        public IRequestBuilderWithResult<ConferenceInfoDataModel> GetConferenceInfo(int conferenceId)
        {
            return new RequestBuilder<ConferenceInfoDataModel>(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/conferenceinfo"), this.ProxerClient
                ).WithGetParameter("conference_id", conferenceId.ToString())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that returns an array of conferences a user participates in.
        /// Requires authentication.
        /// 
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 0
        /// </summary>
        /// <param name="type">Optional. The list from which the conferences are returned.</param>
        /// <param name="page">Optional. The index of the page that will be loaded. Default: 0</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of conferences.</returns>
        public IRequestBuilderWithResult<ConferenceDataModel[]> GetConferences(
            ConferenceList type = ConferenceList.Default, int page = 0)
        {
            return new RequestBuilder<ConferenceDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/conferences"), this.ProxerClient
                ).WithGetParameter("type", type.ToString().ToLowerInvariant())
                .WithGetParameter("p", page.ToString())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that returns all messenger constants. These values should only change
        /// every few months.
        /// 
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns the messenger constants.</returns>
        public IRequestBuilderWithResult<ConstantsDataModel> GetConstants()
        {
            return new RequestBuilder<ConstantsDataModel>(
                new Uri($"{ApiConstants.ApiUrlV1}/messenger/constants"), this.ProxerClient
            );
        }

        /// <summary>
        /// Builds a request that returns the most recent recieved messages of a conference or a user.
        /// Requires authentication.
        /// 
        /// Api permissions required (class - permission level):
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
        public IRequestBuilderWithResult<MessageDataModel[]> GetMessages(
            int conferenceId = 0, int messageId = 0, bool markAsRead = true)
        {
            return new RequestBuilder<MessageDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/messages"), this.ProxerClient
                ).WithGetParameter("conference_id", conferenceId.ToString())
                .WithGetParameter("message_id", messageId.ToString())
                .WithGetParameter("read", markAsRead.ToString().ToLowerInvariant())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that creates a new conference between only two users and returns the id.
        /// If a conference between these users is already found the id of the existing conference will be returned.
        /// Requires authentication.
        /// 
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="username">The username of the user whom the conference will be created with.</param>
        /// <param name="text">The message that will be send to the conference.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns a id of a conference.</returns>
        public IRequestBuilderWithResult<int> NewConference(string username, string text)
        {
            return new RequestBuilder<int>(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/newconference"), this.ProxerClient
                ).WithPostParameter("username", username)
                .WithPostParameter("text", text)
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that creates a new group conference and returns the id.
        /// Requires authentication.
        /// 
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="participantNames">
        /// An enumeration of all names of the participants. The max amount of participants is
        /// limited by the constants (see <see cref="GetConstants" />).
        /// </param>
        /// <param name="topic">The topic of the new conference.</param>
        /// <param name="text">Optional. A message that will be send to the conference. Default: null</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns a id of a group conference.</returns>
        public IRequestBuilderWithResult<int> NewConferenceGroup(
            IEnumerable<string> participantNames, string topic, string text = null)
        {
            List<KeyValuePair<string, string>> lPostArgs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("topic", topic)
            };
            lPostArgs.AddIf(
                new KeyValuePair<string, string>("text", text), pair => !string.IsNullOrEmpty(pair.Value)
            );
            lPostArgs.AddRange(
                participantNames.Select(name => new KeyValuePair<string, string>("users[]", name))
            );

            return new RequestBuilder<int>(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/newconferencegroup"), this.ProxerClient
                ).WithPostParameter(lPostArgs)
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that blocks a conference.
        /// Requires authentication.
        /// 
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="conferenceId">The id of the conference that will be blocked.</param>
        /// <returns>An instance of <see cref="ApiRequest" />.</returns>
        public IRequestBuilder SetBlock(int conferenceId)
        {
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setblock"), this.ProxerClient)
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that marks a conference as a favourite.
        /// Requires authentication.
        /// 
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="conferenceId">The id of the conference that will be marked as a favourite.</param>
        /// <returns>An instance of <see cref="ApiRequest" />.</returns>
        public IRequestBuilder SetFavour(int conferenceId)
        {
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setfavour"), this.ProxerClient)
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that sends a message to a conference. If the message was a command the
        /// answer of the server will be returned.
        /// Requires authentication.
        /// 
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="conferenceId">The id of the conference the message will be send to.</param>
        /// <param name="message">The message that will be send.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns a string.</returns>
        public IRequestBuilderWithResult<string> SetMessage(int conferenceId, string message)
        {
            return new RequestBuilder<string>(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setmessage"), this.ProxerClient
                ).WithGetParameter("conference_id", conferenceId.ToString())
                .WithPostParameter("text", message)
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that marks a conference as read.
        /// Requires authentication.
        /// 
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 0
        /// </summary>
        /// <param name="conferenceId">The id of the conference.</param>
        /// <returns>An instance of <see cref="ApiRequest" />.</returns>
        public IRequestBuilder SetRead(int conferenceId)
        {
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setread"), this.ProxerClient)
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that reports a conference to the admins.
        /// Requires authentication.
        /// 
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 0
        /// </summary>
        /// <param name="conferenceId">The id of the conference that is being reported.</param>
        /// <param name="reason">The reason that the conference is being reported.</param>
        /// <returns>An instance of <see cref="ApiRequest" />.</returns>
        public IRequestBuilder SetReport(int conferenceId, string reason)
        {
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/report"), this.ProxerClient)
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithPostParameter("text", reason)
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that unblocks a conference.
        /// Requires authentication.
        /// 
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="conferenceId">The id of the conference that will be unblocked.</param>
        /// <returns>An instance of <see cref="ApiRequest" />.</returns>
        public IRequestBuilder SetUnblock(int conferenceId)
        {
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setunblock"), this.ProxerClient)
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that removes a conference from the favourites.
        /// Requires authentication.
        /// 
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="conferenceId">The id of the conference that will be removed from the favourites.</param>
        /// <returns>An instance of <see cref="ApiRequest" />.</returns>
        public IRequestBuilder SetUnfavour(int conferenceId)
        {
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setunfavour"), this.ProxerClient)
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that marks a conference as unread.
        /// Requires authentication.
        /// 
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="conferenceId">The id of the conference that will be marked as unread.</param>
        /// <returns>An instance of <see cref="ApiRequest" />.</returns>
        public IRequestBuilder SetUnread(int conferenceId)
        {
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setunread"), this.ProxerClient)
                .WithGetParameter("conference_id", conferenceId.ToString())
                .WithLoginCheck();
        }
    }
}