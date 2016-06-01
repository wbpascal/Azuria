using JetBrains.Annotations;

namespace Azuria.Notifications
{
    /// <summary>
    ///     Represents a notification.
    /// </summary>
    public interface INotification
    {
        #region Properties

        /// <summary>
        ///     Gets the message of the notification.
        /// </summary>
        [NotNull]
        string Message { get; }

        /// <summary>
        ///     Gets the type of the notification.
        /// </summary>
        NotificationType Type { get; }

        #endregion
    }
}