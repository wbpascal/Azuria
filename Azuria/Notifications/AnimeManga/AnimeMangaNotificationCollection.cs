using System.Collections;
using System.Collections.Generic;
using Azuria.AnimeManga;
using JetBrains.Annotations;

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

        internal AnimeMangaNotificationCollection([NotNull] Senpai senpai, int maxNotificationsToParse = -1)
        {
            this._senpai = senpai;
            this._maxNotificationsToParse = maxNotificationsToParse;
        }

        #region Inherited

        /// <summary>
        /// </summary>
        public NotificationType Type => typeof(T) == typeof(Anime) ? NotificationType.Anime : NotificationType.Manga;

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public INotificationEnumerator<AnimeMangaNotification<T>> GetEnumerator()
        {
            return new AnimeMangaNotificationEnumerator<T>(this._senpai, this._maxNotificationsToParse);
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<AnimeMangaNotification<T>> IEnumerable<AnimeMangaNotification<T>>.
            GetEnumerator()
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