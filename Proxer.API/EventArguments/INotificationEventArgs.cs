namespace Proxer.API.EventArguments
{
    /// <summary>
    ///     Stellt die Eventdaten für die Benachrichtigungen bereit.
    /// </summary>
    /// <seealso cref="AmNotificationEventArgs" />
    /// <seealso cref="FriendNotificationEventArgs" />
    /// <seealso cref="INotificationEventArgs" />
    /// <seealso cref="NewsNotificationEventArgs" />
    /// <seealso cref="NotificationEventArgsType" />
    /// <seealso cref="PmNotificationEventArgs" />
    public interface INotificationEventArgs
    {
        #region Properties

        /// <summary>
        ///     Gibt die Anzahl der Benachrichtigungen zurück.
        /// </summary>
        int NotificationCount { get; }

        /// <summary>
        ///     Gibt den Typ der Benachrichtigung zurück.
        /// </summary>
        NotificationEventArgsType Type { get; }

        #endregion
    }
}