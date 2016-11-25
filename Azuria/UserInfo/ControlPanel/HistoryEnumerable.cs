using Azuria.Enumerable;
using Azuria.Media;

namespace Azuria.UserInfo.ControlPanel
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HistoryEnumerable<T> : PagedEnumerable<HistoryObject<T>> where T : IMediaObject
    {
        private readonly Senpai _senpai;
        private readonly UserControlPanel _userControlPanel;

        internal HistoryEnumerable(Senpai senpai, UserControlPanel userControlPanel)
        {
            this._senpai = senpai;
            this._userControlPanel = userControlPanel;
        }

        #region Methods

        /// <inheritdoc />
        public override PagedEnumerator<HistoryObject<T>> GetEnumerator()
        {
            return new HistoryEnumerator<T>(this._senpai, this._userControlPanel);
        }

        #endregion
    }
}