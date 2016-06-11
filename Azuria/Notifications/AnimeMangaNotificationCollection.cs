using System.Collections;
using System.Collections.Generic;
using Azuria.Main;
using JetBrains.Annotations;

namespace Azuria.Notifications
{
    /// <summary>
    ///     Represents a collection of <see cref="Anime" />- and <see cref="Manga" />-notifications.
    /// </summary>
    public class AnimeMangaNotificationCollection : INotificationCollection<AnimeMangaNotification>
    {
        private readonly Senpai _senpai;

        internal AnimeMangaNotificationCollection([NotNull] Senpai senpai)
        {
            this._senpai = senpai;
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
        public INotificationEnumerator<AnimeMangaNotification> GetEnumerator()
        {
            return new AnimeMangaNotificationEnumerator(this._senpai);
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<AnimeMangaNotification> IEnumerable<AnimeMangaNotification>.GetEnumerator()
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