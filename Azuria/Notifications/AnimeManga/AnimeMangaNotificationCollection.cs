using System.Collections;
using System.Collections.Generic;
using Azuria.AnimeManga;

namespace Azuria.Notifications.AnimeManga
{
    /// <summary>
    ///     Represents a collection of <see cref="Anime" />- and <see cref="Manga" />-notifications.
    /// </summary>
    public class AnimeMangaNotificationCollection<T> : IEnumerable<AnimeMangaNotification<T>>
        where T : IAnimeMangaObject
    {
        private readonly int _nodesToParse;
        private readonly Senpai _senpai;

        public AnimeMangaNotificationCollection(Senpai senpai, int nodesToParse = 0)
        {
            this._senpai = senpai;
            this._nodesToParse = nodesToParse;
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
        public IEnumerator<AnimeMangaNotification<T>> GetEnumerator()
        {
            return new AnimeMangaNotificationEnumerator<T>(this._senpai, this._nodesToParse);
        }

        #endregion
    }
}