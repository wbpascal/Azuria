using System.Collections;
using System.Collections.Generic;
using Azuria.AnimeManga;

namespace Azuria.User.ControlPanel
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BookmarkEnumerable<T> : IEnumerable<BookmarkObject<T>> where T : class, IAnimeMangaObject
    {
        private readonly UserControlPanel _controlPanel;
        private readonly Senpai _senpai;

        internal BookmarkEnumerable(Senpai senpai, UserControlPanel controlPanel)
        {
            this._senpai = senpai;
            this._controlPanel = controlPanel;
        }

        #region Inherited

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<BookmarkObject<T>> GetEnumerator()
        {
            return new BookmarkEnumerator<T>(this._senpai, this._controlPanel);
        }

        #endregion
    }
}