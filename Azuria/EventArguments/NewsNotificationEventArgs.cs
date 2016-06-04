using Azuria.Notifications;
using JetBrains.Annotations;

namespace Azuria.EventArguments
{
    /// <summary>
    ///     Represents the event details for <see cref="Senpai.FriendNotificationRaised">news notifications</see>.
    /// </summary>
    public class NewsNotificationEventArgs : INotificationEventArgs
    {
        private readonly Senpai _senpai;

        internal NewsNotificationEventArgs(int count, [NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationEventArgsType.News;
            this.NotificationCount = count;
        }

        #region Geerbt

        /// <summary>
        ///     Gets the notification count. (Inherited from <see cref="INotificationEventArgs" />)
        /// </summary>
        public int NotificationCount { get; }

        /// <summary>
        ///     Gets the notification type. (Inherited from <see cref="INotificationEventArgs" />)
        /// </summary>
        public NotificationEventArgsType Type { get; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets an object with the help of which the notifications can be retrieved.
        /// </summary>
        [NotNull]
        public NewsNotificationCollection Notifications => this._senpai.NewsNotification;

        #endregion
    }
}