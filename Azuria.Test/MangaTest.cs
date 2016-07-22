using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Test.Attributes;
using Azuria.Test.Utility;
using Azuria.User;
using Azuria.User.Comment;
using Azuria.User.ControlPanel;
using Azuria.Utilities.ErrorHandling;
using NUnit.Framework;

namespace Azuria.Test
{
    [TestFixture, LoginRequired]
    public class MangaTest
    {
        private readonly Senpai _senpai = SenpaiTest.Senpai;
        private Manga _manga;
        private Manga.Chapter _chapter;

        [Test, Order(2)]
        public async Task AddToPlanned()
        {
            Assert.IsNotNull(this._manga);
            ProxerResult<AnimeMangaUcpObject<Manga>> lAddToPlannedResult = await this._manga.AddToPlanned();
            Assert.IsTrue(lAddToPlannedResult.Success);
            Assert.IsNotNull(lAddToPlannedResult.Result);
            Assert.AreEqual(lAddToPlannedResult.Result.AnimeMangaObject.Id, this._manga.Id);
        }

        [Test, Order(2)]
        public async Task AvailableLanguagesTest()
        {
            Assert.IsNotNull(this._manga);
            IEnumerable<Language> lLanguages = await this._manga.AvailableLanguages.GetObject(new Language[0]);
            Assert.IsNotEmpty(lLanguages);
        }

        [Test, Order(4)]
        public async Task Chapter_AddBookmarkTest()
        {
            Assert.IsNotNull(this._manga);
            Assert.IsNotNull(this._chapter);

            await Task.Delay(2000);

            ProxerResult<AnimeMangaBookmarkObject<Manga>> lChapterBookmark =
                await this._chapter.AddToBookmarks();
            Assert.IsTrue(lChapterBookmark.Success);
            Assert.IsNotNull(lChapterBookmark.Result);
            Assert.AreEqual(lChapterBookmark.Result.AnimeMangaContentObject.ParentObject.Id, this._manga.Id);

            await Task.Delay(2000);

            ProxerResult lDeleteResult = await lChapterBookmark.Result.DeleteEntry();
            Assert.IsTrue(lDeleteResult.Success);
        }

        [Test, Order(4)]
        public async Task Chapter_DateTest()
        {
            Assert.IsNotNull(this._manga);
            Assert.IsNotNull(this._chapter);

            DateTime lUploadDate = await this._chapter.Date.GetObject(DateTime.MinValue);
            Assert.AreNotEqual(lUploadDate, DateTime.MinValue);
        }

        [Test, Order(4)]
        public async Task Chapter_PagesTest()
        {
            Assert.IsNotNull(this._manga);
            Assert.IsNotNull(this._chapter);

            IEnumerable<Uri> lPages = await this._chapter.Pages.GetObject(new Uri[0]);
            Assert.IsNotEmpty(lPages);
            Assert.IsTrue(lPages.All(uri => uri.OriginalString.EndsWith(".png") || uri.OriginalString.EndsWith(".jpg")));
        }

        [Test, Order(4)]
        public async Task Chapter_ScanlatorGroupTest()
        {
            Assert.IsNotNull(this._manga);
            Assert.IsNotNull(this._chapter);

            Group lScanlatorGroup = await this._chapter.ScanlatorGroup.GetObject(Group.Error);
            Assert.AreNotEqual(lScanlatorGroup, Group.Error);
        }

        [Test, Order(4)]
        public async Task Chapter_TitleTest()
        {
            Assert.IsNotNull(this._manga);
            Assert.IsNotNull(this._chapter);

            string lHexString = RandomUtility.GetRandomHexString();
            string lTitle = await this._chapter.Titel.GetObject(lHexString);
            Assert.AreNotEqual(lTitle, lHexString);
        }

        [Test, Order(4)]
        public async Task Chapter_UploaderNameTest()
        {
            Assert.IsNotNull(this._manga);
            Assert.IsNotNull(this._chapter);

            string lHexString = RandomUtility.GetRandomHexString();
            string lUploaderName = await this._chapter.UploaderName.GetObject(lHexString);
            Assert.AreNotEqual(lUploaderName, lHexString);
        }

        [Test, Order(2)]
        public void CoverTest()
        {
            Assert.IsNotNull(this._manga);
            Assert.IsTrue(this._manga.CoverUri.OriginalString.EndsWith(".jpg"));
        }

        [Test, Order(2)]
        public async Task DescriptionTest()
        {
            Assert.IsNotNull(this._manga);

            string lHexString = RandomUtility.GetRandomHexString();
            string lDescription = await this._manga.Description.GetObject(lHexString);
            Assert.AreNotEqual(lDescription, lHexString);
        }

        [Test, Order(2)]
        public async Task EnglishTitleTest()
        {
            Assert.IsNotNull(this._manga);

            string lHexString = RandomUtility.GetRandomHexString();
            string lEnglishTitle = await this._manga.EnglishTitle.GetObject(lHexString);
            Assert.AreNotEqual(lEnglishTitle, lHexString);
        }

        [Test, Order(2)]
        public async Task EpisodeCountTest()
        {
            Assert.IsNotNull(this._manga);
            int lChapterCount = await this._manga.ContentCount.GetObject(int.MinValue);
            Assert.AreNotEqual(lChapterCount, int.MinValue);
        }

        [Test, Order(2)]
        public async Task FskTest()
        {
            Assert.IsNotNull(this._manga);

            ProxerResult<IEnumerable<FskType>> lFskResult = await this._manga.Fsk.GetObject();
            Assert.IsTrue(lFskResult.Success);
            Assert.IsNotNull(lFskResult.Result);
            Assert.IsTrue(lFskResult.Result.Any());
        }

        [Test, Order(2)]
        public async Task GenreTest()
        {
            Assert.IsNotNull(this._manga);
            IEnumerable<GenreType> lGenre = await this._manga.Genre.GetObject(new GenreType[0]);
            Assert.IsNotEmpty(lGenre);
        }

        [Test, Order(2)]
        public async Task GermanTitleTest()
        {
            Assert.IsNotNull(this._manga);

            string lHexString = RandomUtility.GetRandomHexString();
            string lGermanTitle = await this._manga.GermanTitle.GetObject(lHexString);
            Assert.AreNotEqual(lGermanTitle, lHexString);
        }

        [Test, Order(3)]
        public async Task GetChapterTest()
        {
            Assert.IsNotNull(this._manga);

            IEnumerable<Language> lAvailableLanguages = await this._manga.AvailableLanguages.GetObject(new Language[0]);
            Assert.IsNotEmpty(lAvailableLanguages);

            ProxerResult<IEnumerable<Manga.Chapter>> lChaper =
                await this._manga.GetChapters(lAvailableLanguages.First());
            Assert.IsTrue(lChaper.Success);
            Assert.IsNotNull(lChaper.Result);
            Assert.IsTrue(lChaper.Result.Count() == await this._manga.ContentCount.GetObject(int.MinValue));
            Assert.IsTrue(lChaper.Result.All(chapter => chapter.Language == lAvailableLanguages.First()));
            Assert.IsTrue(lChaper.Result.All(chapter => chapter.ParentObject == this._manga));

            foreach (Manga.Chapter chapter in lChaper.Result)
            {
                if (await chapter.IsAvailable.GetObject(false))
                {
                    this._chapter = chapter;
                    break;
                }
            }
            Assert.IsNotNull(this._chapter);

            await Task.Delay(2000);
        }

        [Test, Order(2)]
        public async Task GetLatestCommentsTest()
        {
            Assert.IsNotNull(this._manga);

            ProxerResult<IEnumerable<Comment<Manga>>> lLatestComments1 = await this._manga.GetCommentsLatest(0, 2);
            Assert.IsTrue(lLatestComments1.Success);
            Assert.IsNotNull(lLatestComments1.Result);
            Assert.IsNotEmpty(lLatestComments1.Result);
            Assert.IsTrue(lLatestComments1.Result.First().AnimeMangaObject.Id == this._manga.Id);

            await Task.Delay(2000);

            ProxerResult<IEnumerable<Comment<Manga>>> lLatestComments2 = await this._manga.GetCommentsLatest(1, 2);
            Assert.IsTrue(lLatestComments2.Success);
            Assert.IsNotNull(lLatestComments2.Result);
            Assert.IsNotEmpty(lLatestComments2.Result);
            Assert.IsTrue(lLatestComments2.Result.First().AnimeMangaObject.Id == this._manga.Id);

            Assert.AreEqual(lLatestComments1.Result.Last().Author.Id, lLatestComments2.Result.First().Author.Id);
        }

        [Test, Order(1)]
        public async Task GetMangaTest()
        {
            Assert.IsNotNull(this._senpai.Me);

            IEnumerable<KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<Manga>>> lMangaList =
                await
                    this._senpai.Me.Manga.GetObject(
                        new KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<Manga>>[0]);
            IEnumerable<Manga> lFavouriteManga = await this._senpai.Me.MangaFavourites.GetObject(new Manga[0]);

            Assert.IsNotEmpty(lMangaList);
            Assert.IsNotEmpty(lFavouriteManga);

            this._manga = lMangaList.First().Value.AnimeMangaObject;
            await Task.Delay(2000);
        }

        [Test, Order(2)]
        public async Task GetPopularMangaTest()
        {
            ProxerResult<IEnumerable<Manga>> lPopularManga = await Manga.GetPopularManga(this._senpai);
            Assert.IsTrue(lPopularManga.Success);
            Assert.IsNotNull(lPopularManga.Result);
            Assert.IsNotEmpty(lPopularManga.Result);
        }

        [Test, Order(2)]
        public async Task GetRatingCommentsTest()
        {
            Assert.IsNotNull(this._manga);

            ProxerResult<IEnumerable<Comment<Manga>>> lRatingComments1 = await this._manga.GetCommentsRating(0, 2);
            Assert.IsTrue(lRatingComments1.Success);
            Assert.IsNotNull(lRatingComments1.Result);
            Assert.IsNotEmpty(lRatingComments1.Result);
            Assert.IsTrue(lRatingComments1.Result.First().AnimeMangaObject.Id == this._manga.Id);

            await Task.Delay(2000);

            ProxerResult<IEnumerable<Comment<Manga>>> lRatingComments2 = await this._manga.GetCommentsRating(1, 2);
            Assert.IsTrue(lRatingComments2.Success);
            Assert.IsNotNull(lRatingComments2.Result);
            Assert.IsNotEmpty(lRatingComments2.Result);
            Assert.IsTrue(lRatingComments2.Result.First().AnimeMangaObject.Id == this._manga.Id);

            Assert.AreEqual(lRatingComments1.Result.Last().Author.Id, lRatingComments2.Result.First().Author.Id);
        }

        [Test, Order(2)]
        public async Task GroupsTest()
        {
            Assert.IsNotNull(this._manga);
            IEnumerable<Group> lGroups = await this._manga.Groups.GetObject(new[] {Group.Error});
            Assert.IsFalse(lGroups.Count() == 1 && lGroups.Contains(Group.Error));
        }

        [Test, Order(2)]
        public async Task IndustryTest()
        {
            Assert.IsNotNull(this._manga);
            IEnumerable<Industry> lIndustry = await this._manga.Industry.GetObject(new[] {Industry.Error});
            Assert.IsFalse(lIndustry.Count() == 1 && lIndustry.Contains(Industry.Error));
        }

        [Test, Order(2)]
        public async Task IsLicensedTest()
        {
            Assert.IsNotNull(this._manga);
            ProxerResult<bool> lIsLicensed = await this._manga.IsLicensed.GetObject();
            Assert.IsTrue(lIsLicensed.Success);
        }

        [Test, Order(2)]
        public async Task JapaneseTitleTest()
        {
            Assert.IsNotNull(this._manga);

            string lHexString = RandomUtility.GetRandomHexString();
            string lJapaneseTitle = await this._manga.JapaneseTitle.GetObject(lHexString);
            Assert.AreNotEqual(lJapaneseTitle, lHexString);
        }

        [Test, Order(2)]
        public async Task MangaTypeTest()
        {
            Assert.IsNotNull(this._manga);
            MangaType lMangaType = await this._manga.MangaType.GetObject(MangaType.Unknown);
            Assert.AreNotEqual(lMangaType, MangaType.Unknown);
        }

        [Test, Order(2)]
        public async Task NameTest()
        {
            Assert.IsNotNull(this._manga);

            string lHexString = RandomUtility.GetRandomHexString();
            string lName = await this._manga.Name.GetObject(lHexString);
            Assert.AreNotEqual(lName, lHexString);
        }

        [Test, Order(2)]
        public async Task SeasonTest()
        {
            Assert.IsNotNull(this._manga);

            string lHexString = RandomUtility.GetRandomHexString();
            IEnumerable<string> lSeason = await this._manga.Season.GetObject(new[] {lHexString});
            Assert.IsFalse(lSeason.Count() == 1 && lSeason.Contains(lHexString));
        }

        [Test, Order(2)]
        public async Task StatusTest()
        {
            Assert.IsNotNull(this._manga);
            AnimeMangaStatus lStatus = await this._manga.Status.GetObject(AnimeMangaStatus.Unknown);
            Assert.AreNotEqual(lStatus, AnimeMangaStatus.Unknown);
        }

        [Test, Order(2)]
        public async Task SynonymTest()
        {
            Assert.IsNotNull(this._manga);

            string lHexString = RandomUtility.GetRandomHexString();
            string lSynonym = await this._manga.Synonym.GetObject(lHexString);
            Assert.AreNotEqual(lSynonym, lHexString);
        }
    }
}