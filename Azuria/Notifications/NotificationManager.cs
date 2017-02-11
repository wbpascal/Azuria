using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Notifications;
using Azuria.Api.v1.RequestBuilder;
using Azuria.ErrorHandling;
using Azuria.Notifications.Message;
using Azuria.Notifications.News;
using Azuria.Notifications.OtherMedia;

namespace Azuria.Notifications
{
    /// <summary>
    /// </summary>
    public class NotificationManager
    {
        private readonly Senpai _senpai;

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        public NotificationManager(Senpai senpai)
        {
            this._senpai = senpai;
            this.OtherMediaNotifications = new OtherMediaNotificationEnumerable(senpai);
            this.MessageNotifications = new MessageNotificationEnumerable(senpai);
            this.NewsNotifications = new NewsNotificationEnumerable(senpai);
        }

        #region Properties

        /// <summary>
        /// </summary>
        public MessageNotificationEnumerable MessageNotifications { get; set; }

        /// <summary>
        /// </summary>
        public NewsNotificationEnumerable NewsNotifications { get; set; }

        /// <summary>
        /// </summary>
        public OtherMediaNotificationEnumerable OtherMediaNotifications { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<IProxerResult> DeleteAllOtherMediaNotifications()
        {
            ProxerApiResponse lResult = await RequestHandler.ApiRequest(
                    NotificationsRequestBuilder.Delete(this._senpai))
                .ConfigureAwait(false);
            return lResult.Success ? new ProxerResult() : new ProxerResult(lResult.Exceptions);
        }

        /// <summary>
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        public async Task<IProxerResult> DeleteOtherMediaNotification(int notificationId)
        {
            ProxerApiResponse lResult = await RequestHandler.ApiRequest(
                    NotificationsRequestBuilder.Delete(this._senpai, notificationId))
                .ConfigureAwait(false);
            return lResult.Success ? new ProxerResult() : new ProxerResult(lResult.Exceptions);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<IProxerResult<NotificationCount>> GetCount()
        {
            ProxerApiResponse<NotificationCountDataModel> lResult = await RequestHandler.ApiRequest(
                NotificationsRequestBuilder.GetCount(this._senpai)).ConfigureAwait(false);
            return lResult.Success && lResult.Result != null
                ? new ProxerResult<NotificationCount>(new NotificationCount(lResult.Result))
                : new ProxerResult<NotificationCount>(lResult.Exceptions);
        }

        #endregion
    }
}