using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Media;
using Azuria.Media.Properties;
using Azuria.UserInfo.Comment;
using Azuria.Utilities.Extensions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Azuria.Test.MediaTests
{
    [TestFixture]
    public class MediaObjectTest
    {
        private MediaObject _mediaObject;

        [OneTimeSetUp]
        public async Task Setup()
        {
            this._mediaObject =
                await MediaObject.CreateFromId(9200).ThrowFirstForNonSuccess() as MediaObject;
            Assert.IsNotNull(this._mediaObject);
        }

        [Test]
        public async Task ClicksTest()
        {
            IProxerResult<int> lResult = await this._mediaObject.Clicks;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.AreEqual(23604, lResult.Result);
        }

        [Test]
        public void CommentsLatestTest()
        {
            Comment<IMediaObject>[] lCommentsLatest = this._mediaObject.CommentsLatest.ToArray();
            Assert.IsNotEmpty(lCommentsLatest);
            Assert.AreEqual(24, lCommentsLatest.Length);
        }

        [Test]
        public void CommentsRatingTest()
        {
            Comment<IMediaObject>[] lCommentsRating = this._mediaObject.CommentsRating.ToArray();
            Assert.IsNotEmpty(lCommentsRating);
            Assert.AreEqual(28, lCommentsRating.Length);
        }

        [Test]
        public async Task ContentCountTest()
        {
            IProxerResult<int> lResult = await this._mediaObject.ContentCount;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.AreEqual(22, lResult.Result);
        }

        [Test]
        public void CoverUriTest()
        {
            Uri lCoverUri = this._mediaObject.CoverUri;
            Assert.IsNotNull(lCoverUri);
            Assert.IsTrue(lCoverUri.AbsoluteUri.EndsWith(".jpg"));
        }

        [Test]
        public async Task CreateFromIdTest()
        {
            Assert.CatchAsync<ArgumentException>(() => MediaObject.CreateFromId(-1).ThrowFirstForNonSuccess());

            IMediaObject lValidObject = await MediaObject.CreateFromId(9200).ThrowFirstForNonSuccess();
            Assert.IsNotNull(lValidObject);
            Assert.AreEqual(9200, lValidObject.Id);

            Assert.AreEqual(ErrorCode.InfoInvalidId,
                Assert.CatchAsync<ProxerApiException>(() => MediaObject.CreateFromId(666).ThrowFirstForNonSuccess())
                    .ErrorCode);
        }

        [Test]
        public async Task DescriptionTest()
        {
            IProxerResult<string> lResult = await this._mediaObject.Description;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual("Description Text", lResult.Result);
        }

        [Test]
        public async Task EnglishTitleTest()
        {
            IProxerResult<string> lResult = await this._mediaObject.EnglishTitle;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.IsNotEmpty(lResult.Result);
        }

        [Test]
        public async Task FskTest()
        {
            IProxerResult<IEnumerable<FskType>> lResult = await this._mediaObject.Fsk;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(1, lResult.Result.Count());
            Assert.IsTrue(lResult.Result.Contains(FskType.Fsk12));
        }

        [Test]
        public async Task GenreTest()
        {
            IProxerResult<IEnumerable<GenreType>> lResult = await this._mediaObject.Genre;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.IsTrue(lResult.Result.Contains(GenreType.Comedy));
            Assert.IsTrue(lResult.Result.Contains(GenreType.Drama));
            Assert.IsTrue(lResult.Result.Contains(GenreType.Music));
            Assert.IsTrue(lResult.Result.Contains(GenreType.Romance));
            Assert.IsTrue(lResult.Result.Contains(GenreType.School));
            Assert.IsTrue(lResult.Result.Contains(GenreType.Shounen));
            Assert.IsTrue(lResult.Result.Contains(GenreType.SliceOfLife));
        }

        [Test]
        public async Task GermanTitleTest()
        {
            IProxerResult<string> lResult = await this._mediaObject.GermanTitle;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.IsNotEmpty(lResult.Result);
        }

        [Test]
        public async Task IndustryTest()
        {
            IProxerResult<IEnumerable<Industry>> lResult = await this._mediaObject.Industry;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(10, lResult.Result.Count());
            Assert.IsTrue(lResult.Result.Any(industry => industry.Type == IndustryType.Producer));
            Assert.IsTrue(lResult.Result.Any(industry => industry.Type == IndustryType.StreamPartner));
            Assert.IsTrue(lResult.Result.Any(industry => industry.Type == IndustryType.Publisher));
            Assert.IsTrue(lResult.Result.Any(industry => industry.Type == IndustryType.Studio));
            Assert.IsFalse(lResult.Result.Any(industry => industry.Type == IndustryType.Unknown));
            Assert.IsTrue(lResult.Result.Any(industry => industry.Country == Country.Germany));
            Assert.IsTrue(lResult.Result.Any(industry => industry.Country == Country.Japan));
            Assert.IsTrue(lResult.Result.Any(industry => industry.Country == Country.UnitedStates));
            Assert.IsFalse(lResult.Result.Any(industry => industry.Country == Country.Unkown));
        }

        [Test]
        public async Task IsHContentTest()
        {
            IProxerResult<bool> lResult = await this._mediaObject.IsHContent;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsFalse(lResult.Result);
        }

        [Test]
        public async Task IsLicensedTest()
        {
            IProxerResult<bool> lResult = await this._mediaObject.IsLicensed.GetNewObject();
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsTrue(lResult.Result);
        }

        [Test]
        public async Task JapaneseTitleTest()
        {
            IProxerResult<string> lResult = await this._mediaObject.JapaneseTitle;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.IsNotEmpty(lResult.Result);
        }

        [Test]
        public async Task NameTest()
        {
            IProxerResult<string> lResult = await this._mediaObject.Name;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.IsNotEmpty(lResult.Result);
        }

        [Test]
        public async Task RatingTest()
        {
            IProxerResult<MediaRating> lResult = await this._mediaObject.Rating;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(4371, lResult.Result.Voters);
            Assert.AreEqual(40527/(decimal) 4371, lResult.Result.Rating);
        }

        [Test]
        public async Task RelationsTest()
        {
            IProxerResult<IEnumerable<IMediaObject>> lResult = await this._mediaObject.Relations;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(4, lResult.Result.Count());
            Assert.IsTrue(lResult.Result.Any(o => o is Anime));
            Assert.IsTrue(lResult.Result.Any(o => o is Manga));
            Assert.IsTrue(lResult.Result.All(o => o.Id != default(int)));
        }

        [Test]
        public async Task SeasonTest()
        {
            IProxerResult<MediaSeasonInfo> lResult = await this._mediaObject.Season;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.IsNotNull(lResult.Result.StartSeason);
            Assert.AreEqual(Season.Autumn, lResult.Result.StartSeason.Season);
            Assert.AreEqual(2014, lResult.Result.StartSeason.Year);
            Assert.IsNotNull(lResult.Result.EndSeason);
            Assert.AreEqual(Season.Winter, lResult.Result.EndSeason.Season);
            Assert.AreEqual(2015, lResult.Result.EndSeason.Year);
        }

        [Test]
        public async Task StatusTest()
        {
            IProxerResult<MediaStatus> lResult = await this._mediaObject.Status;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.AreEqual(MediaStatus.Completed, lResult.Result);
        }

        [Test]
        public async Task SynonymTest()
        {
            IProxerResult<string> lResult = await this._mediaObject.Synonym;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.IsEmpty(lResult.Result);
        }

        [Test]
        public async Task TagsTest()
        {
            IProxerResult<IEnumerable<Tag>> lResult = await this._mediaObject.Tags;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(9, lResult.Result.Count());
            Assert.IsTrue(lResult.Result.Any(tag => tag.IsRated));
            Assert.IsTrue(lResult.Result.Any(tag => !tag.IsRated));
            Assert.IsTrue(lResult.Result.Any(tag => tag.IsSpoiler));
            Assert.IsTrue(lResult.Result.Any(tag => !tag.IsSpoiler));
            Assert.IsTrue(lResult.Result.All(tag => !string.IsNullOrEmpty(tag.Description)));
        }

        [Test]
        public async Task TranslatorsTest()
        {
            IProxerResult<IEnumerable<Translator>> lResult = await this._mediaObject.Groups;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(4, lResult.Result.Count());
            Assert.IsTrue(
                lResult.Result.Any(
                    translator =>
                        (translator.Language == Language.English) && (translator.Id == 795) &&
                        translator.Name.Equals("Dicescans")));
            Assert.IsTrue(
                lResult.Result.Any(
                    translator =>
                        (translator.Language == Language.German) && (translator.Id == 11) &&
                        translator.Name.Equals("Gruppe Kampfkuchen")));
        }
    }
}