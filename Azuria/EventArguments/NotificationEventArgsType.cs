namespace Azuria.EventArguments
{
    /// <summary>
    ///     Represents the type of the notification event argument.
    /// </summary>
    public enum NotificationEventArgsType
    {
        /// <summary>
        ///     Represents <see cref="AmNotificationEventArgs" />.
        /// </summary>
        AnimeManga,

        /// <summary>
        ///     Represents <see cref="FriendNotificationEventArgs" />.
        /// </summary>
        Friend,

        /// <summary>
        ///     Represents <see cref="NewsNotificationEventArgs" />.
        /// </summary>
        News,

        /// <summary>
        ///     Represents <see cref="PmNotificationEventArgs" />.
        /// </summary>
        PrivateMessage
    }
}