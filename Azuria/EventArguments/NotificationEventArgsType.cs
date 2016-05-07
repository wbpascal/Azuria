namespace Azuria.EventArguments
{
    /// <summary>
    ///     Represents the type of the notification event argument.
    /// </summary>
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