using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Azuria.Notifications.PrivateMessage
{
    /// <summary>
    ///     Represents a collection of private message notifications.
    /// </summary>
    public class PrivateMessageNotificationCollection : INotificationCollection<PrivateMessageNotification>
    {
        private readonly Senpai _senpai;

        internal PrivateMessageNotificationCollection([NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationType.PrivateMessage;
        }

        #region Inherited

        /// <summary>
        ///     Gets the type of the notifications.
        /// </summary>
        public NotificationType Type { get; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public INotificationEnumerator<PrivateMessageNotification> GetEnumerator()
        {
            return new PrivateMessageNotificationEnumerator(this._senpai);
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<PrivateMessageNotification> IEnumerable<PrivateMessageNotification>.GetEnumerator()
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