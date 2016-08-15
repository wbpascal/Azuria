using System.Collections.Generic;
using Azuria.AnimeManga;
using Azuria.Exceptions;
using JetBrains.Annotations;

namespace Azuria.User.ControlPanel
{
    /// <summary>
    ///     Represents the User-Control-Panel of a specified user.
    /// </summary>
    public class UserControlPanel
    {
        private readonly Senpai _senpai;

        /// <summary>
        ///     Inititalises a new instance of the <see cref="UserControlPanel" /> class with a specified user.
        /// </summary>
        /// <exception cref="NotLoggedInException">Raised when <paramref name="senpai" /> is not logged in.</exception>
        /// <param name="senpai">The user that owns this User-Control-Panel.</param>
        public UserControlPanel([NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            if (!this._senpai.IsProbablyLoggedIn) throw new NotLoggedInException(this._senpai);
        }

        #region Properties

        /// <summary>
        ///     Gets all bookmarks of the user that are <see cref="Anime">Anime</see>.
        /// </summary>
        [NotNull]
        public IEnumerable<AnimeMangaBookmarkObject<Anime>> AnimeBookmarks
            => new BookmarkEnumerable<Anime>(this._senpai, this);

        /// <summary>
        ///     Gets all bookmarks of the user that are <see cref="Manga">Manga</see>.
        /// </summary>
        [NotNull]
        public IEnumerable<AnimeMangaBookmarkObject<Manga>> MangaBookmarks
            => new BookmarkEnumerable<Manga>(this._senpai, this);

        #endregion
    }
}