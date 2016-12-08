using System.Collections;
using System.Collections.Generic;
using Azuria.Media;

namespace Azuria.Notifications.OtherMedia
{
    /// <summary>
    /// Represents a collection of <see cref="Anime" />- and <see cref="Manga" />-notifications.
    /// </summary>
    public class OtherMediaNotificationEnumerable : IEnumerable<OtherMediaNotification>
    {
        private readonly Senpai _senpai;

        internal OtherMediaNotificationEnumerable(Senpai senpai)
        {
            this._senpai = senpai;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public int RetryCount { get; set; } = 2;

        #endregion

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
        public IEnumerator<OtherMediaNotification> GetEnumerator()
        {
            return new OtherMediaNotificationEnumerator(this._senpai, this.RetryCount);
        }

        #endregion
    }
}