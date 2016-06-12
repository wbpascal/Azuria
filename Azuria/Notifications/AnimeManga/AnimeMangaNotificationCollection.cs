using System.Collections;
using System.Collections.Generic;
using Azuria.Main;
using JetBrains.Annotations;

namespace Azuria.Notifications.AnimeManga
{
    /// <summary>
    ///     Represents a collection of <see cref="Anime" />- and <see cref="Manga" />-notifications.
    /// </summary>
    public class AnimeMangaNotificationCollection<T> : INotificationCollection<AnimeMangaNotification<T>>
        where T : IAnimeMangaObject
    {
        private readonly int _maxNotificationsToParse;
        private readonly Senpai _senpai;

        internal AnimeMangaNotificationCollection([NotNull] Senpai senpai, int maxNotificationsToParse = -1)
        {
            this._senpai = senpai;
            this._maxNotificationsToParse = maxNotificationsToParse;
            this.Type = NotificationType.AnimeManga;
        }

        #region Geerbt

        /// <summary>
        ///     Gets the type of the notifications.
        /// </summary>
        public NotificationType Type { get; }

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