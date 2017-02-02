using System.Linq;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Media;
using Azuria.Media.Properties;
using Azuria.UserInfo.ControlPanel;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Azuria.Test.UcpTests
{
    [TestFixture]
    public class BookmarkTest
    {
        private Bookmark<Anime> _animeBookmark;
        private Bookmark<Manga> _mangaBookmark;

        [OneTimeSetUp]
        public void Setup()
        {
            this._animeBookmark = UcpSetup.ControlPanel.BookmarksAnime.First();
            this._mangaBookmark = UcpSetup.ControlPanel.BookmarksManga.First();
        }

        [Test]
        public void BookmarkIdTest()
        {
            Assert.AreEqual(1, this._animeBookmark.BookmarkId);
            Assert.AreEqual(3, this._mangaBookmark.BookmarkId);
        }

        [Test]
        public async Task DeleteReminderTest()
        {
            IProxerResult lResult = await this._animeBookmark.Delete();
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
        }

        [Test]
        public void MediaContentObjectTest()
        {
            Episode lEpisode = this._animeBookmark.MediaContentObject as Episode;
            Chapter lChapter = this._mangaBookmark.MediaContentObject as Chapter;
            Assert.IsNotNull(lEpisode);
            Assert.IsNotNull(lChapter);
            Assert.AreEqual(Language.German, lEpisode.GeneralLanguage);
            Assert.AreEqual(Language.English, lChapter.GeneralLanguage);
            Assert.AreEqual(AnimeLanguage.GerSub, lEpisode.Language);
            Assert.AreEqual(Language.English, lChapter.Language);
            Assert.AreEqual(23, lEpisode.ContentIndex);
            Assert.AreEqual(145, lChapter.ContentIndex);

            Assert.AreEqual(355, lEpisode.ParentObject.Id);
            Assert.AreEqual(7578, lChapter.ParentObject.Id);
            Assert.AreEqual("Initial D First Stage", lEpisode.ParentObject.Name.GetIfInitialised());
            Assert.AreEqual("The Gamer", lChapter.ParentObject.Name.GetIfInitialised());
            Assert.AreEqual(AnimeMedium.Series, lEpisode.ParentObject.AnimeMedium.GetIfInitialised());
            Assert.AreEqual(MangaMedium.Series, lChapter.ParentObject.MangaMedium.GetIfInitialised());
            Assert.AreEqual(MediaStatus.Completed, lEpisode.ParentObject.Status.GetIfInitialised());
            Assert.AreEqual(MediaStatus.Airing, lChapter.ParentObject.Status.GetIfInitialised());
        }

        [Test]
        public void UserControlPanelTest()
        {
            Assert.AreSame(UcpSetup.ControlPanel, this._animeBookmark.UserControlPanel);
            Assert.AreSame(UcpSetup.ControlPanel, this._mangaBookmark.UserControlPanel);
        }
    }
}