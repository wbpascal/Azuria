using System.Threading.Tasks;
using Azuria.User.ControlPanel;
using Azuria.Utilities.ErrorHandling;

namespace Azuria.AnimeManga
{
    /// <summary>
    ///     Represents an <see cref="Anime.Episode" /> or <see cref="Manga.Chapter" />.
    /// </summary>
    public interface IAnimeMangaContentBase
    {
        #region Properties

        /// <summary>
        ///     Gets the <see cref="Anime.Episode" />- or <see cref="Manga.Chapter" />-number.
        /// </summary>
        int ContentIndex { get; }

        #endregion

        #region

        /// <summary>
        ///     Adds the <see cref="Anime.Episode" /> or <see cref="Manga.Chapter" /> to the bookmarks. If
        ///     <paramref name="userControlPanel" /> is specified the object is also added to the corresponding
        ///     <see cref="UserControlPanel.BookmarksAnime" />- or <see cref="UserControlPanel.BookmarksManga" />-enumeration.
        /// </summary>
        /// <param name="userControlPanel">The object which, if specified, this object is added to.</param>
        /// <returns>If the action was successful.</returns>
        Task<ProxerResult> AddToBookmarks(UserControlPanel userControlPanel);

        #endregion
    }
}