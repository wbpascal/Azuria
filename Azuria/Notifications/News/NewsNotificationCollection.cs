using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Azuria.Notifications.News
{
    /// <summary>
    ///     Represents a collection of news notifications.
    /// </summary>
    public class NewsNotificationCollection : INotificationCollection<NewsNotification>
    {
        private readonly Senpai _senpai;

        internal NewsNotificationCollection([NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationType.News;
        }

        #region Geerbt

        /// <summary>
        ///     Gets the type of the notifications.
        /// </summary>
        public NotificationType Type { get; }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<NewsNotification> IEnumerable<NewsNotification>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public INotificationEnumerator<NewsNotification> GetEnumerator()
        {
            return new NewsNotificationEnumerator(this._senpai);
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