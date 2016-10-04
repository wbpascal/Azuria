using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Media;
using Azuria.Media.Properties;
using Azuria.Utilities.Extensions;
using NUnit.Framework;

namespace Azuria.Test.MediaTests
{
    [TestFixture]
    public class AnimeMangaObjectTest
    {
        private AnimeMangaObject _animeMangaObject;

        [OneTimeSetUp]
        public async Task Setup()
        {
            this._animeMangaObject =
                await AnimeMangaObject.CreateFromId(41).ThrowFirstForNonSuccess() as AnimeMangaObject;
            Assert.IsNotNull(this._animeMangaObject);
        }

        [Test]
        public async Task ClicksTest()
        {
            ProxerResult<int> lClickResult = await this._animeMangaObject.Clicks;
            Assert.IsTrue(lClickResult.Success);
            Assert.IsEmpty(lClickResult.Exceptions);
            Assert.AreEqual(lClickResult.Result, 8603);
        }

        [Test]
        public async Task ContentCountTest()
        {
            ProxerResult<int> lContentCountResult = await this._animeMangaObject.ContentCount;
            Assert.IsTrue(lContentCountResult.Success);
            Assert.IsEmpty(lContentCountResult.Exceptions);
            Assert.AreEqual(lContentCountResult.Result, 25);
        }

        [Test]
        public void CoverUriTest()
        {
            Uri lCoverUri = this._animeMangaObject.CoverUri;
            Assert.IsNotNull(lCoverUri);
            Assert.IsTrue(lCoverUri.AbsoluteUri.EndsWith(".jpg"));
        }

        [Test]
        public async Task CreateFromIdTest()
        {
            Assert.CatchAsync<ArgumentException>(() => AnimeMangaObject.CreateFromId(-1).ThrowFirstForNonSuccess());

            IAnimeMangaObject lValidObject = await AnimeMangaObject.CreateFromId(41).ThrowFirstForNonSuccess();
            Assert.IsNotNull(lValidObject);
            Assert.AreEqual(lValidObject.Id, 41);

            Assert.AreEqual(
                Assert.CatchAsync<ProxerApiException>(() => AnimeMangaObject.CreateFromId(666).ThrowFirstForNonSuccess())
                    .ErrorCode, ErrorCode.InfoInvalidId);
        }

        [Test]
        public async Task DescriptionTest()
        {
            ProxerResult<string> lDescriptionResult = await this._animeMangaObject.Description;
            Assert.IsTrue(lDescriptionResult.Success);
            Assert.IsEmpty(lDescriptionResult.Exceptions);
            Assert.IsNotNull(lDescriptionResult.Result);
            Assert.AreEqual(lDescriptionResult.Result,
                "Am 10. August 2010 zerschmettert das Heilige Britannische Reich die " +
                "japanischen Streitkräfte und erobert das Land in weniger als einem Monat. " +
                "Durch die Niederlage verliert Japan seine Unabhängigkeit und wird zur " +
                "11. Kolonie der Britannischen Reiches.\n\nEin Jahr ist seit Zeros Niederlage " +
                "vergangen. Japan ist noch immer unter der Kontrolle Britannias, in " +
                "der Kolonie ist Ruhe eingekehrt. Lelouch scheint die vergangenen Ereignisse " +
                "völlig vergessen zu haben und lebt friedlich mit seinem Bruder Rolo " +
                "in der Ashford-Akademie. Doch was ist bloß mit Lelouch während seines " +
                "Kampfes mit Suzaku geschehen? Dieses neue, auf Lügen basierende Leben " +
                "fällt jedoch in sich zusammen, als die Schwarzen Ritter das gigantische " +
                "Casino „Babel-Tower“ angreifen und damit Lelouchs Erinnerungen aus den " +
                "Tiefen seines Bewusstseins wiedererwecken …");
        }

        [Test]
        public async Task EnglishTitleTest()
        {
            ProxerResult<string> lEnglishTitleResult = await this._animeMangaObject.EnglishTitle;
            Assert.IsTrue(lEnglishTitleResult.Success);
            Assert.IsEmpty(lEnglishTitleResult.Exceptions);
            Assert.IsNotNull(lEnglishTitleResult.Result);
            Assert.AreEqual(lEnglishTitleResult.Result, "English title");
        }

        [Test]
        public async Task FskTest()
        {
            ProxerResult<IEnumerable<FskType>> lFskResult = await this._animeMangaObject.Fsk;
            Assert.IsTrue(lFskResult.Success);
            Assert.IsEmpty(lFskResult.Exceptions);
            Assert.IsNotNull(lFskResult.Result);
            Assert.AreEqual(lFskResult.Result.Count(), 2);
            Assert.IsTrue(lFskResult.Result.Contains(FskType.Fsk16));
            Assert.IsTrue(lFskResult.Result.Contains(FskType.Violence));
        }

        [Test]
        public async Task GenreTest()
        {
            ProxerResult<IEnumerable<GenreType>> lGenreResult = await this._animeMangaObject.Genre;
            Assert.IsTrue(lGenreResult.Success);
            Assert.IsEmpty(lGenreResult.Exceptions);
            Assert.IsNotNull(lGenreResult.Result);
            Assert.IsTrue(lGenreResult.Result.Contains(GenreType.Action));
            Assert.IsTrue(lGenreResult.Result.Contains(GenreType.Drama));
            Assert.IsTrue(lGenreResult.Result.Contains(GenreType.Mecha));
            Assert.IsTrue(lGenreResult.Result.Contains(GenreType.Military));
            Assert.IsTrue(lGenreResult.Result.Contains(GenreType.Psychological));
            Assert.IsTrue(lGenreResult.Result.Contains(GenreType.Romance));
            Assert.IsTrue(lGenreResult.Result.Contains(GenreType.School));
            Assert.IsTrue(lGenreResult.Result.Contains(GenreType.SciFi));
            Assert.IsTrue(lGenreResult.Result.Contains(GenreType.Superpower));
            Assert.IsTrue(lGenreResult.Result.Contains(GenreType.Violence));
        }

        [Test]
        public async Task GermanTitleTest()
        {
            ProxerResult<string> lGermanTitleResult = await this._animeMangaObject.GermanTitle;
            Assert.IsTrue(lGermanTitleResult.Success);
            Assert.IsEmpty(lGermanTitleResult.Exceptions);
            Assert.IsNotNull(lGermanTitleResult.Result);
            Assert.AreEqual(lGermanTitleResult.Result, "Deutscher Titel ü");
        }

        [Test]
        public async Task IsHContentTest()
        {
            ProxerResult<bool> lIsLicensedResult = await this._animeMangaObject.IsHContent;
            Assert.IsTrue(lIsLicensedResult.Success);
            Assert.IsEmpty(lIsLicensedResult.Exceptions);
            Assert.AreEqual(lIsLicensedResult.Result, false);
        }

        [Test]
        public async Task IsLicensedTest()
        {
            ProxerResult<bool> lIsLicensedResult = await this._animeMangaObject.IsLicensed.GetNewObject();
            Assert.IsTrue(lIsLicensedResult.Success);
            Assert.IsEmpty(lIsLicensedResult.Exceptions);
            Assert.AreEqual(lIsLicensedResult.Result, true);
        }

        [Test]
        public async Task JapaneseTitleTest()
        {
            ProxerResult<string> lJapaneseTitleResult = await this._animeMangaObject.JapaneseTitle;
            Assert.IsTrue(lJapaneseTitleResult.Success);
            Assert.IsEmpty(lJapaneseTitleResult.Exceptions);
            Assert.IsNotNull(lJapaneseTitleResult.Result);
            Assert.AreEqual(lJapaneseTitleResult.Result, "コ japanese title");
        }

        [Test]
        public async Task NameTest()
        {
            ProxerResult<string> lNameResult = await this._animeMangaObject.Name;
            Assert.IsTrue(lNameResult.Success);
            Assert.IsEmpty(lNameResult.Exceptions);
            Assert.IsNotNull(lNameResult.Result);
            Assert.AreEqual(lNameResult.Result, "Code Geass: Hangyaku no Lelouch R2");
        }

        [Test]
        public async Task RatingTest()
        {
            ProxerResult<AnimeMangaRating> lRatingResult = await this._animeMangaObject.Rating;
            Assert.IsTrue(lRatingResult.Success);
            Assert.IsEmpty(lRatingResult.Exceptions);
            Assert.IsNotNull(lRatingResult.Result);
            Assert.AreEqual(lRatingResult.Result.Voters, 6949);
            Assert.AreEqual(lRatingResult.Result.Rating, 65634/(decimal) 6949);
        }

        [Test]
        public async Task RelationsTest()
        {
            ProxerResult<IEnumerable<IAnimeMangaObject>> lRelationsResult = await this._animeMangaObject.Relations;
            Assert.IsTrue(lRelationsResult.Success);
            Assert.IsEmpty(lRelationsResult.Exceptions);
            Assert.IsNotNull(lRelationsResult.Result);
            Assert.AreEqual(lRelationsResult.Result.Count(), 29);
            Assert.IsTrue(lRelationsResult.Result.Any(o => o is Anime));
            Assert.IsTrue(lRelationsResult.Result.Any(o => o is Manga));
            Assert.IsTrue(lRelationsResult.Result.All(o => o.Id != default(int)));
        }

        [Test]
        public async Task SeasonTest()
        {
            ProxerResult<AnimeMangaSeasonInfo> lSeasonResult = await this._animeMangaObject.Season;
            Assert.IsTrue(lSeasonResult.Success);
            Assert.IsEmpty(lSeasonResult.Exceptions);
            Assert.IsNotNull(lSeasonResult.Result);
            Assert.IsNotNull(lSeasonResult.Result.StartSeason);
            Assert.AreEqual(lSeasonResult.Result.StartSeason.Season, Season.Spring);
            Assert.AreEqual(lSeasonResult.Result.StartSeason.Year, 2008);
            Assert.IsNotNull(lSeasonResult.Result.EndSeason);
            Assert.AreEqual(lSeasonResult.Result.EndSeason.Season, Season.Summer);
            Assert.AreEqual(lSeasonResult.Result.EndSeason.Year, 2008);
        }

        [Test]
        public async Task StatusTest()
        {
            ProxerResult<AnimeMangaStatus> lStatusResult = await this._animeMangaObject.Status;
            Assert.IsTrue(lStatusResult.Success);
            Assert.IsEmpty(lStatusResult.Exceptions);
            Assert.AreEqual(lStatusResult.Result, AnimeMangaStatus.Completed);
        }

        [Test]
        public async Task SynonymTest()
        {
            ProxerResult<string> lSynonymResult = await this._animeMangaObject.Synonym;
            Assert.IsTrue(lSynonymResult.Success);
            Assert.IsEmpty(lSynonymResult.Exceptions);
            Assert.IsNotNull(lSynonymResult.Result);
            Assert.AreEqual(lSynonymResult.Result, string.Empty);
        }

        [Test]
        public async Task TagsTest()
        {
            ProxerResult<IEnumerable<Tag>> lTagsResult = await this._animeMangaObject.Tags;
            Assert.IsTrue(lTagsResult.Success);
            Assert.IsEmpty(lTagsResult.Exceptions);
            Assert.IsNotNull(lTagsResult.Result);
            Assert.AreEqual(lTagsResult.Result.Count(), 10);
            Assert.IsTrue(lTagsResult.Result.Any(tag => tag.IsRated));
            Assert.IsTrue(lTagsResult.Result.Any(tag => !tag.IsRated));
            Assert.IsTrue(lTagsResult.Result.Any(tag => tag.IsSpoiler));
            Assert.IsTrue(lTagsResult.Result.Any(tag => !tag.IsSpoiler));
            Assert.IsTrue(lTagsResult.Result.All(tag => !string.IsNullOrEmpty(tag.Description)));
        }

        [Test]
        public async Task TranslatorsTest()
        {
            ProxerResult<IEnumerable<Translator>> lGroupResult = await this._animeMangaObject.Groups;
            Assert.IsTrue(lGroupResult.Success);
            Assert.IsEmpty(lGroupResult.Exceptions);
            Assert.IsNotNull(lGroupResult.Result);
            Assert.AreEqual(lGroupResult.Result.Count(), 2);
            Assert.IsTrue(
                lGroupResult.Result.Any(
                    translator =>
                        (translator.Language == Language.English) && (translator.Id == 795) &&
                        translator.Name.Equals("Dicescans")));
            Assert.IsTrue(
                lGroupResult.Result.Any(
                    translator =>
                        (translator.Language == Language.German) && (translator.Id == 11) &&
                        translator.Name.Equals("Gruppe Kampfkuchen")));
        }
    }
}