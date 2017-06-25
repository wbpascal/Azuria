using System;
using Azuria.Api.v1.Converters.Notifications;
using Azuria.Api.v1.DataModels.Notifications;
using Azuria.Requests.Builder;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the notification api class.
    /// </summary>
    public class NotificationsRequestBuilder : IApiClassRequestBuilder
    {
        /// <summary>
        /// </summary>
        /// <param name="client"></param>
        public NotificationsRequestBuilder(IProxerClient client)
        {
            this.ProxerClient = client;
        }

        /// <inheritdoc />
        public IProxerClient ProxerClient { get; }

        /// <summary>
        /// Builds a request that deletes a notification.
        /// Api permissions required (class - permission level):
        /// * Notifications - Level 0
        /// </summary>
        /// <param name="nid">
        /// Optional. The id of the notification that will be deleted. If not set or 0, all notifications, that
        /// are marked as read, will be deleted. Default: 0
        /// </param>
        /// <returns>An instance of <see cref="IRequestBuilder" />.</returns>
        public IRequestBuilder Delete(int nid = 0)
        {
            return new Requests.Builder.RequestBuilder(
                    new Uri($"{ApiConstants.ApiUrlV1}/notifications/delete"), this.ProxerClient)
                .WithPostParameter("nid", nid.ToString())
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
        /// <param name="limit">Optional. The amount of news that will be returned per page. Default: 15</param>
        /// <param name="page">Optional. The index of the page that will be loaded. Default: 0</param>
        /// <returns>An instance of <see cref="IRequestBuilderWithResult{T}" /> that returns an array of news.</returns>
        public IRequestBuilderWithResult<NewsNotificationDataModel[]> GetNews(int page = 0, int limit = 15)
        {
            return new RequestBuilder<NewsNotificationDataModel[]>(
                    new Uri($"{ApiConstants.ApiUrlV1}/notifications/news"), this.ProxerClient
                ).WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString());
        }
    }
}