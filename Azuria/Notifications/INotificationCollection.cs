using System.Collections.Generic;

namespace Azuria.Notifications
{
    /// <summary>
    ///     Represents a collection of notifications.
    /// </summary>
    public interface INotificationCollection<out T> : IEnumerable<T> where T : INotification
    {
        #region Properties

        /// <summary>
        ///     Gets the type of the notifications.
        /// </summary>
        NotificationType Type { get; }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <returns></returns>
        new INotificationEnumerator<T> GetEnumerator();

        #endregion
    }
}