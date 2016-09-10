using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1.DataModels.Notifications;

namespace Azuria.Notifications.PrivateMessage
{
    /// <summary>
    /// </summary>
    public class PrivateMessageNotificationManager : INotificationManager
    {
        private readonly Senpai _senpai;

        private PrivateMessageNotificationManager(Senpai senpai)
        {
            this._senpai = senpai;
        }

        #region Properties

        Senpai INotificationManager.Senpai => this._senpai;

        #endregion

        #region Events

        /// <summary>
        ///     Represents a method that is executed when new private message notifications are available.
        /// </summary>
        /// <param name="sender">The user that recieved the notifications.</param>
        /// <param name="e">The notifications.</param>
        public delegate void PrivateMessageNotificationEventHandler(
            Senpai sender, IEnumerable<PrivateMessageNotification> e);

        /// <summary>
        /// </summary>
        public event PrivateMessageNotificationEventHandler NotificationRecieved;

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static PrivateMessageNotificationManager Create(Senpai senpai)
        {
            return NotificationCountManager.GetOrAddManager(senpai, new PrivateMessageNotificationManager(senpai));
        }

        void INotificationManager.OnNewNotificationsAvailable(NotificationCountDataModel notificationsCounts)
        {
            PrivateMessageNotification[] lPrivateMessageNotifications =
                new PrivateMessageNotificationCollection(this._senpai).Take(notificationsCounts.PrivateMessages)
                    .ToArray();
            if (lPrivateMessageNotifications.Length > 0)
                this.OnNotificationRecieved(this._senpai, lPrivateMessageNotifications);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnNotificationRecieved(Senpai sender, IEnumerable<PrivateMessageNotification> e)
        {
            this.NotificationRecieved?.Invoke(sender, e);
        }

        #endregion
    }
}