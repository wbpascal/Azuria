using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class AnimeTest
    {
        private Anime _anime;

        [OneTimeSetUp]
        public async Task Setup()
        {
            this._anime = await MediaObject.CreateFromId(9200).ThrowFirstForNonSuccess() as Anime;
            Assert.IsNotNull(this._anime);
        }

        [Test]
        public async Task AnimeMediumTest()
        {
            IProxerResult<AnimeMedium> lResult = await this._anime.AnimeMedium;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.AreEqual(AnimeMedium.Series, lResult.Result);
        }

        [Test]
        public async Task AvailableLanguagesTest()
        {
            IProxerResult<IEnumerable<AnimeLanguage>> lResult = await this._anime.AvailableLanguages;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.IsNotEmpty(lResult.Result);
            Assert.IsTrue(lResult.Result.Contains(AnimeLanguage.EngSub));
            Assert.IsTrue(lResult.Result.Contains(AnimeLanguage.GerSub));
        }

        [Test]
        public void CommentsLatestTest()
        {
            Comment<Anime>[] lCommentsLatest = this._anime.CommentsLatest.ToArray();
            Assert.IsNotEmpty(lCommentsLatest);
            Assert.AreEqual(24, lCommentsLatest.Length);
        }

        [Test]
        public void CommentsRatingTest()
        {
            Comment<Anime>[] lCommentsRating = this._anime.CommentsRating.ToArray();
            Assert.IsNotEmpty(lCommentsRating);
            Assert.AreEqual(28, lCommentsRating.Length);
        }


        [Test]
        public async Task GetEpisodesTest()
        {
            Assert.CatchAsync<LanguageNotAvailableException>(
                () => this._anime.GetEpisodes(AnimeLanguage.EngDub).ThrowFirstForNonSuccess());

            IProxerResult<IEnumerable<Episode>> lResult = await this._anime.GetEpisodes(AnimeLanguage.EngSub);
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(22, lResult.Result.Count());
            Assert.IsTrue(lResult.Result.All(episode => episode.Language == AnimeLanguage.EngSub));
            Assert.IsTrue(lResult.Result.All(episode => episode.ParentObject == this._anime));
        }
    }
}