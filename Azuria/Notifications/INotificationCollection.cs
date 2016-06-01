using System.Collections.Generic;
using System.Threading.Tasks;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.Notifications
{
    /// <summary>
    ///     Represents a collection of notifications.
    /// </summary>
    public interface INotificationCollection
    {
        #region Properties

        /// <summary>
        ///     Gets the type of the notifications.
        /// </summary>
        NotificationType Type { get; }

        #endregion

        #region

        /// <summary>
        ///     Gets all notifications of the current <see cref="Type" />.
        /// </summary>
        /// <returns>An enumeration of notifications.</returns>
        [ItemNotNull]
        Task<ProxerResult<IEnumerable<INotification>>> GetAllNotifications();

        /// <summary>
        ///     Gets a specified <paramref name="count" /> of notifications from the current ones.
        /// </summary>
        /// <param name="count">The notification count.</param>
        /// <returns>
        ///     An enumeration of notifications with a maximum length of <paramref name="count" />.
        /// </returns>
        [ItemNotNull]
        Task<ProxerResult<IEnumerable<INotification>>> GetNotifications(int count);

        #endregion
    }
}