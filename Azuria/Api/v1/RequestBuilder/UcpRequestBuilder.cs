using System;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.Input.Info;
using Azuria.Api.v1.Input.Ucp;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;
using HistoryDataModel = Azuria.Api.v1.DataModels.Ucp.HistoryDataModel;
using ToptenDataModel = Azuria.Api.v1.DataModels.Ucp.ToptenDataModel;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the ucp api class.
    /// </summary>
    public class UcpRequestBuilder : ApiClassRequestBuilderBase
    {
        /// <inheritdoc />
        public UcpRequestBuilder(IProxerClient proxerClient) : base(proxerClient)
        {
        }
        
        /// <summary>
        /// Builds a request that removes an entry from a users topten.
        /// **Requires authentication.**
        /// <para>
        /// Api permissions required (class - permission level):
        /// <para />
        /// * UCP - Level 1
        /// </para>
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilder" /> that removes an entry from a users topten.</returns>
        /// <seealso cref="GetTopten" />
        public IRequestBuilder DeleteTopten(DeleteToptenInput input)
        {
            this.CheckInputDataModel(input);
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/ucp/deletefavorite"), this.ProxerClient)
                .WithPostParameter(input.Build())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that deletes a reminder of a user.
        /// **Requires authentication.**
        /// <para>
        /// Api permissions required (class - permission level):
        /// <para />
        /// * UCP - Level 1
        /// </para>
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilder" /> that deletes a reminder.</returns>
        /// <seealso cref="GetReminder" />
        public IRequestBuilder DeleteReminder(DeleteReminderInput input)
        {
            this.CheckInputDataModel(input);
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/ucp/deletereminder"), this.ProxerClient)
                .WithPostParameter(input.Build())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that removes a users comment upvote.
        /// **Requires authentication.**
        /// Api permissions required (class - permission level):
        /// * UCP - Level 1
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilder" /> that removes a comment upvote.</returns>
        /// <seealso cref="GetVotes" />
        public IRequestBuilder DeleteVote(DeleteVoteInput input)
        {
            this.CheckInputDataModel(input);
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/ucp/deletevote"), this.ProxerClient
                ).WithPostParameter(input.Build())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that returns the history of all watched episodes and read chapters of a
        /// user.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * UCP - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of episodes and chapters.</returns>
        public IRequestBuilderWithResult<HistoryDataModel[]> GetHistory(UcpEntryHistoryInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<HistoryDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/ucp/history"), this.ProxerClient
                ).WithGetParameter(input.BuildDictionary())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that returns a list of all anime or manga a user has listed in their ucp.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// <para />
        /// * UCP - Level 0
        /// </summary>
        /// <param name="input">The data model that contains further input parameters for the request.</param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of anime or manga entries.</returns>
        public IRequestBuilderWithResult<ListDataModel[]> GetList(UcpGetListInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<ListDataModel[]>(new Uri($"{ApiConstants.ApiUrlV1}/ucp/list"), this.ProxerClient)
                .WithGetParameter(input.BuildDictionary())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that returns the sum of a users watched episodes or read chapters.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * UCP - Level 0
        /// </summary>
        /// <returns>
        /// An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns the sum of watched episodes or read
        /// chapters.
        /// </returns>
        public IRequestBuilderWithResult<int> GetListsum(ListsumInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<int>(new Uri($"{ApiConstants.ApiUrlV1}/ucp/listsum"), this.ProxerClient)
                .WithGetParameter(input.BuildDictionary())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that returns all reminders of a user.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * UCP - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of reminders.</returns>
        public IRequestBuilderWithResult<BookmarkDataModel[]> GetReminder(ReminderListInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<BookmarkDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/ucp/reminder"), this.ProxerClient
                ).WithGetParameter(input.BuildDictionary())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that returns the topten of a user (anime and manga).
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * UCP - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of anime and manga.</returns>
        public IRequestBuilderWithResult<ToptenDataModel[]> GetTopten()
        {
            return new RequestBuilder<ToptenDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/ucp/topten"), this.ProxerClient
            ).WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that returns all comments a user has voted for.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * UCP - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of comments.</returns>
        public IRequestBuilderWithResult<VoteDataModel[]> GetVotes()
        {
            return new RequestBuilder<VoteDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/ucp/votes"), this.ProxerClient
            ).WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that sets the number of a users watched episodes/read chapters of an
        /// anime/manga. If the number is set to a value greater or equal of the amount of episodes/chapter the anime/manga has,
        /// the status of the entry will be set to "Finished" as well.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * UCP - Level 1
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilder" /> sets a users progress of an anime/manga.</returns>
        public IRequestBuilder SetCommentState(SetCommentProgressInput input)
        {
            this.CheckInputDataModel(input);
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/ucp/setcommentstate"), this.ProxerClient)
                .WithPostParameter(input.Build())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that adds a reminder for an episode/chapter to a users control-panel.
        /// Requires authentication.
        /// Api permissions required (class - permission level):
        /// * UCP - Level 1
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilder" />.</returns>
        public IRequestBuilder SetReminder(SetReminderInput input)
        {
            this.CheckInputDataModel(input);
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/ucp/setreminder"), this.ProxerClient)
                .WithGetParameter(input.BuildDictionary())
                .WithLoginCheck();
        }
    }
}