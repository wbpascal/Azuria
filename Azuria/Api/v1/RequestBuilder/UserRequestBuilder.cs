using System;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.Input.User;
using Azuria.Enums;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the user api class.
    /// </summary>
    public class UserRequestBuilder : ApiClassRequestBuilderBase
    {
        /// <inheritdoc />
        public UserRequestBuilder(IProxerClient proxerClient) : base(proxerClient)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="input"><see cref="UserEntryHistoryInput.UserId"/> or <see cref="UserEntryHistoryInput.Username"/> must be given.</param>
        /// <returns></returns>
        public IRequestBuilderWithResult<HistoryDataModel[]> GetHistory(UserEntryHistoryInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<HistoryDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/user/history"), this.ProxerClient
                ).WithGetParameter(input.BuildDictionary());
        }

        /// <summary>
        /// Builds a request that...
        /// Api permissions required (class - permission level):
        /// * User - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns...</returns>
        public IRequestBuilderWithResult<UserInfoDataModel> GetInfo(UserInfoInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<UserInfoDataModel>(
                    new Uri($"{ApiConstants.ApiUrlV1}/user/userinfo"), this.ProxerClient
                ).WithGetParameter(input.BuildDictionary())
                .WithLoginCheck(input.UserId == null && string.IsNullOrEmpty(input.Username));
        }

        /// <summary>
        /// Builds a request that...
        /// Api permissions required (class - permission level):
        /// * User - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns...</returns>
        public IRequestBuilderWithResult<CommentDataModel[]> GetLatestComments(UserCommentsListInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<CommentDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/user/comments"), this.ProxerClient
                ).WithGetParameter(input.BuildDictionary());
        }

        /// <summary>
        /// Builds a request that...
        /// Api permissions required (class - permission level):
        /// * User - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns...</returns>
        public IRequestBuilderWithResult<ListDataModel[]> GetList(UserGetListInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<ListDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/user/list"), this.ProxerClient
                ).WithGetParameter(input.BuildDictionary())
                .WithLoginCheck(input.UserId == null && string.IsNullOrEmpty(input.Username));
        }

        /// <summary>
        /// Builds a request that...
        /// Api permissions required (class - permission level):
        /// * User - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns...</returns>
        public IRequestBuilderWithResult<ToptenDataModel[]> GetTopten(UserToptenListInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<ToptenDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/user/topten"), this.ProxerClient
            ).WithGetParameter(input.BuildDictionary());
        }

        /// <summary>
        /// Builds a request that...
        /// Api permissions required (class - permission level):
        /// * User - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns...</returns>
        public IRequestBuilderWithResult<LoginDataModel> Login(LoginInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<LoginDataModel>(
                    new Uri($"{ApiConstants.ApiUrlV1}/user/login"), this.ProxerClient
                ).WithPostParameter(input.Build());
        }

        /// <summary>
        /// Builds a request that...
        /// Api permissions required (class - permission level):
        /// * User - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilder" /> that returns...</returns>
        public IRequestBuilder Logout()
        {
            return new Requests.Builder.RequestBuilder(
                new Uri($"{ApiConstants.ApiUrlV1}/user/logout"), this.ProxerClient);
        }
    }
}