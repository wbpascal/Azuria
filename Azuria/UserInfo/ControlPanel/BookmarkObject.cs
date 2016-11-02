using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Media;

namespace Azuria.UserInfo.ControlPanel
{
    /// <summary>
    /// Represents an <see cref="Anime.Episode" /> or <see cref="Manga.Chapter" /> the user has bookmarked.
    /// </summary>
    /// <typeparam name="T">Specifies if the bookmark is an <see cref="Anime" /> or <see cref="Manga" />.</typeparam>
    public class BookmarkObject<T> where T : IMediaObject
    {
        internal BookmarkObject(IMediaContent<T> mediaContentObject, int bookmarkId,
            UserControlPanel controlPanel)
        {
            this.MediaContentObject = mediaContentObject;
            this.BookmarkId = bookmarkId;
            this.UserControlPanel = controlPanel;
        }

        #region Properties

        /// <summary>
        /// Gets the id of this bookmark.
        /// </summary>
        public int BookmarkId { get; }

        /// <summary>
        /// Gets the <see cref="Anime.Episode" /> or <see cref="Manga.Chapter" /> the user has bookmarked.
        /// </summary>
        public IMediaContent<T> MediaContentObject { get; }

        /// <summary>
        /// </summary>
        public UserControlPanel UserControlPanel { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes the entry from the User-Control-Panel.
        /// </summary>
        /// <returns>If the action was successfull.</returns>
        public Task<ProxerResult> DeleteReminder()
        {
            return this.UserControlPanel.DeleteBookmark(this.BookmarkId);
        }

        #endregion
    }
}