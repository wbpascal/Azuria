using System;
using Azuria.Api.v1.Converters.Notifications;
using Azuria.Api.v1.DataModels.Notifications;

namespace Azuria.Api.v1.RequestBuilder
{
    /// <summary>
    /// </summary>
    public static class NotificationsRequestBuilder
    {
        #region Methods

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * Notifications - Level 0
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="nid"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest Delete(Senpai senpai, int nid = 0)
        {
            return ApiRequest.Create(new Uri($"{ApiConstants.ApiUrlV1}/notifications/delete"))
                .WithCheckLogin(true)
                .WithPostArgument("nid", nid.ToString())
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * Notifications - Level 0
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<NotificationCountDataModel> GetCount(Senpai senpai)
        {
            return ApiRequest<NotificationCountDataModel>.Create(new Uri($"{ApiConstants.ApiUrlV1}/notifications/count"))
                .WithCheckLogin(true)
                .WithCustomDataConverter(new NotificationCountConverter())
                .WithSenpai(senpai);
        }

        /// <summary>
        /// Creates an <see cref="ApiRequest" /> instance that...
        /// 
        /// Api permissions required:
        /// * Notifications - Level 0
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="senpai"></param>
        /// <returns>An instance of <see cref="ApiRequest" /> that returns...</returns>
        public static ApiRequest<NewsNotificationDataModel[]> GetNews(int page, int limit, Senpai senpai)
        {
            return ApiRequest<NewsNotificationDataModel[]>.Create(
                    new Uri($"{ApiConstants.ApiUrlV1}/notifications/news?p={page}&limit={limit}"))
                .WithCheckLogin(true)
                .WithSenpai(senpai);
        }

        #endregion
    }
}