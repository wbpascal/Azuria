using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.AnimeManga.Properties;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.Enums;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Utilities.Properties;

namespace Azuria.UserInfo.ControlPanel
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
        public UserControlPanel(Senpai senpai)
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
        public IEnumerable<BookmarkObject<Anime>> BookmarksAnime
            => new BookmarkEnumerable<Anime>(this._senpai, this);

        /// <summary>
        ///     Gets all bookmarks of the user that are <see cref="Manga">Manga</see>.
        /// </summary>
        public IEnumerable<BookmarkObject<Manga>> BookmarksManga
            => new BookmarkEnumerable<Manga>(this._senpai, this);

        /// <summary>
        /// </summary>
        public IInitialisableProperty<IEnumerable<CommentVote>> CommentVotes => this._commentVotes;

        /// <summary>
        /// </summary>
        public IEnumerable<HistoryObject<IAnimeMangaObject>> History
            => new HistoryEnumerable<IAnimeMangaObject>(this._senpai, this);

        /// <summary>
        /// </summary>
        public IInitialisableProperty<IEnumerable<ToptenObject<Anime>>> ToptenAnime => this._toptenAnime;

        /// <summary>
        /// </summary>
        public IInitialisableProperty<IEnumerable<ToptenObject<Manga>>> ToptenManga => this._toptenManga;

        #endregion

        #region Methods

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
        /// <param name="bookmarkId"></param>
        /// <returns></returns>
        public async Task<ProxerResult> DeleteBookmark(int bookmarkId)
        {
            ProxerResult<ProxerApiResponse<BookmarkDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.UcpDeleteReminder(bookmarkId, this._senpai));
            return lResult.Success ? new ProxerResult() : new ProxerResult(lResult.Exceptions);
        }

        /// <summary>
        /// </summary>
        /// <param name="voteId"></param>
        /// <returns></returns>
        public async Task<ProxerResult> DeleteCommentVote(int voteId)
        {
            ProxerResult<ProxerApiResponse<BookmarkDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.UcpDeleteVote(voteId, this._senpai));
            return lResult.Success ? new ProxerResult() : new ProxerResult(lResult.Exceptions);
        }

        /// <summary>
        /// </summary>
        /// <param name="toptenId"></param>
        /// <returns></returns>
        public async Task<ProxerResult> DeleteTopten(int toptenId)
        {
            ProxerResult<ProxerApiResponse<BookmarkDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.UcpDeleteFavourite(toptenId, this._senpai));
            return lResult.Success ? new ProxerResult() : new ProxerResult(lResult.Exceptions);
        }

        private async Task<ProxerResult> InitTopten()
        {
            ProxerResult<ProxerApiResponse<ToptenDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.UcpGetTopten(this._senpai));
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);

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
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);

            this._commentVotes.SetInitialisedObject(from voteDataModel in lResult.Result.Data
                select new CommentVote(voteDataModel, this));
            return new ProxerResult();
        }

        #endregion
    }
}