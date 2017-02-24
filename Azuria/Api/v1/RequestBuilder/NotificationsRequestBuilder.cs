using System;
using Azuria.Api.v1.Converters.Notifications;
using Azuria.Api.v1.DataModels.Notifications;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// Represents the notification api class.
    /// </summary>
    public static class NotificationsRequestBuilder
    {
        #region Methods

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that deletes a notification.
        /// 
        /// Api permissions required:
        /// * Notifications - Level 0
        /// </summary>
        /// <param name="senpai">The user that owns the notification. Needs to be logged in.</param>
        /// <param name="nid">
        /// Optional. The id of the notification that will be deleted. If not set or 0, all notifications, that
        /// are marked as read, will be deleted. Default: 0
        /// </param>
        /// <returns>An instance of <see cref="ApiRequest" />.</returns>
        public static ApiRequest Delete(Senpai senpai, int nid = 0)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/notifications/delete"))
                .WithCheckLogin(true)
                .WithPostArgument("nid", nid.ToString())
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns how many notifications a user has recieved and not read.
        /// 
        /// Api permissions required:
        /// * Notifications - Level 0
        /// </summary>
        /// <param name="senpai">The user that recieved the notifications. Needs to be logged in.</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns the number of notifications.</returns>
        public static ApiRequest<NotificationCountDataModel> GetCount(Senpai senpai)
        {
            return ApiRequest<NotificationCountDataModel>.Create(new Uri($"{ApiConstants.ApiUrlV1}/notifications/count"))
                .WithCheckLogin(true)
                .WithCustomDataConverter(new NotificationCountConverter())
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that returns the current news ordered by date of publication.
        /// 
        /// Api permissions required:
        /// * Notifications - Level 0
        /// </summary>
        /// <param name="senpai">
        /// Optional. If given and logged in all recieved news notifications of the user will be marked as
        /// read. Default: null
        /// </param>
        /// <param name="limit">Optional. The amount of news that will be returned per page. Default: 15</param>
        /// <param name="page">Optional. The index of the page that will be loaded. Default: 0</param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns an array of news.</returns>
        public static ApiRequest<NewsNotificationDataModel[]> GetNews(Senpai senpai = null, int page = 0, int limit = 15)
        {
            return ApiRequest<NewsNotificationDataModel[]>.Create(new Uri($"{ApiConstants.ApiUrlV1}/notifications/news"))
                .WithGetParameter("p", page.ToString())
                .WithGetParameter("limit", limit.ToString())
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        #endregion
    }
}