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
        private BookmarkObject<Anime> _animeBookmark;
        private BookmarkObject<Manga> _mangaBookmark;

        [OneTimeSetUp]
        public void Setup()
        {
            this._animeBookmark = UcpSetup.ControlPanel.BookmarksAnime.First();
            this._mangaBookmark = UcpSetup.ControlPanel.BookmarksManga.First();
        }

        [Test]
        public void AnimeMangaContentObjectTest()
        {
            Anime.Episode lEpisode = this._animeBookmark.AnimeMangaContentObject as Anime.Episode;
            Manga.Chapter lChapter = this._mangaBookmark.AnimeMangaContentObject as Manga.Chapter;
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
            Assert.AreEqual("Initial D First Stage", lEpisode.ParentObject.Name.GetObjectIfInitialised());
            Assert.AreEqual("The Gamer", lChapter.ParentObject.Name.GetObjectIfInitialised());
            Assert.AreEqual(AnimeMedium.Series, lEpisode.ParentObject.AnimeMedium.GetObjectIfInitialised());
            Assert.AreEqual(MangaMedium.Series, lChapter.ParentObject.MangaMedium.GetObjectIfInitialised());
            Assert.AreEqual(AnimeMangaStatus.Completed, lEpisode.ParentObject.Status.GetObjectIfInitialised());
            Assert.AreEqual(AnimeMangaStatus.Airing, lChapter.ParentObject.Status.GetObjectIfInitialised());
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
            ProxerResult lResult = await this._animeBookmark.DeleteReminder();
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
        }

        [Test]
        public void UserControlPanelTest()
        {
            Assert.AreSame(UcpSetup.ControlPanel, this._animeBookmark.UserControlPanel);
            Assert.AreSame(UcpSetup.ControlPanel, this._mangaBookmark.UserControlPanel);
        }
    }
}