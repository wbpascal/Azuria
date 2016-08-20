using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.User.ControlPanel
{
    /// <summary>
    ///     Represents the User-Control-Panel of a specified user.
    ///     TODO: Implement Listsum
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
        /// </summary>
        public IEnumerable<AnimeMangaHistoryObject<IAnimeMangaObject>> History
            => new HistoryEnumerable<IAnimeMangaObject>(this._senpai, this);

        /// <summary>
        ///     Gets all bookmarks of the user that are <see cref="Manga">Manga</see>.
        /// </summary>
        [NotNull]
        public IEnumerable<AnimeMangaBookmarkObject<Manga>> MangaBookmarks
            => new BookmarkEnumerable<Manga>(this._senpai, this);

        #endregion

        #region

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bookmark"></param>
        /// <returns></returns>
        public async Task<ProxerResult> DeleteBookmark<T>(AnimeMangaBookmarkObject<T> bookmark)
            where T : IAnimeMangaObject
        {
            if ((bookmark.UserControlPanel._senpai.Me?.Id ?? -1) != (this._senpai.Me?.Id ?? -1))
                return new ProxerResult(new[] {new ArgumentException(nameof(bookmark))});

            ProxerResult<ProxerApiResponse<BookmarkDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.UcpDeleteReminder(bookmark.BookmarkId, this._senpai));
            return lResult.Success ? new ProxerResult() : new ProxerResult(new Exception[0]);
        }

        #endregion
    }
}