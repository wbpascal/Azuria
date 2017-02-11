using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Media;
using Azuria.UserInfo;
using Azuria.UserInfo.Comment;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Azuria.Test.UserTests
{
    public class UserTest
    {
        private User _user;

        #region Methods

        [Test]
        public void AnimeTest()
        {
            UserEntryEnumerable<Anime> lAnimeEnumerable = this._user.Anime;
            lAnimeEnumerable.Senpai = GeneralSetup.SenpaiInstance;
            UserProfileEntry<Anime>[] lAnime = lAnimeEnumerable.ToArray();
            Assert.AreEqual(6, lAnime.Length);
            Assert.IsTrue(lAnime.All(entry => entry != null));
            Assert.IsTrue(lAnime.All(entry => entry.Comment != null));
            Assert.IsTrue(lAnime.All(entry => entry.Comment.Author == this._user));
            Assert.AreEqual(1, lAnime.Count(entry => !string.IsNullOrEmpty(entry.Comment.Content)));
            Assert.IsTrue(lAnime.All(entry => entry.Comment.Id != default(int)));
            Assert.IsTrue(lAnime.All(entry => entry.Comment.MediaObject == entry.MediaObject));
            Assert.IsTrue(lAnime.All(entry => entry.Comment.Progress > 0));
            Assert.IsTrue(lAnime.All(entry => entry.Comment.ProgressState == MediaProgressState.Finished));
            Assert.AreEqual(1, lAnime.Count(entry =>
                    entry.Comment.Rating != default(int) && entry.Comment.SubRatings.Any()));
            Assert.IsTrue(lAnime.All(entry => entry.MediaObject != null));
            Assert.IsTrue(lAnime.All(entry => entry.MediaObject.Id != default(int)));
            Assert.IsTrue(lAnime.All(entry =>
                    !string.IsNullOrEmpty(entry.MediaObject.Name.GetIfInitialised(string.Empty))));
            Assert.IsTrue(lAnime.All(entry => entry.User == this._user));
        }

        [Test]
        public async Task AvatarTest()
        {
            Assert.CatchAsync<InvalidUserException>(() => User.System.Points.ThrowFirstOnNonSuccess());

            IProxerResult<Uri> lResult = await this._user.Avatar.Get();
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsTrue(lResult.Result.AbsoluteUri.StartsWith("https://cdn.proxer.me/avatar/"));
        }

        [Test]
        public void CommentsAnimeTest()
        {
            CommentEnumerable<Anime> lCommentEnumerable = this._user.CommentsAnime;
            lCommentEnumerable.Senpai = GeneralSetup.SenpaiInstance;
            Comment<Anime>[] lComments = lCommentEnumerable.ToArray();
            Assert.IsNotEmpty(lComments);
            Assert.IsTrue(lComments.All(comment => comment.Author == this._user));
            Assert.IsTrue(lComments.All(comment => !string.IsNullOrEmpty(comment.Content)));
            Assert.IsTrue(lComments.All(comment => comment.Id != default(int)));
            Assert.IsTrue(lComments.All(comment => comment.MediaObject != null));
            Assert.IsTrue(lComments.All(comment => comment.Progress != default(int)));
            Assert.IsTrue(lComments.All(comment => comment.ProgressState == MediaProgressState.Finished));
            Assert.IsTrue(lComments.All(comment => comment.Rating != default(int)));
            Assert.AreEqual(1, lComments.Count(comment => comment.SubRatings.Count == 0));
            Assert.IsTrue(lComments.All(comment => comment.Upvotes != default(int)));
        }

        [Test]
        public void CommentsMangaTest()
        {
            CommentEnumerable<Anime> lCommentEnumerable = this._user.CommentsAnime;
            lCommentEnumerable.Senpai = GeneralSetup.SenpaiInstance;
            Comment<Anime>[] lComments = lCommentEnumerable.ToArray();
            Assert.IsNotEmpty(lComments);
            Assert.IsTrue(lComments.All(comment => comment.Author == this._user));
            Assert.IsTrue(lComments.All(comment => !string.IsNullOrEmpty(comment.Content)));
            Assert.IsTrue(lComments.All(comment => comment.Id != default(int)));
            Assert.IsTrue(lComments.All(comment => comment.MediaObject != null));
            Assert.IsTrue(lComments.All(comment => comment.Progress != default(int)));
            Assert.IsTrue(lComments.All(comment => comment.ProgressState == MediaProgressState.Finished));
            Assert.IsTrue(lComments.All(comment => comment.Rating != default(int)));
            Assert.IsTrue(lComments.All(comment => comment.Upvotes != default(int)));
        }

        [Test]
        public async Task FromIdTest()
        {
            IProxerResult<User> lResult = await User.FromId(1);
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.AreEqual(1, lResult.Result.Id);
            Assert.AreEqual("Username", lResult.Result.UserName.GetIfInitialised(string.Empty));
        }

        [Test]
        public async Task FromUsernameTest()
        {
            IProxerResult<User> lResult = await User.FromUsername("Username");
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.AreEqual(1, lResult.Result.Id);
            Assert.AreEqual("Username", lResult.Result.UserName.GetIfInitialised(string.Empty));
        }

        [Test]
        public void IdTest()
        {
            Assert.AreEqual(1, this._user.Id);
        }

        [Test]
        public void MangaTest()
        {
            UserEntryEnumerable<Manga> lMangaEnumerable = this._user.Manga;
            lMangaEnumerable.Senpai = GeneralSetup.SenpaiInstance;
            UserProfileEntry<Manga>[] lManga = lMangaEnumerable.ToArray();
            Assert.AreEqual(4, lManga.Length);
            Assert.IsFalse(lManga.Any(entry => entry == null));
            Assert.IsTrue(lManga.All(entry => entry.Comment != null));
            Assert.IsTrue(lManga.All(entry => entry.Comment.Author == this._user));
            Assert.IsTrue(lManga.All(entry => entry.Comment.Id != default(int)));
            Assert.IsTrue(lManga.All(entry => entry.Comment.MediaObject == entry.MediaObject));
            Assert.AreEqual(1, lManga.Count(entry => entry.Comment.Progress == 0));
            Assert.AreEqual(2, lManga.Count(entry => entry.Comment.ProgressState == MediaProgressState.InProgress));
            Assert.IsTrue(lManga.All(entry => entry.MediaObject != null));
            Assert.IsTrue(lManga.All(entry => entry.MediaObject.Id != default(int)));
            Assert.IsTrue(lManga.All(entry =>
                    !string.IsNullOrEmpty(entry.MediaObject.Name.GetIfInitialised(string.Empty))));
            Assert.IsTrue(lManga.All(entry => entry.User == this._user));
        }

        [Test]
        public async Task PointsTest()
        {
            Assert.CatchAsync<InvalidUserException>(() => User.System.Points.ThrowFirstOnNonSuccess());

            IProxerResult<UserPoints> lResult = await this._user.Points;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.AreEqual(3912, lResult.Result.Anime);
            Assert.AreEqual(4, lResult.Result.Forum);
            Assert.AreEqual(2, lResult.Result.Info);
            Assert.AreEqual(624, lResult.Result.Manga);
            Assert.AreEqual(200, lResult.Result.Miscellaneous);
            Assert.AreEqual(97, lResult.Result.Uploads);
        }

        [OneTimeSetUp]
        public void Setup()
        {
            this._user = UserSetup.User;
        }

        [Test]
        public async Task StatusTest()
        {
            Assert.CatchAsync<InvalidUserException>(() => User.System.Points.ThrowFirstOnNonSuccess());

            IProxerResult<UserStatus> lResult = await this._user.Status;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.AreNotEqual(default(DateTime), lResult.Result.Status);
            Assert.IsNotNull(lResult.Result.Status);
            Assert.IsNotEmpty(lResult.Result.Status);
        }

        [Test]
        public async Task ToptenAnimeTest()
        {
            Assert.CatchAsync<InvalidUserException>(() => User.System.ToptenAnime.ThrowFirstOnNonSuccess(null));

            IProxerResult<IEnumerable<Anime>> lResult =
                await this._user.ToptenAnime.Get(GeneralSetup.SenpaiInstance);
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.AreEqual(2, lResult.Result.Count());
            Assert.IsTrue(lResult.Result.All(anime => anime.Id != default(int)));
        }

        [Test]
        public async Task ToptenMangaTest()
        {
            Assert.CatchAsync<InvalidUserException>(() => User.System.ToptenManga.ThrowFirstOnNonSuccess(null));

            IProxerResult<IEnumerable<Manga>> lResult =
                await this._user.ToptenManga.Get(GeneralSetup.SenpaiInstance);
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.AreEqual(4, lResult.Result.Count());
            Assert.IsTrue(lResult.Result.All(manga => manga.Id != default(int)));
        }

        [Test]
        public async Task UsernameTest()
        {
            Assert.CatchAsync<InvalidUserException>(() => User.System.Points.ThrowFirstOnNonSuccess());

            IProxerResult<string> lResult = await this._user.UserName;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.IsNotEmpty(lResult.Result);
        }

        #endregion
    }
}