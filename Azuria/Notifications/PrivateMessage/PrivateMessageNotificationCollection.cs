using System.Collections;
using System.Collections.Generic;

namespace Azuria.Notifications.PrivateMessage
{
    /// <summary>
    ///     Represents a collection of private message notifications.
    /// </summary>
    public class PrivateMessageNotificationCollection : IEnumerable<PrivateMessageNotification>
    {
        private readonly Senpai _senpai;

        internal PrivateMessageNotificationCollection(Senpai senpai)
        {
            this._senpai = senpai;
        }

        #region Methods

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<PrivateMessageNotification> GetEnumerator()
        {
            return new PrivateMessageNotificationEnumerator(this._senpai);
        }

        #endregion
    }
}