using Azuria.Notifications;
using JetBrains.Annotations;

namespace Azuria.EventArguments
{
    /// <summary>
    ///     Represents the event details for <see cref="Senpai.FriendNotificationRaised">news notifications</see>.
    /// </summary>
    /// <seealso cref="AmNotificationEventArgs" />
    /// <seealso cref="FriendNotificationEventArgs" />
    /// <seealso cref="PmNotificationEventArgs" />
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
        ///     Gibt die Anzahl der Benachrichtigungen zurück.
        ///     <para>(Vererbt von <see cref="INotificationEventArgs" />)</para>
        /// </summary>
        public int NotificationCount { get; }

        /// <summary>
        ///     Gibt den Typ der Benachrichtigung zurück.
        ///     <para>(Vererbt von <see cref="INotificationEventArgs" />)</para>
        /// </summary>
        public NotificationEventArgsType Type { get; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets an object with the help of which the notifications can be retrieved.
        /// </summary>
        [NotNull]
        public NewsCollection Notifications => this._senpai.News;

        #endregion
    }
}