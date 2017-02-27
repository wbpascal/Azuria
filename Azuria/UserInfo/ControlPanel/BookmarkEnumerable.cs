using Azuria.Enumerable;
using Azuria.Media;

namespace Azuria.UserInfo.ControlPanel
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BookmarkEnumerable<T> : PagedEnumerable<Bookmark<T>> where T : IMediaObject
    {
        private readonly UserControlPanel _controlPanel;
        private readonly Senpai _senpai;

        internal BookmarkEnumerable(Senpai senpai, UserControlPanel controlPanel)
        {
            this._senpai = senpai;
            this._controlPanel = controlPanel;
        }

        #region Methods

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public override PagedEnumerator<Bookmark<T>> GetEnumerator()
        {
            return new BookmarkEnumerator<T>(this._senpai, this._controlPanel);
        }

        #endregion
    }
}