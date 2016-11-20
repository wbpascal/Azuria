using System.Collections;
using System.Collections.Generic;

namespace Azuria.Notifications.Message
{
    /// <summary>
    /// Represents a collection of private message notifications.
    /// </summary>
    public class MessageNotificationEnumerable : IEnumerable<MessageNotification>
    {
        private readonly Senpai _senpai;

        internal MessageNotificationEnumerable(Senpai senpai)
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
        public IEnumerator<MessageNotification> GetEnumerator()
        {
            return new MessageNotificationEnumerator(this._senpai);
        }

        #endregion
    }
}