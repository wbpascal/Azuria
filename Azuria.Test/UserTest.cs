using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Main.User;
using Azuria.Main.User.Comment;
using Azuria.Test.Attributes;
using Azuria.Test.Utility;
using Azuria.Utilities.ErrorHandling;
using NUnit.Framework;

namespace Azuria.Test
{
    /// <summary>
    ///     Zusammenfassungsbeschreibung für UserTest
    /// </summary>
    [TestFixture, LoginRequired]
    public class UserTest
    {
        private readonly Senpai _senpai = SenpaiTest.Senpai;

        [Test, Order(3)]
        public async Task AreUserFriendsTest()
        {
            Assert.IsNotNull(this._senpai.Me);
            ProxerResult<bool> lAreUserFriendsResult = await User.AreUserFriends(this._senpai.Me, User.System);
            Assert.IsTrue(lAreUserFriendsResult.Success);
            Assert.IsFalse(lAreUserFriendsResult.Result);
        }

        [Test, Order(1)]
        public async Task AvatarTest()
        {
            Assert.IsNotNull(this._senpai.Me);
            Uri lAvatar = await this._senpai.Me.Avatar.GetObject(new Uri("https://google.com/"));

            Assert.AreNotEqual(lAvatar.OriginalString, "https://google.com/");
            Assert.IsTrue(lAvatar.OriginalString.EndsWith(".jpg"));
        }

        [Test, Order(1)]
        public async Task ChronicTest()
        {
            Assert.IsNotNull(this._senpai.Me);
            IEnumerable<AnimeMangaChronicObject> lChronic =
                await this._senpai.Me.Chronic.GetObject(new AnimeMangaChronicObject[0]);
            Assert.IsNotEmpty(lChronic);
        }

        [Test, Order(1)]
        public async Task FriendsTest()
        {
            Assert.IsNotNull(this._senpai.Me);
            IEnumerable<User> lFriends = await this._senpai.Me.Friends.GetObject(new User[0]);
            Assert.IsNotEmpty(lFriends);
        }

        [Test, Order(3)]
        public async Task GetCommentsTest()
        {
            Assert.IsNotNull(this._senpai.Me);
            ProxerResult<IEnumerable<Comment>> lCommentsResult = await this._senpai.Me.GetComments(0, 20);
            Assert.IsTrue(lCommentsResult.Success);
            Assert.IsNotNull(lCommentsResult.Result);
            Assert.IsTrue(lCommentsResult.Result.Count() <= 20);

            await Task.Delay(2000);
        }

        [Test, Order(1)]
        public async Task InfoHtmlTest()
        {
            Assert.IsNotNull(this._senpai.Me);
            string lInfoHtml = await this._senpai.Me.InfoHtml.GetObject(string.Empty);
            Assert.IsNotEmpty(lInfoHtml);
        }

        [Test, Order(1)]
        public async Task InfoTest()
        {
            Assert.IsNotNull(this._senpai.Me);

            //Ich wette diesen String hat keiner in seinem Profil stehen
            string lRandomHexString = RandomUtility.GetRandomHexString();

            string lInfo = await this._senpai.Me.Info.GetObject(lRandomHexString);
            //Assert.Pass($"Original: {lRandomBytes.ToHexString()} ; Encrypted: {lRandomHexString}");
            Assert.AreNotEqual(lInfo, lRandomHexString, $"WTF-String: {lRandomHexString}");
        }

        [Test, Order(1)]
        public async Task IsOnlineTest()
        {
            Assert.IsNotNull(this._senpai.Me);
            ProxerResult<bool> lIsOnlineResult = await this._senpai.Me.IsOnline.GetObject();
            Assert.IsTrue(lIsOnlineResult.Success);
        }


        [Test, Order(1)]
        public async Task PointsTest()
        {
            Assert.IsNotNull(this._senpai.Me);
            int lPoints = await this._senpai.Me.Points.GetObject(int.MinValue);
            Assert.AreNotEqual(lPoints, int.MinValue);
        }

        [Test, Order(1)]
        public async Task RankingTest()
        {
            Assert.IsNotNull(this._senpai.Me);
            string lRanking = await this._senpai.Me.Ranking.GetObject(string.Empty);
            Assert.IsNotEmpty(lRanking);
        }

        [Test, Order(3)]
        public async Task SendFriendsRequestTest()
        {
            Assert.IsNotNull(this._senpai.Me);
            ProxerResult lInvalidUserResut = await User.System.SendFriendRequest();
            ProxerResult lValidUserResult = await this._senpai.Me.SendFriendRequest();

            Assert.IsFalse(lInvalidUserResut.Success);
            Assert.IsNotEmpty(lInvalidUserResut.Exceptions);
            Assert.IsTrue(
                lInvalidUserResut.Exceptions.Any(exception => exception.GetType() == typeof(InvalidUserException)));

            await Task.Delay(2000);
        }

        [Test, Order(1)]
        public async Task StatusTest()
        {
            Assert.IsNotNull(this._senpai.Me);
            string lStatus = await this._senpai.Me.Status.GetObject(string.Empty);
            Assert.IsNotEmpty(lStatus);
        }

        [Test, Order(3)]
        public void ToStringTest()
        {
            Assert.IsNotNull(this._senpai.Me);
            string lToString = this._senpai.Me.ToString();
            Assert.AreEqual(lToString,
                this._senpai.Me.UserName.GetObjectIfInitialised("ERROR") + " [" + this._senpai.Me.Id + "]");
        }

        [Test, Order(1)]
        public async Task UsernameTest()
        {
            Assert.IsNotNull(this._senpai.Me);
            string lUsername = await this._senpai.Me.UserName.GetObject(string.Empty);
            Assert.AreEqual(lUsername, Credentials.Username);
        }
    }
}