using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.Enums;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Media;
using Azuria.Media.Properties;
using Azuria.Utilities.Properties;

namespace Azuria.UserInfo.ControlPanel
{
    /// <summary>
    /// Represents the User-Control-Panel of a specified user.
    /// </summary>
    public class UserControlPanel
    {
        private readonly InitialisableProperty<IEnumerable<CommentVote>> _commentVotes;
        private readonly InitialisableProperty<int> _pointsAnime;
        private readonly InitialisableProperty<int> _pointsManga;
        private readonly Senpai _senpai;
        private readonly InitialisableProperty<IEnumerable<ToptenObject<Anime>>> _toptenAnime;
        private readonly InitialisableProperty<IEnumerable<ToptenObject<Manga>>> _toptenManga;

        /// <summary>
        /// Inititalises a new instance of the <see cref="UserControlPanel" /> class with a specified user.
        /// </summary>
        /// <exception cref="NotLoggedInException">Raised when <paramref name="senpai" /> is not logged in.</exception>
        /// <param name="senpai">The user that owns this User-Control-Panel.</param>
        public UserControlPanel(Senpai senpai)
        {
            this._senpai = senpai;
            if (this._senpai == null) throw new ArgumentException(nameof(senpai));
            if (!this._senpai.IsProbablyLoggedIn) throw new NotLoggedInException(this._senpai);

            this._commentVotes = new InitialisableProperty<IEnumerable<CommentVote>>(this.InitVotes);
            this._pointsAnime = new InitialisableProperty<int>(() => this.InitPoints("anime"));
            this._pointsManga = new InitialisableProperty<int>(() => this.InitPoints("manga"));
            this._toptenAnime = new InitialisableProperty<IEnumerable<ToptenObject<Anime>>>(this.InitTopten);
            this._toptenManga = new InitialisableProperty<IEnumerable<ToptenObject<Manga>>>(this.InitTopten);
        }

        #region Properties

        /// <summary>
        /// Gets all bookmarks of the user that are <see cref="Anime">Anime</see>.
        /// </summary>
        public BookmarkEnumerable<Anime> BookmarksAnime
            => new BookmarkEnumerable<Anime>(this._senpai, this);

        /// <summary>
        /// Gets all bookmarks of the user that are <see cref="Manga">Manga</see>.
        /// </summary>
        public BookmarkEnumerable<Manga> BookmarksManga
            => new BookmarkEnumerable<Manga>(this._senpai, this);

        /// <summary>
        /// </summary>
        public IInitialisableProperty<IEnumerable<CommentVote>> CommentVotes => this._commentVotes;

        /// <summary>
        /// </summary>
        public HistoryEnumerable<IMediaObject> History
            => new HistoryEnumerable<IMediaObject>(this._senpai, this);

        /// <summary>
        /// </summary>
        public IInitialisableProperty<int> PointsAnime => this._pointsAnime;

        /// <summary>
        /// </summary>
        public IInitialisableProperty<int> PointsManga => this._pointsManga;

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
        /// <param name="contentObject"></param>
        /// <returns></returns>
        public async Task<IProxerResult> AddToBookmarks(IMediaContent contentObject)
        {
            if (contentObject == null) return new ProxerResult(new ArgumentNullException(nameof(contentObject)));

            ProxerApiResponse lResult = await RequestHandler.ApiRequest(
                    ApiRequestBuilder.UcpSetBookmark(contentObject.ParentObject.Id,
                        contentObject.ContentIndex,
                        (contentObject as Anime.Episode)?.Language.ToString().ToLowerInvariant() ??
                        (contentObject.GeneralLanguage == Language.German ? "de" : "en"),
                        contentObject.ParentObject.GetType().GetTypeInfo().Name.ToLowerInvariant(), this._senpai))
                .ConfigureAwait(false);

            return lResult.Success ? new ProxerResult() : new ProxerResult(lResult.Exceptions);
        }

        /// <summary>
        /// </summary>
        /// <param name="entryId"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<IProxerResult> AddToProfileList(int entryId, MediaProfileList list)
        {
            if (entryId < 0) return new ProxerResult(new ArgumentOutOfRangeException(nameof(entryId)));

            ProxerApiResponse lResult = await RequestHandler.ApiRequest(
                    ApiRequestBuilder.InfoSetUserInfo(entryId, ProfileListToString(list), this._senpai))
                .ConfigureAwait(false);
            return lResult.Success ? new ProxerResult() : new ProxerResult(lResult.Exceptions);
        }

        /// <summary>
        /// </summary>
        /// <param name="bookmarkId"></param>
        /// <returns></returns>
        public async Task<IProxerResult> DeleteBookmark(int bookmarkId)
        {
            if (bookmarkId < 0) return new ProxerResult(new ArgumentOutOfRangeException(nameof(bookmarkId)));

            ProxerApiResponse<BookmarkDataModel[]> lResult = await RequestHandler.ApiRequest(
                    ApiRequestBuilder.UcpDeleteReminder(bookmarkId, this._senpai))
                .ConfigureAwait(false);
            return lResult.Success ? new ProxerResult() : new ProxerResult(lResult.Exceptions);
        }

        /// <summary>
        /// </summary>
        /// <param name="voteId"></param>
        /// <returns></returns>
        public async Task<IProxerResult> DeleteCommentVote(int voteId)
        {
            if (voteId < 0) return new ProxerResult(new ArgumentOutOfRangeException(nameof(voteId)));

            ProxerApiResponse<BookmarkDataModel[]> lResult = await RequestHandler.ApiRequest(
                    ApiRequestBuilder.UcpDeleteVote(voteId, this._senpai))
                .ConfigureAwait(false);
            return lResult.Success ? new ProxerResult() : new ProxerResult(lResult.Exceptions);
        }

        /// <summary>
        /// </summary>
        /// <param name="toptenId"></param>
        /// <returns></returns>
        public async Task<IProxerResult> DeleteTopten(int toptenId)
        {
            if (toptenId < 0) return new ProxerResult(new ArgumentOutOfRangeException(nameof(toptenId)));

            ProxerApiResponse<BookmarkDataModel[]> lResult = await RequestHandler.ApiRequest(
                    ApiRequestBuilder.UcpDeleteFavourite(toptenId, this._senpai))
                .ConfigureAwait(false);
            return lResult.Success ? new ProxerResult() : new ProxerResult(lResult.Exceptions);
        }

        private async Task<IProxerResult> InitPoints(string category)
        {
            ProxerApiResponse<int> lResult = await RequestHandler.ApiRequest(
                    ApiRequestBuilder.UcpGetListsum(this._senpai, category))
                .ConfigureAwait(false);
            if (!lResult.Success) return new ProxerResult(lResult.Exceptions);

            switch (category)
            {
                case "anime":
                    this._pointsAnime.Set(lResult.Result);
                    break;
                case "manga":
                    this._pointsManga.Set(lResult.Result);
                    break;
            }

            return new ProxerResult();
        }

        private async Task<IProxerResult> InitTopten()
        {
            ProxerApiResponse<ToptenDataModel[]> lResult = await RequestHandler.ApiRequest(
                ApiRequestBuilder.UcpGetTopten(this._senpai)).ConfigureAwait(false);
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);

            ToptenDataModel[] lData = lResult.Result;
            this._toptenAnime.Set(from toptenDataModel in lData
                where toptenDataModel.EntryType == MediaEntryType.Anime
                select new ToptenObject<Anime>(toptenDataModel.ToptenId, new Anime(toptenDataModel), this));
            this._toptenManga.Set(from toptenDataModel in lData
                where toptenDataModel.EntryType == MediaEntryType.Manga
                select new ToptenObject<Manga>(toptenDataModel.ToptenId, new Manga(toptenDataModel), this));

            return new ProxerResult();
        }

        private async Task<IProxerResult> InitVotes()
        {
            ProxerApiResponse<VoteDataModel[]> lResult = await RequestHandler.ApiRequest(
                ApiRequestBuilder.UcpGetVotes(this._senpai)).ConfigureAwait(false);
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);

            this._commentVotes.Set(from voteDataModel in lResult.Result
                select new CommentVote(voteDataModel, this));
            return new ProxerResult();
        }

        private static string ProfileListToString(MediaProfileList list)
        {
            switch (list)
            {
                case MediaProfileList.Favourites:
                    return "favor";
                case MediaProfileList.Finished:
                    return "finish";
                case MediaProfileList.Noted:
                    return "note";
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}