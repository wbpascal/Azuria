using System.Collections;
using System.Collections.Generic;
using Azuria.AnimeManga;

namespace Azuria.Notifications.AnimeManga
{
    /// <summary>
    ///     Represents a collection of <see cref="Anime" />- and <see cref="Manga" />-notifications.
    /// </summary>
    public class AnimeMangaNotificationCollection<T> : INotificationCollection<AnimeMangaNotification<T>>
        where T : class, IAnimeMangaObject
    {
        private readonly int _maxNotificationsToParse;
        private readonly Senpai _senpai;

        internal AnimeMangaNotificationCollection(Senpai senpai, int maxNotificationsToParse = -1)
        {
            this._senpai = senpai;
            this._maxNotificationsToParse = maxNotificationsToParse;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public NotificationType Type => typeof(T) == typeof(Anime) ? NotificationType.Anime : NotificationType.Manga;

        #endregion

        #region Methods

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<AnimeMangaNotification<T>> IEnumerable<AnimeMangaNotification<T>>.
            GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public INotificationEnumerator<AnimeMangaNotification<T>> GetEnumerator()
        {
            return new AnimeMangaNotificationEnumerator<T>(this._senpai, this._maxNotificationsToParse);
        }

        #endregion
    }
}