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
    }
}