namespace Azuria.EventArguments
{
    /// <summary>
    ///     Repräsentiert die Typen der Benachrichtigungs-Eventdaten.
    /// </summary>
    /// <seealso cref="AmNotificationEventArgs" />
    /// <seealso cref="FriendNotificationEventArgs" />
    /// <seealso cref="INotificationEventArgs" />
    /// <seealso cref="NewsNotificationEventArgs" />
    /// <seealso cref="NotificationEventArgsType" />
    /// <seealso cref="PmNotificationEventArgs" />
    public enum NotificationEventArgsType
    {
        /// <summary>
        ///     <see cref="AmNotificationEventArgs" />
        /// </summary>
        AnimeManga,

        /// <summary>
        ///     <see cref="FriendNotificationEventArgs" />
        /// </summary>
        Friend,

        /// <summary>
        ///     <see cref="NewsNotificationEventArgs" />
        /// </summary>
        News,

        /// <summary>
        ///     <see cref="PmNotificationEventArgs" />
        /// </summary>
        PrivateMessage
    }
}