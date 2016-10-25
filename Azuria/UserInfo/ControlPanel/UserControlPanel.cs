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
        public IEnumerable<BookmarkObject<Anime>> BookmarksAnime
            => new BookmarkEnumerable<Anime>(this._senpai, this);

        /// <summary>
        /// Gets all bookmarks of the user that are <see cref="Manga">Manga</see>.
        /// </summary>
        public IEnumerable<BookmarkObject<Manga>> BookmarksManga
            => new BookmarkEnumerable<Manga>(this._senpai, this);

        /// <summary>
        /// </summary>
        public IInitialisableProperty<IEnumerable<CommentVote>> CommentVotes => this._commentVotes;

        /// <summary>
        /// </summary>
        public IEnumerable<HistoryObject<IMediaObject>> History
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
        public async Task<ProxerResult> AddToBookmarks(IMediaContent contentObject)
        {
            if (contentObject == null) return new ProxerResult(new ArgumentNullException(nameof(contentObject)));

            ProxerResult<ProxerApiResponse> lResult =
                await
                    RequestHandler.ApiRequest(ApiRequestBuilder.UcpSetBookmark(contentObject.ParentObject.Id,
                        contentObject.ContentIndex,
                        (contentObject as Anime.Episode)?.Language.ToString().ToLowerInvariant() ??
                        (contentObject.GeneralLanguage == Language.German ? "de" : "en"),
                        contentObject.ParentObject.GetType().GetTypeInfo().Name.ToLowerInvariant(), this._senpai));

            return lResult.Success ? new ProxerResult() : new ProxerResult(lResult.Exceptions);
        }

        /// <summary>
        /// </summary>
        /// <param name="mediaObject"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<ProxerResult> AddToProfileList(IMediaObject mediaObject, MediaProfileList list)
        {
            ProxerResult<ProxerApiResponse> lResult =
                await
                    RequestHandler.ApiRequest(ApiRequestBuilder.InfoSetUserInfo(mediaObject.Id,
                        ProfileListToString(list), this._senpai));
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

        private async Task<ProxerResult> InitPoints(string category)
        {
            ProxerResult<ProxerApiResponse<int>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.UcpGetListsum(this._senpai, category));
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);

            switch (category)
            {
                case "anime":
                    this._pointsAnime.SetInitialisedObject(lResult.Result.Data);
                    break;
                case "manga":
                    this._pointsManga.SetInitialisedObject(lResult.Result.Data);
                    break;
            }

            return new ProxerResult();
        }

        private async Task<ProxerResult> InitTopten()
        {
            ProxerResult<ProxerApiResponse<ToptenDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.UcpGetTopten(this._senpai));
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);

            ToptenDataModel[] lData = lResult.Result.Data;
            this._toptenAnime.SetInitialisedObject(from toptenDataModel in lData
                where toptenDataModel.EntryType == MediaEntryType.Anime
                select new ToptenObject<Anime>(toptenDataModel.ToptenId, new Anime(toptenDataModel), this));
            this._toptenManga.SetInitialisedObject(from toptenDataModel in lData
                where toptenDataModel.EntryType == MediaEntryType.Manga
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