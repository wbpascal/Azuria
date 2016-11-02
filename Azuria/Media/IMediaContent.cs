using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Media.Properties;

namespace Azuria.Media
{
    /// <summary>
    /// Represents an <see cref="Anime.Episode" /> or <see cref="Manga.Chapter" />.
    /// </summary>
    /// <typeparam name="T">The type of the parent object. Either an <see cref="Anime" /> or <see cref="Manga" />.</typeparam>
    // ReSharper disable once TypeParameterCanBeVariant
    public interface IMediaContent<T> : IMediaContent where T : IMediaObject
    {
        #region Properties

        /// <summary>
        /// Gets the <see cref="Anime" /> or <see cref="Manga" /> this <see cref="Anime.Episode" /> or
        /// <see cref="Manga.Chapter" /> belongs to.
        /// </summary>
        new T ParentObject { get; }

        #endregion
    }

    /// <summary>
    /// </summary>
    public interface IMediaContent
    {
        #region Properties

        /// <summary>
        /// Gets the <see cref="Anime.Episode" />- or <see cref="Manga.Chapter" />-number.
        /// </summary>
        int ContentIndex { get; }

        /// <summary>
        /// Gets whether the language of the <see cref="Anime.Episode" /> or <see cref="Manga.Chapter" /> is
        /// <see cref="Language.English">english</see> or <see cref="Language.German">german</see>.
        /// </summary>
        Language GeneralLanguage { get; }

        /// <summary>
        /// </summary>
        IMediaObject ParentObject { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the <see cref="Anime.Episode" /> or <see cref="Manga.Chapter" /> to the bookmarks.
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns>If the action was successful.</returns>
        Task<ProxerResult> AddToBookmarks(Senpai senpai);

        #endregion
    }
}