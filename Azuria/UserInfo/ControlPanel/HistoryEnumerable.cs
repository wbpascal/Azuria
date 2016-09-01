using System.Collections;
using System.Collections.Generic;
using Azuria.AnimeManga;

namespace Azuria.UserInfo.ControlPanel
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HistoryEnumerable<T> : IEnumerable<HistoryObject<T>> where T : IAnimeMangaObject
    {
        private readonly Senpai _senpai;
        private readonly UserControlPanel _userControlPanel;

        internal HistoryEnumerable(Senpai senpai, UserControlPanel userControlPanel)
        {
            this._senpai = senpai;
            this._userControlPanel = userControlPanel;
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
        public IEnumerator<HistoryObject<T>> GetEnumerator()
        {
            return new HistoryEnumerator<T>(this._senpai, this._userControlPanel);
        }

        #endregion
    }
}