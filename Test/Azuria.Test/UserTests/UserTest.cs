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
        public async Task AvatarTest()
        {
            Assert.CatchAsync<InvalidUserException>(() => User.System.Points.ThrowFirstOnNonSuccess());

            ProxerResult<Uri> lResult = await this._user.Avatar;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsTrue(lResult.Result.AbsoluteUri.StartsWith("https://cdn.proxer.me/avatar/"));
        }

        [Test]
        public void CommentsAnimeTest()
        {
            Comment<Anime>[] lComments = this._user.CommentsAnime.ToArray();
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
            Comment<Manga>[] lComments = this._user.CommentsManga.ToArray();
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
        public void ConstructorTest()
        {
            Assert.Catch<ArgumentOutOfRangeException>(() => new User(-1));
        }

        [Test]
        public void IdTest()
        {
            Assert.AreEqual(1, this._user.Id);
        }

        [Test]
        public async Task PointsTest()
        {
            Assert.CatchAsync<InvalidUserException>(() => User.System.Points.ThrowFirstOnNonSuccess());

            ProxerResult<UserPoints> lResult = await this._user.Points;
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

            ProxerResult<UserStatus> lResult = await this._user.Status;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.AreNotEqual(default(DateTime), lResult.Result.Status);
            Assert.IsNotNull(lResult.Result.Status);
            Assert.IsNotEmpty(lResult.Result.Status);
        }

        [Test]
        public async Task ToptenAnimeTest()
        {
            Assert.CatchAsync<InvalidUserException>(() => User.System.ToptenAnime.ThrowFirstOnNonSuccess());

            ProxerResult<IEnumerable<Anime>> lResult = await this._user.ToptenAnime;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.AreEqual(2, lResult.Result.Count());
            Assert.IsTrue(lResult.Result.All(anime => anime.Id != default(int)));
        }

        [Test]
        public async Task ToptenMangaTest()
        {
            Assert.CatchAsync<InvalidUserException>(() => User.System.ToptenManga.ThrowFirstOnNonSuccess());

            ProxerResult<IEnumerable<Manga>> lResult = await this._user.ToptenManga;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.AreEqual(4, lResult.Result.Count());
            Assert.IsTrue(lResult.Result.All(manga => manga.Id != default(int)));
        }

        [Test]
        public async Task UsernameTest()
        {
            Assert.CatchAsync<InvalidUserException>(() => User.System.Points.ThrowFirstOnNonSuccess());

            ProxerResult<string> lResult = await this._user.UserName;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.IsNotEmpty(lResult.Result);
        }

        #endregion
    }
}