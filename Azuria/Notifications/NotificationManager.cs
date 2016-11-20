using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Notifications;
using Azuria.ErrorHandling;
using Azuria.Media;
using Azuria.Notifications.Media;
using Azuria.Notifications.Message;
using Azuria.Notifications.News;

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
            this.AnimeNotifications = new MediaNotificationEnumerable<Anime>(senpai);
            this.MangaNotifications = new MediaNotificationEnumerable<Manga>(senpai);
            this.MediaNotifications = new MediaNotificationEnumerable<IMediaObject>(senpai);
            this.MessageNotifications = new MessageNotificationEnumerable(senpai);
            this.NewsNotifications = new NewsNotificationEnumerable(senpai);
        }

        #region Properties

        /// <summary>
        /// </summary>
        public MediaNotificationEnumerable<Anime> AnimeNotifications { get; set; }

        /// <summary>
        /// </summary>
        public MediaNotificationEnumerable<Manga> MangaNotifications { get; set; }

        /// <summary>
        /// </summary>
        public MediaNotificationEnumerable<IMediaObject> MediaNotifications { get; set; }

        /// <summary>
        /// </summary>
        public MessageNotificationEnumerable MessageNotifications { get; set; }

        /// <summary>
        /// </summary>
        public NewsNotificationEnumerable NewsNotifications { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        public async Task<IProxerResult> DeleteMediaNotification(int notificationId)
        {
            ProxerApiResponse lResult = await RequestHandler.ApiRequest(
                ApiRequestBuilder.NotificationDelete(this._senpai, notificationId));
            return lResult.Success ? new ProxerResult() : new ProxerResult(lResult.Exceptions);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<IProxerResult> DeleteReadMediaNotifications()
        {
            ProxerApiResponse lResult = await RequestHandler.ApiRequest(
                ApiRequestBuilder.NotificationDelete(this._senpai));
            return lResult.Success ? new ProxerResult() : new ProxerResult(lResult.Exceptions);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<IProxerResult<NotificationCount>> GetUnreadCount()
        {
            ProxerApiResponse<NotificationCountDataModel> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.NotificationGetCount(this._senpai));
            return lResult.Success && (lResult.Result != null)
                ? new ProxerResult<NotificationCount>(new NotificationCount(lResult.Result))
                : new ProxerResult<NotificationCount>(lResult.Exceptions);
        }

        #endregion
    }
}