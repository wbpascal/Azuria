using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Azuria.Notifications.FriendRequest
{
    /// <summary>
    ///     Represents a collection of friend request notifications.
    /// </summary>
    public class FriendRequestNotificationCollection : INotificationCollection<FriendRequestNotification>
    {
        private readonly Senpai _senpai;

        internal FriendRequestNotificationCollection([NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationType.FriendRequest;
        }

        #region Geerbt

        /// <summary>
        ///     Gets the type of the notifications.
        /// </summary>
        public NotificationType Type { get; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public INotificationEnumerator<FriendRequestNotification> GetEnumerator()
        {
            return new FriendRequestNotificationEnumerator(this._senpai);
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<FriendRequestNotification> IEnumerable<FriendRequestNotification>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}