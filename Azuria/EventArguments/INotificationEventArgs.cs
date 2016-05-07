namespace Azuria.EventArguments
{
    /// <summary>
    ///     Represents the event details of the notification events.
    /// </summary>
    /// <seealso cref="AmNotificationEventArgs" />
    /// <seealso cref="FriendNotificationEventArgs" />
    /// <seealso cref="NewsNotificationEventArgs" />
    /// <seealso cref="PmNotificationEventArgs" />
    public interface INotificationEventArgs
    {
        #region Properties

        /// <summary>
        ///     Gets the notification count. (Inherited from <see cref="INotificationEventArgs" />)
        /// </summary>
        int NotificationCount { get; }

        /// <summary>
        ///     Gets the notification type. (Inherited from <see cref="INotificationEventArgs" />)
        /// </summary>
        NotificationEventArgsType Type { get; }

        #endregion
    }
}