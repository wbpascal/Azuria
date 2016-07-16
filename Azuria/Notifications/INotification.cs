namespace Azuria.Notifications
{
    /// <summary>
    ///     Represents a notification.
    /// </summary>
    public interface INotification
    {
        #region Properties

        /// <summary>
        ///     Gets the id of the notification.
        /// </summary>
        string NotificationId { get; }

        /// <summary>
        ///     Gets the type of the notification.
        /// </summary>
        NotificationType Type { get; }

        #endregion
    }
}