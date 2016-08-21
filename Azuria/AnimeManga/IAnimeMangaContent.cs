using Azuria.AnimeManga.Properties;

namespace Azuria.AnimeManga
{
    /// <summary>
    ///     Represents an <see cref="Anime.Episode" /> or <see cref="Manga.Chapter" />.
    /// </summary>
    /// <typeparam name="T">The type of the parent object. Either an <see cref="Anime" /> or <see cref="Manga" />.</typeparam>
    // ReSharper disable once TypeParameterCanBeVariant
    public interface IAnimeMangaContent<T> : IAnimeMangaContentBase where T : IAnimeMangaObject
    {
        #region Properties

        /// <summary>
        ///     Gets whether the language of the <see cref="Anime.Episode" /> or <see cref="Manga.Chapter" /> is
        ///     <see cref="Language.English">english</see> or <see cref="Language.German">german</see>.
        /// </summary>
        Language GeneralLanguage { get; }

        /// <summary>
        ///     Gets the <see cref="Anime" /> or <see cref="Manga" /> this <see cref="Anime.Episode" /> or
        ///     <see cref="Manga.Chapter" /> belongs to.
        /// </summary>
        T ParentObject { get; }

        #endregion
    }
}