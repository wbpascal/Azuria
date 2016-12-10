using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Media;

namespace Azuria.UserInfo.ControlPanel
{
    /// <summary>
    /// Represents an <see cref="Anime.Episode" /> or <see cref="Manga.Chapter" /> the user has bookmarked.
    /// </summary>
    /// <typeparam name="T">Specifies if the bookmark is an <see cref="Anime" /> or <see cref="Manga" />.</typeparam>
    public class Bookmark<T> : IBookmark where T : IMediaObject
    {
        internal Bookmark(IMediaContent<T> mediaContentObject, int bookmarkId,
            UserControlPanel controlPanel)
        {
            this.MediaContentObject = mediaContentObject;
            this.BookmarkId = bookmarkId;
            this.UserControlPanel = controlPanel;
        }

        #region Properties

        /// <inheritdoc />
        public int BookmarkId { get; }

        /// <summary>
        /// Gets the <see cref="Anime.Episode" /> or <see cref="Manga.Chapter" /> the user has bookmarked.
        /// </summary>
        public IMediaContent<T> MediaContentObject { get; }

        IMediaContent IBookmark.MediaContentObject => this.MediaContentObject;

        /// <inheritdoc />
        public UserControlPanel UserControlPanel { get; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public Task<IProxerResult> Delete()
        {
            return this.UserControlPanel.DeleteBookmark(this.BookmarkId);
        }

        #endregion
    }
}