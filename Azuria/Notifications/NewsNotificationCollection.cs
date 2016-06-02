using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Net;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Azuria.Notifications
{
    /// <summary>
    ///     Represents a collection of news notifications.
    /// </summary>
    public class NewsNotificationCollection : INotificationCollection
    {
        private readonly Senpai _senpai;
        private NewsNotification[] _newsNotifications;
        private INotification[] _notification;

        internal NewsNotificationCollection([NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationType.News;
        }

        #region Geerbt

        /// <summary>
        ///     Gets the type of the notifications.
        /// </summary>
        public NotificationType Type { get; }

        /// <summary>
        ///     Gets all notifications of the current <see cref="INotificationCollection.Type" />.
        /// </summary>
        /// <returns>An enumeration of notifications.</returns>
        public async Task<ProxerResult<IEnumerable<INotification>>> GetAllNotifications()
        {
            if (this._notification != null)
                return new ProxerResult<IEnumerable<INotification>>(this._notification);

            ProxerResult lResult;
            return !(lResult = await this.GetInfos()).Success
                ? new ProxerResult<IEnumerable<INotification>>(lResult.Exceptions)
                : new ProxerResult<IEnumerable<INotification>>(this._notification);
        }

        /// <summary>
        ///     Gets a specified <paramref name="count" /> of notifications from the current ones.
        /// </summary>
        /// <param name="count">The notification count.</param>
        /// <returns>
        ///     An enumeration of notifications with a maximum length of <paramref name="count" />.
        /// </returns>
        public async Task<ProxerResult<IEnumerable<INotification>>> GetNotifications(int count)
        {
            if (this._notification != null)
                return this._notification.Length >= count
                    ? new ProxerResult<IEnumerable<INotification>>(this._notification)
                    : new ProxerResult<IEnumerable<INotification>>(this._notification.Take(count).ToArray());
            ProxerResult lResult;
            if (!(lResult = await this.GetInfos()).Success)
                return new ProxerResult<IEnumerable<INotification>>(lResult.Exceptions);

            return this._notification.Length >= count
                ? new ProxerResult<IEnumerable<INotification>>(this._notification)
                : new ProxerResult<IEnumerable<INotification>>(this._notification.Take(count).ToArray());
        }

        #endregion

        #region

        /// <summary>
        ///     Gets all notifications of the current <see cref="INotificationCollection.Type" />.
        /// </summary>
        /// <returns>An enumeration of notifications.</returns>
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<NewsNotification>>> GetAllNewsNotifications()
        {
            if (this._notification != null)
                return new ProxerResult<IEnumerable<NewsNotification>>(this._newsNotifications);

            ProxerResult lResult;
            return !(lResult = await this.GetInfos()).Success
                ? new ProxerResult<IEnumerable<NewsNotification>>(lResult.Exceptions)
                : new ProxerResult<IEnumerable<NewsNotification>>(this._newsNotifications);
        }


        [ItemNotNull]
        private async Task<ProxerResult> GetInfos()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/notifications?format=json&s=news&p=1"),
                        this._senpai.LoginCookies,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            if (lResponse == null || !lResponse.StartsWith("{\"error\":0"))
                return new ProxerResult
                {
                    Success = false
                };

            try
            {
                Dictionary<string, List<NewsNotification>> lDeserialized =
                    JsonConvert.DeserializeObject<Dictionary<string, List<NewsNotification>>>("{" +
                                                                                              lResponse.Substring(
                                                                                                  "{\"error\":0,".Length));

                this._newsNotifications = lDeserialized["notifications"].ToArray();
                this._notification = lDeserialized["notifications"].Cast<INotification>().ToArray();

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        /// <summary>
        ///     Gets a specified <paramref name="count" /> of notifications from the current ones.
        /// </summary>
        /// <param name="count">The notification count.</param>
        /// <returns>
        ///     An enumeration of notifications with a maximum length of <paramref name="count" />.
        /// </returns>
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<NewsNotification>>> GetNewsNotifications(int count)
        {
            if (this._notification != null)
                return this._notification.Length >= count
                    ? new ProxerResult<IEnumerable<NewsNotification>>(this._newsNotifications)
                    : new ProxerResult<IEnumerable<NewsNotification>>(this._newsNotifications.Take(count).ToArray());
            ProxerResult lResult;
            if (!(lResult = await this.GetInfos()).Success)
                return new ProxerResult<IEnumerable<NewsNotification>>(lResult.Exceptions);

            return this._notification.Length >= count
                ? new ProxerResult<IEnumerable<NewsNotification>>(this._newsNotifications)
                : new ProxerResult<IEnumerable<NewsNotification>>(this._newsNotifications.Take(count).ToArray());
        }

        #endregion
    }
}