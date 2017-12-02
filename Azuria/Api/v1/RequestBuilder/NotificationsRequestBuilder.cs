using System;
using Azuria.Api.v1.Converters.Notifications;
using Azuria.Api.v1.DataModels.Notifications;
using Azuria.Api.v1.Input.Notifications;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the notification api class.
    /// </summary>
    public class NotificationsRequestBuilder : ApiClassRequestBuilderBase
    {
        /// <inheritdoc />
        public NotificationsRequestBuilder(IProxerClient proxerClient) : base(proxerClient)
        {
        }

        /// <summary>
        /// Builds a request that deletes a notification.
        /// Api permissions required (class - permission level):
        /// * Notifications - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilder" />.</returns>
        public IRequestBuilder Delete(DeleteNotificationInput input)
        {
            this.CheckInputDataModel(input);
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/notifications/delete"), this.ProxerClient)
                .WithPostParameter(input.Build())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that returns how many notifications a user has recieved and not read.
        /// Api permissions required (class - permission level):
        /// * Notifications - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns the number of notifications.</returns>
        public IRequestBuilderWithResult<NotificationCountDataModel> GetCount()
        {
            return new RequestBuilder<NotificationCountDataModel>(
                    new Uri($"{ApiConstants.ApiUrlV1}/notifications/count"), this.ProxerClient
                ).WithCustomDataConverter(new NotificationCountConverter())
                .WithLoginCheck();
        }

        /// <summary>
        /// Builds a request that returns the current news ordered by date of publication.
        /// Api permissions required (class - permission level):
        /// * Notifications - Level 0
        /// </summary>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of news.</returns>
        public IRequestBuilderWithResult<NewsNotificationDataModel[]> GetNews(NewsListInput input)
        {
            this.CheckInputDataModel(input);
            return new RequestBuilder<NewsNotificationDataModel[]>(
                new Uri($"{ApiConstants.ApiUrlV1}/notifications/news"), this.ProxerClient
            ).WithGetParameter(input.BuildDictionary());
        }
    }
}