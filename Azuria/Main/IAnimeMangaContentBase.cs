using Azuria.Utilities.Properties;

namespace Azuria.Main
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

        /// <summary>
        ///     Gets if the <see cref="Anime.Episode" /> or <see cref="Manga.Chapter" /> is available.
        /// </summary>
        InitialisableProperty<bool> IsAvailable { get; }

        #endregion
    }
}