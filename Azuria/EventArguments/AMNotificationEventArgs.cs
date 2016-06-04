using Azuria.Main;
using Azuria.Notifications;
using JetBrains.Annotations;

namespace Azuria.EventArguments
{
    /// <summary>
    ///     Represents the event details for new <see cref="Anime" /> or <see cref="Manga" />
    ///     <see cref="Senpai.AmUpdateNotificationRaised">notifications</see>.
    /// </summary>
    public class AmNotificationEventArgs : INotificationEventArgs
    {
        private readonly Senpai _senpai;

        internal AmNotificationEventArgs(int count, [NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationEventArgsType.AnimeManga;
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
        public AnimeMangaNotificationCollection Notifications => this._senpai.AnimeMangaNotifications;

        #endregion
    }
}