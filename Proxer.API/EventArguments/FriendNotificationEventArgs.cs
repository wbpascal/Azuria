using Proxer.API.Notifications;

namespace Proxer.API.EventArguments
{
    /// <summary>
    ///     Stellt die Eventdaten für die Benachrichtigungen bereit.
    ///     <para>(Vererbt von <see cref="INotificationEventArgs" />)</para>
    /// </summary>
    /// <seealso cref="AmNotificationEventArgs" />
    /// <seealso cref="FriendNotificationEventArgs" />
    /// <seealso cref="INotificationEventArgs" />
    /// <seealso cref="NewsNotificationEventArgs" />
    /// <seealso cref="NotificationEventArgsType" />
    /// <seealso cref="PmNotificationEventArgs" />
    public class FriendNotificationEventArgs : INotificationEventArgs
    {
        private readonly Senpai _senpai;

        internal FriendNotificationEventArgs(int count, Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationEventArgsType.Friend;
            this.NotificationCount = count;
        }

        #region Geerbt

        /// <summary>
        ///     Gibt die Anzahl der Benachrichtigungen zurück.
        ///     <para>(Vererbt von <see cref="INotificationEventArgs" />)</para>
        /// </summary>
        public int NotificationCount { get; private set; }

        /// <summary>
        ///     Gibt den Typ der Benachrichtigung zurück.
        ///     <para>(Vererbt von <see cref="INotificationEventArgs" />)</para>
        /// </summary>
        public NotificationEventArgsType Type { get; private set; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gibt ein Objekt zurück, mithilfe dessen die Benachrichtigungen abgerufen werden können.
        /// </summary>
        public FriendRequestCollection Benachrichtigungen
        {
            get { return this._senpai.FriendRequests; }
        }

        #endregion
    }
}