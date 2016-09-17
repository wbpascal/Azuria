using System.Threading.Tasks;
using Azuria.ErrorHandling;

namespace Azuria.Media
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

        #region Methods

        /// <summary>
        ///     Adds the <see cref="Anime.Episode" /> or <see cref="Manga.Chapter" /> to the bookmarks.
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns>If the action was successful.</returns>
        Task<ProxerResult> AddToBookmarks(Senpai senpai);

        #endregion
    }
}