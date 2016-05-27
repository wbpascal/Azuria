using System.Threading.Tasks;
using Azuria.Main.Minor;
using Azuria.Main.User.ControlPanel;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.Main
{
    /// <summary>
    ///     Represents an <see cref="Anime.Episode" /> or <see cref="Manga.Chapter" />.
    /// </summary>
    /// <typeparam name="T">The type of the parent object. Either an <see cref="Anime" /> or <see cref="Manga" />.</typeparam>
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
        [NotNull]
        T ParentObject { get; }

        #endregion

        #region

        /// <summary>
        ///     Adds the <see cref="Anime.Episode" /> or <see cref="Manga.Chapter" /> to the bookmarks. If
        ///     <paramref name="userControlPanel" /> is specified the object is also added to the corresponding
        ///     <see cref="UserControlPanel.AnimeBookmarks" />- or <see cref="UserControlPanel.MangaBookmarks" />-enumeration.
        /// </summary>
        /// <param name="userControlPanel">The object which, if specified, this object is added to.</param>
        /// <returns>If the action was successful.</returns>
        [ItemNotNull]
        Task<ProxerResult<AnimeMangaBookmarkObject<T>>> AddToBookmarks(UserControlPanel userControlPanel = null);

        #endregion
    }
}