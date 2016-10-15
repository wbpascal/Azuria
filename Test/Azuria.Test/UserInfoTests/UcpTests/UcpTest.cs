using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Media;
using Azuria.Media.Properties;
using Azuria.UserInfo.ControlPanel;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Azuria.Test.UserInfoTests.UcpTests
{
    [TestFixture]
    public class UcpTest
    {
        private UserControlPanel _controlPanel;

        [OneTimeSetUp]
        public void Setup()
        {
            this._controlPanel = UcpSetup.ControlPanel;
        }

        [Test]
        public async Task CommentVotesTest()
        {
            ProxerResult<IEnumerable<CommentVote>> lResult = await this._controlPanel.CommentVotes;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(2, lResult.Result.Count());
            Assert.IsTrue(lResult.Result.All(vote => vote.CommentId != default(int)));
            Assert.IsTrue(lResult.Result.All(vote => vote.VoteId != default(int)));
            Assert.IsTrue(lResult.Result.All(vote => vote.Rating != default(int)));
            Assert.IsTrue(lResult.Result.All(vote => vote.UserControlPanel == this._controlPanel));
            Assert.IsTrue(lResult.Result.All(vote => vote.Author.Id == 163825));
            Assert.IsTrue(
                lResult.Result.All(vote => vote.Author.UserName.GetObjectIfInitialised(string.Empty).Equals("KutoSan")));
            Assert.IsTrue(lResult.Result.All(vote => !string.IsNullOrEmpty(vote.AnimeMangaName)));
            Assert.IsTrue(lResult.Result.All(vote => vote.CommentContent != null));
        }

        [Test]
        public void ConstructorTest()
        {
            Assert.Catch<ArgumentException>(() => new UserControlPanel(null));
            Assert.Catch<NotLoggedInException>(() => new UserControlPanel(new Senpai("test")));
        }

        [Test]
        public async Task PointsTest()
        {
            ProxerResult<int> lAnimeResult = await this._controlPanel.PointsAnime;
            Assert.IsTrue(lAnimeResult.Success, JsonConvert.SerializeObject(lAnimeResult.Exceptions));
            Assert.AreEqual(3330, lAnimeResult.Result);

            ProxerResult<int> lMangaResult = await this._controlPanel.PointsManga;
            Assert.IsTrue(lMangaResult.Success, JsonConvert.SerializeObject(lMangaResult.Exceptions));
            Assert.AreEqual(1053, lMangaResult.Result);
        }

        [Test]
        public async Task ToptenAnimeTest()
        {
            ProxerResult<IEnumerable<ToptenObject<Anime>>> lResult = await this._controlPanel.ToptenAnime;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(1, lResult.Result.Count());
            Assert.IsTrue(lResult.Result.All(o => o.ToptenId != default(int)));
            Assert.IsTrue(lResult.Result.All(o => o.AnimeMangaObject.Id != default(int)));
            Assert.IsTrue(
                lResult.Result.All(
                    o => !string.IsNullOrEmpty(o.AnimeMangaObject.Name.GetObjectIfInitialised(string.Empty))));
            Assert.IsTrue(
                lResult.Result.All(
                    o =>
                        o.AnimeMangaObject.AnimeMedium.GetObjectIfInitialised(AnimeMedium.Unknown) !=
                        AnimeMedium.Unknown));
            Assert.IsTrue(lResult.Result.All(o => o.UserControlPanel == this._controlPanel));
        }

        [Test]
        public async Task ToptenMangaTest()
        {
            ProxerResult<IEnumerable<ToptenObject<Manga>>> lResult = await this._controlPanel.ToptenManga;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(1, lResult.Result.Count());
            Assert.IsTrue(lResult.Result.All(o => o.ToptenId != default(int)));
            Assert.IsTrue(lResult.Result.All(o => o.AnimeMangaObject.Id != default(int)));
            Assert.IsTrue(
                lResult.Result.All(
                    o => !string.IsNullOrEmpty(o.AnimeMangaObject.Name.GetObjectIfInitialised(string.Empty))));
            Assert.IsTrue(
                lResult.Result.All(
                    o =>
                        o.AnimeMangaObject.MangaMedium.GetObjectIfInitialised(MangaMedium.Unknown) !=
                        MangaMedium.Unknown));
            Assert.IsTrue(lResult.Result.All(o => o.UserControlPanel == this._controlPanel));
        }
    }
}