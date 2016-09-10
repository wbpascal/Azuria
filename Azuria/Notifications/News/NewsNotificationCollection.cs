using System.Collections;
using System.Collections.Generic;

namespace Azuria.Notifications.News
{
    /// <summary>
    ///     Represents a collection of news notifications.
    /// </summary>
    public class NewsNotificationCollection : IEnumerable<NewsNotification>
    {
        private readonly Senpai _senpai;

        internal NewsNotificationCollection(Senpai senpai)
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

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<NewsNotification> GetEnumerator()
        {
            return new NewsNotificationEnumerator(this._senpai);
        }

        #endregion
    }
}