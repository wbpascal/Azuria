using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.Api.v1.Input.Messenger;
using Azuria.Enums.Messenger;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the messenger api class.
    /// </summary>
    public class MessengerRequestBuilder : ApiClassRequestBuilderBase
    {
        /// <summary>
        /// </summary>
        /// <param name="client"></param>
        public MessengerRequestBuilder(IProxerClient client) : base(client)
        {
        }

        /// <summary>
        /// Builds a request that returns informations about a specified conference.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns information about a conference.</returns>
        public IRequestBuilderWithResult<ConferenceInfoDataModel> GetConferenceInfo(ConferenceInfoInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<ConferenceInfoDataModel>(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/conferenceinfo"), this.ProxerClient
                ).WithGetParameter(input.BuildDictionary())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that returns an array of conferences a user participates in.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of conferences.</returns>
        public IRequestBuilderWithResult<ConferenceDataModel[]> GetConferences(ConferenceListInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<ConferenceDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/conferences"), this.ProxerClient
                ).WithGetParameter(input.BuildDictionary())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that returns all messenger constants. These values should only change
        /// every few months.
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns the messenger constants.</returns>
        public IRequestBuilderWithResult<ConstantsDataModel> GetConstants()
        {
            return new RequestBuilder<ConstantsDataModel>(
                new Uri($"{ApiConstants.ApiUrlV1}/messenger/constants"), this.ProxerClient
            );
        }

        /// <summary>
        /// Builds a request that returns the most recent recieved messages of a conference or a user.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of messages.</returns>
        public IRequestBuilderWithResult<MessageDataModel[]> GetMessages(MessageListInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<MessageDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/messages"), this.ProxerClient
                ).WithGetParameter(input.BuildDictionary())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that creates a new conference between only two users and returns the id.
        /// If a conference between these users is already found the id of the existing conference will be returned.
        /// Requires authentication.
        /// <para>
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 1
        /// </para>
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns the id of a conference.</returns>
        public IRequestBuilderWithResult<int> NewConference(NewConferenceInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<int>(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/newconference"), this.ProxerClient
                ).WithPostParameter(input.Build())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that creates a new group conference and returns the id.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 1
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns the id of a group conference.</returns>
        public IRequestBuilderWithResult<int> NewConferenceGroup(NewConferenceGroupInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<int>(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/newconferencegroup"), this.ProxerClient
                ).WithPostParameter(input.Build())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that blocks a conference.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 1
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilder" />.</returns>
        public IRequestBuilder SetBlock(ConferenceIdInput input)
        {
            this.CheckInputDataModel(input);
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setblock"), this.ProxerClient)
                .WithGetParameter(input.BuildDictionary())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that marks a conference as a favourite.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 1
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilder" />.</returns>
        public IRequestBuilder SetFavour(ConferenceIdInput input)
        {
            this.CheckInputDataModel(input);
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setfavour"), this.ProxerClient)
                .WithGetParameter(input.BuildDictionary())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that sends a message to a conference. If the message was a command the
        /// answer of the server will be returned.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 1
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns a string.</returns>
        public IRequestBuilderWithResult<string> SetMessage(SendMessageInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<string>(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setmessage"), this.ProxerClient
                ).WithPostParameter(input.Build())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that marks a conference as read.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilder" />.</returns>
        public IRequestBuilder SetRead(ConferenceIdInput input)
        {
            this.CheckInputDataModel(input);
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setread"), this.ProxerClient)
                .WithGetParameter(input.BuildDictionary())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that reports a conference to the admins.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilder" />.</returns>
        public IRequestBuilder SetReport(SendReportInput input)
        {
            this.CheckInputDataModel(input);
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/report"), this.ProxerClient)
                .WithPostParameter(input.Build())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that unblocks a conference.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 1
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilder" />.</returns>
        public IRequestBuilder SetUnblock(ConferenceIdInput input)
        {
            this.CheckInputDataModel(input);
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setunblock"), this.ProxerClient)
                .WithGetParameter(input.BuildDictionary())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that removes a conference from the favourites.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 1
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilder" />.</returns>
        public IRequestBuilder SetUnfavour(ConferenceIdInput input)
        {
            this.CheckInputDataModel(input);
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setunfavour"), this.ProxerClient)
                .WithGetParameter(input.BuildDictionary())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that marks a conference as unread.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * Messenger - Level 1
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilder" />.</returns>
        public IRequestBuilder SetUnread(ConferenceIdInput input)
        {
            this.CheckInputDataModel(input);
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/messenger/setunread"), this.ProxerClient)
                .WithGetParameter(input.BuildDictionary())
                .WithLoginCheck();
        }
    }
}