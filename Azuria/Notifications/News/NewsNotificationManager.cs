using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1.DataModels.Notifications;

namespace Azuria.Notifications.News
{
    /// <summary>
    /// </summary>
    public class NewsNotificationManager : INotificationManager
    {
        private readonly List<NewsNotificationEventHandler> _newsNotificationEventHandlers =
            new List<NewsNotificationEventHandler>();

        private readonly Senpai _senpai;

        private NewsNotificationManager(Senpai senpai)
        {
            this._senpai = senpai;
            this.Notifications = new NewsNotificationCollection(senpai);
        }

        #region Properties

        /// <summary>
        /// </summary>
        public IEnumerable<NewsNotification> Notifications { get; }

        Senpai INotificationManager.Senpai => this._senpai;

        #endregion

        #region Events

        /// <summary>
        ///     Represents a method that is executed when new news notifications are available.
        /// </summary>
        /// <param name="sender">The user that recieved the notifications.</param>
        /// <param name="e">The notifications. Maximum length of 50 elements.</param>
        public delegate void NewsNotificationEventHandler(Senpai sender, IEnumerable<NewsNotification> e);

        /// <summary>
        /// </summary>
        public event NewsNotificationEventHandler NotificationRecieved
        {
            add
            {
                if (this._newsNotificationEventHandlers.Contains(value)) return;
                this._newsNotificationEventHandlers.Add(value);
                NotificationCountManager.CheckNotificationsForNewEvent().ConfigureAwait(false);
            }
            remove { this._newsNotificationEventHandlers.Remove(value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static NewsNotificationManager Create(Senpai senpai)
        {
            return NotificationCountManager.GetOrAddManager(senpai, new NewsNotificationManager(senpai));
        }

        void INotificationManager.OnNewNotificationsAvailable(NotificationCountDataModel notificationsCounts)
        {
            NewsNotification[] lNewsNotifications =
                new NewsNotificationCollection(this._senpai).Take(notificationsCounts.News).ToArray();
            if (lNewsNotifications.Length > 0) this.OnNotificationRecieved(this._senpai, lNewsNotifications);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnNotificationRecieved(Senpai sender, IEnumerable<NewsNotification> e)
        {
            IEnumerable<NewsNotification> newsNotifications = e as NewsNotification[] ?? e.ToArray();
            foreach (NewsNotificationEventHandler newsNotificationEventHandler in this._newsNotificationEventHandlers)
                newsNotificationEventHandler?.Invoke(sender, newsNotifications);
        }

        #endregion
    }
}