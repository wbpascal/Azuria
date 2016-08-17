using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.Enums;
using Azuria.Exceptions;
using Azuria.Search;
using Azuria.User.Comment;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Properties;
using Azuria.Utilities.Web;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Azuria.User
{
    /// <summary>
    ///     Represents a user of proxer.
    /// </summary>
    public class User : ISearchableObject
    {
        /// <summary>
        ///     Represents the system as a user.
        /// </summary>
        [NotNull] public static User System = new User("System", -1, new Senpai("System"));

        /// <summary>
        ///     Initialises a new instance of the class.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="senpai">The user that makes the requests.</param>
        public User(int userId, [NotNull] Senpai senpai)
        {
            //TODO: Wait for update on official API. IsOnline, Ranking

            this.Senpai = senpai;
            this.Id = userId;

            this.Anime = new UserEntryEnumerable<Anime>(this, this.Senpai);
            this.Avatar = new InitialisableProperty<Uri>(this.InitMainInfo,
                new Uri("https://cdn.proxer.me/avatar/nophoto.png"))
            {
                IsInitialisedOnce = false
            };
            this.AnimeTopten =
                new InitialisableProperty<IEnumerable<Anime>>(() => this.InitTopten(AnimeMangaEntryType.Anime));
            this.MangaTopten =
                new InitialisableProperty<IEnumerable<Manga>>(() => this.InitTopten(AnimeMangaEntryType.Manga));
            this.IsOnline = new InitialisableProperty<bool>(this.InitMainInfo);
            this.Manga = new UserEntryEnumerable<Manga>(this, this.Senpai);
            this.Points = new InitialisableProperty<UserPoints>(this.InitMainInfo);
            this.Ranking = new InitialisableProperty<string>(this.InitMainInfo);
            this.Status = new InitialisableProperty<UserStatus>(this.InitMainInfo);
            this.UserName = new InitialisableProperty<string>(this.InitMainInfo);
        }

        internal User([NotNull] string name, int userId, [NotNull] Senpai senpai) : this(userId, senpai)
        {
            this.UserName.SetInitialisedObject(name);
        }

        internal User(int userId, [CanBeNull] Uri avatar, [NotNull] Senpai senpai)
            : this(userId, senpai)
        {
            this.Avatar.SetInitialisedObject(avatar ?? new Uri("https://cdn.proxer.me/avatar/nophoto.png"));
        }

        internal User([NotNull] string name, int userId, [CanBeNull] Uri avatar, [NotNull] Senpai senpai)
            : this(name, userId, senpai)
        {
            this.Avatar.SetInitialisedObject(avatar ?? new Uri("https://cdn.proxer.me/avatar/nophoto.png"));
        }

        internal User([NotNull] string name, int userId, [NotNull] Uri avatar, bool online, [NotNull] Senpai senpai)
            : this(name, userId, avatar, senpai)
        {
            this.IsOnline.SetInitialisedObject(online);
        }

        internal User(UserInfoDataModel dataModel, Senpai senpai)
            : this(
                dataModel.Username, dataModel.UserId, new Uri("http://cdn.proxer.me/avatar/" + dataModel.Avatar), senpai
                )
        {
            this.Points.SetInitialisedObject(dataModel.Points);
            this.Status.SetInitialisedObject(dataModel.Status);
        }

        #region Properties

        /// <summary>
        ///     Gets all <see cref="AnimeManga.Anime" /> the <see cref="User" /> has in his profile.
        /// </summary>
        [NotNull]
        public IEnumerable<UserProfileEntry<Anime>> Anime { get; }

        /// <summary>
        ///     Gets all favourites of the user that are <see cref="AnimeManga.Anime">Anime</see>.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<Anime>> AnimeTopten { get; }

        /// <summary>
        ///     Gets the avatar of the user.
        /// </summary>
        [NotNull]
        public InitialisableProperty<Uri> Avatar { get; }

        /// <summary>
        /// </summary>
        public IEnumerable<Comment<Anime>> CommentsLatestAnime => new CommentEnumerable<Anime>(this, this.Senpai);

        /// <summary>
        /// </summary>
        public IEnumerable<Comment<Manga>> CommentsLatestManga => new CommentEnumerable<Manga>(this, this.Senpai);

        /// <summary>
        ///     Gets the id of the user.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Gets whether the user is currently online.
        /// </summary>
        [NotNull]
        public InitialisableProperty<bool> IsOnline { get; }

        /// <summary>
        ///     Gets all <see cref="AnimeManga.Manga" /> the <see cref="User" /> has in his profile.
        /// </summary>
        [NotNull]
        public IEnumerable<UserProfileEntry<Manga>> Manga { get; }

        /// <summary>
        ///     Gets the <see cref="AnimeManga.Manga">Manga</see> top ten of the user.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<Manga>> MangaTopten { get; }

        /// <summary>
        ///     Gets the current number of total points the user has.
        /// </summary>
        [NotNull]
        public InitialisableProperty<UserPoints> Points { get; }

        /// <summary>
        ///     Gets the name of the rank the user is currently in.
        /// </summary>
        [NotNull]
        public InitialisableProperty<string> Ranking { get; }

        /// <summary>
        /// </summary>
        public Senpai Senpai { get; }

        /// <summary>
        ///     Gets the current status of the user.
        /// </summary>
        [NotNull]
        public InitialisableProperty<UserStatus> Status { get; }

        /// <summary>
        ///     Gets the username of the user.
        /// </summary>
        [NotNull]
        public InitialisableProperty<string> UserName { get; }

        #endregion

        #region

        /// <summary>
        ///     Initialises the object.
        /// </summary>
        [ItemNotNull]
        [Obsolete]
        public async Task<ProxerResult> Init()
        {
            return await this.InitAllInitalisableProperties();
        }

        [ItemNotNull]
        private async Task<ProxerResult> InitMainInfo()
        {
            if (this.Id == -1) return new ProxerResult();

            ProxerResult<ProxerApiResponse<UserInfoDataModel>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.UserGetInfo(this.Id, this.Senpai));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);

            UserInfoDataModel lDataModel = lResult.Result.Data;
            this.Avatar.SetInitialisedObject(new Uri("http://cdn.proxer.me/avatar/" + lDataModel.Avatar));
            this.Points.SetInitialisedObject(lDataModel.Points);
            this.Status.SetInitialisedObject(lDataModel.Status);
            this.UserName.SetInitialisedObject(lDataModel.Username);

            return new ProxerResult();
        }

        private async Task<ProxerResult> InitTopten(AnimeMangaEntryType category)
        {
            ProxerResult<ProxerApiResponse<ToptenDataModel[]>> lResult =
                await
                    RequestHandler.ApiRequest(ApiRequestBuilder.UserGetTopten(this.Id,
                        category.ToString().ToLower(), this.Senpai));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);

            if (category == AnimeMangaEntryType.Anime)
                this.AnimeTopten.SetInitialisedObject(from toptenDataModel in lResult.Result.Data
                    select new Anime(toptenDataModel.Name, toptenDataModel.EntryId, this.Senpai));
            if (category == AnimeMangaEntryType.Manga)
                this.MangaTopten.SetInitialisedObject(from toptenDataModel in lResult.Result.Data
                    select new Manga(toptenDataModel.Name, toptenDataModel.EntryId, this.Senpai));

            return new ProxerResult();
        }

        /// <summary>
        ///     Sends the user a friend request.
        /// </summary>
        /// <param name="senpai">The user that sends the friend request.</param>
        /// <returns>If the action was successful.</returns>
        [ItemNotNull]
        public async Task<ProxerResult> SendFriendRequest([NotNull] Senpai senpai)
        {
            if (this.Id == -1) return new ProxerResult(new[] {new InvalidUserException()});

            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"type", "addFriend"}
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.PostResponseErrorHandling(new Uri("https://proxer.me/user/" + this.Id + "?format=json"),
                        lPostArgs,
                        senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            try
            {
                Dictionary<string, string> lResultDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResult.Result);

                return new ProxerResult
                {
                    Success = lResultDictionary.ContainsKey("error") && lResultDictionary["error"].Equals("0")
                };
            }
            catch
            {
                return
                    new ProxerResult(ErrorHandler.HandleError(this.Senpai, lResult.Result, false).Exceptions);
            }
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return this.UserName.GetObjectIfInitialised("ERROR") + " [" + this.Id + "]";
        }

        #endregion
    }
}