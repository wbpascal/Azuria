using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Media;
using Azuria.Media.Properties;
using Azuria.UserInfo;
using Azuria.UserInfo.ControlPanel;
using Azuria.Utilities.Extensions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Azuria.Test.UcpTests
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
        public async Task AddToBookmarksTest()
        {
            Assert.CatchAsync<ArgumentNullException>(
                () => this._controlPanel.AddToBookmarks(null).ThrowFirstForNonSuccess());

            Anime.Episode lEpisode = (await
                ((Anime) await MediaObject.CreateFromId(9200).ThrowFirstForNonSuccess()).GetEpisodes(
                    AnimeLanguage.EngSub).ThrowFirstForNonSuccess())?.FirstOrDefault();
            IProxerResult lResult = await this._controlPanel.AddToBookmarks(lEpisode);
            Assert.IsTrue(lResult.Success);
        }

        [Test]
        [TestCase(MediaProfileList.Favourites)]
        [TestCase(MediaProfileList.Finished)]
        [TestCase(MediaProfileList.Noted)]
        public async Task AddToProfileListTest(MediaProfileList profileList)
        {
            Assert.CatchAsync<ArgumentOutOfRangeException>(
                () => this._controlPanel.AddToProfileList(-1, profileList).ThrowFirstForNonSuccess());

            IProxerResult lResult = await this._controlPanel.AddToProfileList(1, profileList);
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
        }

        [Test]
        public void BookmarkAnimeTest()
        {
            Bookmark<Anime>[] lBookmarks = this._controlPanel.BookmarksAnime.ToArray();
            Assert.AreEqual(17, lBookmarks.Length);
            Assert.IsTrue(lBookmarks.All(o => o.MediaContentObject != null));
            Assert.IsTrue(lBookmarks.All(o => o.MediaContentObject.ContentIndex != default(int)));
            Assert.IsTrue(lBookmarks.All(o => o.MediaContentObject.ParentObject.Id != default(int)));
            Assert.IsTrue(
                lBookmarks.All(
                    o =>
                        !string.IsNullOrEmpty(
                            o.MediaContentObject.ParentObject.Name.GetObjectIfInitialised(string.Empty))));
            Assert.IsTrue(
                lBookmarks.All(
                    o =>
                        o.MediaContentObject.ParentObject.AnimeMedium.GetObjectIfInitialised(
                            AnimeMedium.Unknown) != AnimeMedium.Unknown));
            Assert.IsTrue(lBookmarks.All(o => o.BookmarkId != default(int)));
            Assert.IsTrue(lBookmarks.All(o => o.UserControlPanel == this._controlPanel));
        }

        [Test]
        public void BookmarkMangaTest()
        {
            Bookmark<Manga>[] lBookmarks = this._controlPanel.BookmarksManga.ToArray();
            Assert.AreEqual(4, lBookmarks.Length);
            Assert.IsTrue(lBookmarks.All(o => o.MediaContentObject != null));
            Assert.IsTrue(lBookmarks.All(o => o.MediaContentObject.ContentIndex != default(int)));
            Assert.IsTrue(lBookmarks.All(o => o.MediaContentObject.ParentObject.Id != default(int)));
            Assert.IsTrue(
                lBookmarks.All(
                    o =>
                        !string.IsNullOrEmpty(
                            o.MediaContentObject.ParentObject.Name.GetObjectIfInitialised(string.Empty))));
            Assert.IsTrue(
                lBookmarks.All(
                    o =>
                        o.MediaContentObject.ParentObject.MangaMedium.GetObjectIfInitialised(
                            MangaMedium.Unknown) != MangaMedium.Unknown));
            Assert.IsTrue(lBookmarks.All(o => o.BookmarkId != default(int)));
            Assert.IsTrue(lBookmarks.All(o => o.UserControlPanel == this._controlPanel));
        }

        [Test]
        public async Task CommentVotesTest()
        {
            IProxerResult<IEnumerable<CommentVote>> lResult = await this._controlPanel.CommentVotes;
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
            Assert.IsTrue(lResult.Result.All(vote => !string.IsNullOrEmpty(vote.MediaName)));
            Assert.IsTrue(lResult.Result.All(vote => vote.CommentContent != null));
        }

        [Test]
        public void ConstructorTest()
        {
            Assert.Catch<ArgumentException>(() => new UserControlPanel(null));
            Assert.Catch<NotLoggedInException>(() => new UserControlPanel(new Senpai("test")));
        }

        [Test]
        public async Task DeleteBookmarkTest()
        {
            Assert.CatchAsync<ArgumentOutOfRangeException>(
                () => this._controlPanel.DeleteBookmark(-1).ThrowFirstForNonSuccess());

            IProxerResult lResult = await this._controlPanel.DeleteBookmark(1);
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
        }

        [Test]
        public async Task DeleteCommentVoteTest()
        {
            Assert.CatchAsync<ArgumentOutOfRangeException>(
                () => this._controlPanel.DeleteCommentVote(-1).ThrowFirstForNonSuccess());

            IProxerResult lResult = await this._controlPanel.DeleteCommentVote(1);
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
        }

        [Test]
        public async Task DeleteToptenTest()
        {
            Assert.CatchAsync<ArgumentOutOfRangeException>(
                () => this._controlPanel.DeleteTopten(-1).ThrowFirstForNonSuccess());

            IProxerResult lResult = await this._controlPanel.DeleteTopten(1);
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
        }

        [Test]
        public void HistoryTest()
        {
            HistoryObject<IMediaObject>[] lHistory = this._controlPanel.History.ToArray();
            Assert.AreEqual(8, lHistory.Length);
            Assert.IsTrue(lHistory.All(o => (o.TimeStamp != DateTime.MinValue) && (o.TimeStamp != DateTime.MaxValue)));
            Assert.IsTrue(lHistory.All(o => o.UserControlPanel == this._controlPanel));
            Assert.IsTrue(lHistory.All(o => o.ContentObject != null));
            Assert.IsTrue(lHistory.All(o => o.ContentObject.ContentIndex != default(int)));
            Assert.IsTrue(lHistory.All(o => o.ContentObject.GeneralLanguage != Language.Unkown));
            Assert.IsTrue(lHistory.All(o => o.ContentObject.ParentObject.Id != default(int)));
            Assert.IsTrue(
                lHistory.All(
                    o => !string.IsNullOrEmpty(o.ContentObject.ParentObject.Name.GetObjectIfInitialised(string.Empty))));
        }

        [Test]
        public async Task PointsTest()
        {
            IProxerResult<int> lAnimeResult = await this._controlPanel.PointsAnime;
            Assert.IsTrue(lAnimeResult.Success, JsonConvert.SerializeObject(lAnimeResult.Exceptions));
            Assert.AreEqual(3330, lAnimeResult.Result);

            IProxerResult<int> lMangaResult = await this._controlPanel.PointsManga;
            Assert.IsTrue(lMangaResult.Success, JsonConvert.SerializeObject(lMangaResult.Exceptions));
            Assert.AreEqual(1053, lMangaResult.Result);
        }

        [Test]
        public async Task ToptenAnimeTest()
        {
            IProxerResult<IEnumerable<ToptenObject<Anime>>> lResult = await this._controlPanel.ToptenAnime;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(1, lResult.Result.Count());
            Assert.IsTrue(lResult.Result.All(o => o.ToptenId != default(int)));
            Assert.IsTrue(lResult.Result.All(o => o.MediaObject != null));
            Assert.IsTrue(lResult.Result.All(o => o.MediaObject.Id != default(int)));
            Assert.IsTrue(
                lResult.Result.All(
                    o => !string.IsNullOrEmpty(o.MediaObject.Name.GetObjectIfInitialised(string.Empty))));
            Assert.IsTrue(
                lResult.Result.All(
                    o =>
                        o.MediaObject.AnimeMedium.GetObjectIfInitialised(AnimeMedium.Unknown) !=
                        AnimeMedium.Unknown));
            Assert.IsTrue(lResult.Result.All(o => o.UserControlPanel == this._controlPanel));
        }

        [Test]
        public async Task ToptenMangaTest()
        {
            IProxerResult<IEnumerable<ToptenObject<Manga>>> lResult = await this._controlPanel.ToptenManga;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(1, lResult.Result.Count());
            Assert.IsTrue(lResult.Result.All(o => o.ToptenId != default(int)));
            Assert.IsTrue(lResult.Result.All(o => o.MediaObject != null));
            Assert.IsTrue(lResult.Result.All(o => o.MediaObject.Id != default(int)));
            Assert.IsTrue(
                lResult.Result.All(
                    o => !string.IsNullOrEmpty(o.MediaObject.Name.GetObjectIfInitialised(string.Empty))));
            Assert.IsTrue(
                lResult.Result.All(
                    o =>
                        o.MediaObject.MangaMedium.GetObjectIfInitialised(MangaMedium.Unknown) !=
                        MangaMedium.Unknown));
            Assert.IsTrue(lResult.Result.All(o => o.UserControlPanel == this._controlPanel));
        }
    }
}