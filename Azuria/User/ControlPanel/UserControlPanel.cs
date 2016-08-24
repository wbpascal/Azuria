using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.AnimeManga.Properties;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.Enums;
using Azuria.Exceptions;
using Azuria.User.Comment;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Properties;
using JetBrains.Annotations;

namespace Azuria.User.ControlPanel
{
    /// <summary>
    ///     Represents the User-Control-Panel of a specified user.
    ///     TODO: Implement Listsum
    /// </summary>
    public class UserControlPanel
    {
        private readonly InitialisableProperty<IEnumerable<CommentVote>> _commentVotes;
        private readonly Senpai _senpai;
        private readonly InitialisableProperty<IEnumerable<ToptenObject<Anime>>> _toptenAnime;
        private readonly InitialisableProperty<IEnumerable<ToptenObject<Manga>>> _toptenManga;

        /// <summary>
        ///     Inititalises a new instance of the <see cref="UserControlPanel" /> class with a specified user.
        /// </summary>
        /// <exception cref="NotLoggedInException">Raised when <paramref name="senpai" /> is not logged in.</exception>
        /// <param name="senpai">The user that owns this User-Control-Panel.</param>
        public UserControlPanel([NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            if (!this._senpai.IsProbablyLoggedIn) throw new NotLoggedInException(this._senpai);

            this._commentVotes = new InitialisableProperty<IEnumerable<CommentVote>>(this.InitVotes);
            this._toptenAnime = new InitialisableProperty<IEnumerable<ToptenObject<Anime>>>(this.InitTopten);
            this._toptenManga = new InitialisableProperty<IEnumerable<ToptenObject<Manga>>>(this.InitTopten);
        }

        #region Properties

        /// <summary>
        ///     Gets all bookmarks of the user that are <see cref="Anime">Anime</see>.
        /// </summary>
        [NotNull]
        public IEnumerable<BookmarkObject<Anime>> BookmarksAnime
            => new BookmarkEnumerable<Anime>(this._senpai, this);

        /// <summary>
        ///     Gets all bookmarks of the user that are <see cref="Manga">Manga</see>.
        /// </summary>
        [NotNull]
        public IEnumerable<BookmarkObject<Manga>> BookmarksManga
            => new BookmarkEnumerable<Manga>(this._senpai, this);

        /// <summary>
        /// </summary>
        public IInitialisableProperty<IEnumerable<CommentVote>> CommentVotes => this._commentVotes;

        /// <summary>
        /// </summary>
        [NotNull]
        public IEnumerable<HistoryObject<IAnimeMangaObject>> History
            => new HistoryEnumerable<IAnimeMangaObject>(this._senpai, this);

        /// <summary>
        /// </summary>
        [NotNull]
        public IInitialisableProperty<IEnumerable<ToptenObject<Anime>>> ToptenAnime => this._toptenAnime;

        /// <summary>
        /// </summary>
        [NotNull]
        public IInitialisableProperty<IEnumerable<ToptenObject<Manga>>> ToptenManga => this._toptenManga;

        #endregion

        #region

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="contentObject"></param>
        /// <returns></returns>
        public async Task<ProxerResult> AddToBookmarks<T>(IAnimeMangaContent<T> contentObject)
            where T : class, IAnimeMangaObject
        {
            ProxerResult<ProxerApiResponse> lResult =
                await
                    RequestHandler.ApiRequest(ApiRequestBuilder.UcpSetBookmark(contentObject.ParentObject.Id,
                        contentObject.ContentIndex,
                        (contentObject as Anime.Episode)?.Language.ToString().ToLowerInvariant() ??
                        (contentObject.GeneralLanguage == Language.German ? "de" : "en"),
                        typeof(T).GetTypeInfo().Name.ToLowerInvariant(), this._senpai));

            return lResult.Success ? new ProxerResult() : new ProxerResult(lResult.Exceptions);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bookmark"></param>
        /// <returns></returns>
        public async Task<ProxerResult> DeleteBookmark<T>(BookmarkObject<T> bookmark)
            where T : IAnimeMangaObject
        {
            if ((bookmark.UserControlPanel._senpai.Me?.Id ?? -1) != (this._senpai.Me?.Id ?? -1))
                return new ProxerResult(new[] {new ArgumentException(nameof(bookmark))});

            ProxerResult<ProxerApiResponse<BookmarkDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.UcpDeleteReminder(bookmark.BookmarkId, this._senpai));
            return lResult.Success ? new ProxerResult() : new ProxerResult(new Exception[0]);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topten"></param>
        /// <returns></returns>
        public async Task<ProxerResult> DeleteTopten<T>(ToptenObject<T> topten)
            where T : IAnimeMangaObject
        {
            if ((topten.UserControlPanel._senpai.Me?.Id ?? -1) != (this._senpai.Me?.Id ?? -1))
                return new ProxerResult(new[] {new ArgumentException(nameof(topten))});

            ProxerResult<ProxerApiResponse<BookmarkDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.UcpDeleteFavourite(topten.ToptenId, this._senpai));
            return lResult.Success ? new ProxerResult() : new ProxerResult(new Exception[0]);
        }

        private async Task<ProxerResult> InitTopten()
        {
            ProxerResult<ProxerApiResponse<ToptenDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.UcpGetTopten(this._senpai));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);

            ToptenDataModel[] lData = lResult.Result.Data;
            this._toptenAnime.SetInitialisedObject(from toptenDataModel in lData
                where toptenDataModel.EntryType == AnimeMangaEntryType.Anime
                select new ToptenObject<Anime>(toptenDataModel.ToptenId, new Anime(toptenDataModel), this));
            this._toptenManga.SetInitialisedObject(from toptenDataModel in lData
                where toptenDataModel.EntryType == AnimeMangaEntryType.Manga
                select new ToptenObject<Manga>(toptenDataModel.ToptenId, new Manga(toptenDataModel), this));

            return new ProxerResult();
        }

        private async Task<ProxerResult> InitVotes()
        {
            ProxerResult<ProxerApiResponse<VoteDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.UcpGetVotes(this._senpai));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);

            this._commentVotes.SetInitialisedObject(from voteDataModel in lResult.Result.Data
                select new CommentVote(voteDataModel, this));
            return new ProxerResult();
        }

        #endregion
    }
}