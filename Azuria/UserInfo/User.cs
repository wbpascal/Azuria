using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.Enums;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Media;
using Azuria.UserInfo.Comment;
using Azuria.Utilities.Properties;

namespace Azuria.UserInfo
{
    /// <summary>
    /// Represents a user of proxer.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Represents the system as a user.
        /// </summary>
        public static User System = new User("System", default(int));

        private readonly InitialisableProperty<Uri> _avatar;
        private readonly InitialisableProperty<UserPoints> _points;
        private readonly InitialisableProperty<UserStatus> _status;
        private readonly ArgumentInitialisableProperty<Senpai, IEnumerable<Anime>> _toptenAnime;
        private readonly ArgumentInitialisableProperty<Senpai, IEnumerable<Manga>> _toptenManga;
        private readonly InitialisableProperty<string> _userName;

        /// <summary>
        /// Initialises a new instance of the class.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        public User(int userId)
        {
            if (userId < 0) throw new ArgumentOutOfRangeException(nameof(userId));

            this.Id = userId;

            this.Anime = new UserEntryEnumerable<Anime>(this);
            this._avatar = new InitialisableProperty<Uri>(
                this.InitMainInfo, new Uri(ApiConstants.ProxerNoAvatarCdnUrl))
            {
                IsInitialised = false
            };
            this._toptenAnime = new ArgumentInitialisableProperty<Senpai, IEnumerable<Anime>>(
                senpai => this.InitTopten(MediaEntryType.Anime, senpai));
            this.CommentsAnime = new CommentEnumerable<Anime>(this);
            this.CommentsManga = new CommentEnumerable<Manga>(this);
            this._toptenManga = new ArgumentInitialisableProperty<Senpai, IEnumerable<Manga>>(
                senpai => this.InitTopten(MediaEntryType.Manga, senpai));
            this.Manga = new UserEntryEnumerable<Manga>(this);
            this._points = new InitialisableProperty<UserPoints>(this.InitMainInfo);
            this._status = new InitialisableProperty<UserStatus>(this.InitMainInfo);
            this._userName = new InitialisableProperty<string>(this.InitMainInfo);
        }

        internal User(string name, int userId) : this(userId)
        {
            this._userName.Set(name);
        }

        internal User(int userId, Uri avatar)
            : this(userId)
        {
            this._avatar.Set(avatar ?? new Uri(ApiConstants.ProxerNoAvatarCdnUrl));
        }

        internal User(string name, int userId, Uri avatar)
            : this(name, userId)
        {
            this._avatar.Set(avatar ?? new Uri(ApiConstants.ProxerNoAvatarCdnUrl));
        }

        internal User(UserInfoDataModel dataModel)
            : this(
                dataModel.Username, dataModel.UserId, new Uri(ApiConstants.ProxerAvatarShortCdnUrl + dataModel.AvatarId)
            )
        {
            this._points.Set(dataModel.Points);
            this._status.Set(dataModel.Status);
        }

        internal User(ConferenceInfoParticipantDataModel dataModel)
            : this(
                dataModel.Username, dataModel.UserId, new Uri(ApiConstants.ProxerAvatarShortCdnUrl + dataModel.AvatarId)
            )
        {
            this._status.Set(new UserStatus(dataModel.UserStatus, DateTime.MinValue));
        }

        #region Properties

        /// <summary>
        /// Gets all <see cref="Media.Anime" /> the <see cref="User" /> has in his profile.
        /// </summary>
        public UserEntryEnumerable<Anime> Anime { get; }

        /// <summary>
        /// Gets the avatar of the user.
        /// </summary>
        public IInitialisableProperty<Uri> Avatar => this._avatar;

        /// <summary>
        /// </summary>
        public CommentEnumerable<Anime> CommentsAnime { get; }

        /// <summary>
        /// </summary>
        public CommentEnumerable<Manga> CommentsManga { get; }

        /// <summary>
        /// Gets the id of the user.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets all <see cref="Media.Manga" /> the <see cref="User" /> has in his profile.
        /// </summary>
        public UserEntryEnumerable<Manga> Manga { get; }

        /// <summary>
        /// Gets the current number of total points the user has.
        /// </summary>
        public IInitialisableProperty<UserPoints> Points => this._points;

        /// <summary>
        /// Gets the current status of the user.
        /// </summary>
        public IInitialisableProperty<UserStatus> Status => this._status;

        /// <summary>
        /// Gets all favourites of the user that are <see cref="Media.Anime">Anime</see>.
        /// </summary>
        public IArgumentInitialisableProperty<Senpai, IEnumerable<Anime>> ToptenAnime => this._toptenAnime;

        /// <summary>
        /// Gets the <see cref="Media.Manga">Manga</see> top ten of the user.
        /// </summary>
        public IArgumentInitialisableProperty<Senpai, IEnumerable<Manga>> ToptenManga => this._toptenManga;

        /// <summary>
        /// Gets the username of the user.
        /// </summary>
        public IInitialisableProperty<string> UserName => this._userName;

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static async Task<IProxerResult<User>> FromUsername(string username)
        {
            ProxerApiResponse<UserInfoDataModel> lResult = await RequestHandler.ApiRequest(
                ApiRequestBuilder.UserGetInfo(username)).ConfigureAwait(false);
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult<User>(lResult.Exceptions);
            return new ProxerResult<User>(new User(lResult.Result));
        }

        private async Task<IProxerResult> InitMainInfo()
        {
            if (this == System) return new ProxerResult(new InvalidUserException());

            ProxerApiResponse<UserInfoDataModel> lResult = await RequestHandler.ApiRequest(
                ApiRequestBuilder.UserGetInfo(this.Id)).ConfigureAwait(false);
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);

            UserInfoDataModel lDataModel = lResult.Result;
            this._avatar.Set(new Uri(ApiConstants.ProxerAvatarShortCdnUrl + lDataModel.AvatarId));
            this._points.Set(lDataModel.Points);
            this._status.Set(lDataModel.Status);
            this._userName.Set(lDataModel.Username);

            return new ProxerResult();
        }

        private async Task<IProxerResult> InitTopten(MediaEntryType category, Senpai senpai)
        {
            if (this == System) return new ProxerResult(new InvalidUserException());

            ProxerApiResponse<ToptenDataModel[]> lResult = await RequestHandler.ApiRequest(
                    ApiRequestBuilder.UserGetTopten(this.Id, category.ToString().ToLower(), senpai))
                .ConfigureAwait(false);
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);

            if (category == MediaEntryType.Anime)
                this._toptenAnime.SetInitialisedObject(from toptenDataModel in lResult.Result
                    select new Anime(toptenDataModel.EntryName, toptenDataModel.EntryId));
            if (category == MediaEntryType.Manga)
                this._toptenManga.SetInitialisedObject(from toptenDataModel in lResult.Result
                    select new Manga(toptenDataModel.EntryName, toptenDataModel.EntryId));

            return new ProxerResult();
        }

        #endregion
    }
}