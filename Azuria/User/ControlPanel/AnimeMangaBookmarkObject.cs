using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.User.ControlPanel
{
    /// <summary>
    ///     Represents an <see cref="Anime" /> or <see cref="Manga" /> the user has bookmarked.
    /// </summary>
    /// <typeparam name="T">Specifies if the bookmark is an <see cref="Anime" /> or <see cref="Manga" />.</typeparam>
    public class AnimeMangaBookmarkObject<T> where T : IAnimeMangaObject
    {
        internal AnimeMangaBookmarkObject(IAnimeMangaContent<T> animeMangaContentObject, int bookmarkId,
            UserControlPanel controlPanel)
        {
            this.AnimeMangaContentObject = animeMangaContentObject;
            this.BookmarkId = bookmarkId;
            this.UserControlPanel = controlPanel;
        }

        #region Properties

        /// <summary>
        ///     Gets the <see cref="Anime.Episode" /> or <see cref="Manga.Chapter" /> the user has bookmarked.
        /// </summary>
        [NotNull]
        public IAnimeMangaContent<T> AnimeMangaContentObject { get; }

        /// <summary>
        ///     Gets the id of this bookmark.
        /// </summary>
        public int BookmarkId { get; }

        /// <summary>
        /// </summary>
        public UserControlPanel UserControlPanel { get; }

        #endregion

        #region

        /// <summary>
        ///     Deletes the entry from the User-Control-Panel.
        /// </summary>
        /// <returns>If the action was successfull.</returns>
        [ItemNotNull]
        public Task<ProxerResult> DeleteEntry()
        {
            return this.UserControlPanel.DeleteBookmark(this);
        }

        #endregion
    }
}