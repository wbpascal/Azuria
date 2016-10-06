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
            this._anime = await AnimeMangaObject.CreateFromId(9200).ThrowFirstForNonSuccess() as Anime;
            Assert.IsNotNull(this._anime);
        }

        [Test]
        public async Task AnimeMediumTest()
        {
            ProxerResult<AnimeMedium> lResult = await this._anime.AnimeMedium;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.AreEqual(lResult.Result, AnimeMedium.Series);
        }

        [Test]
        public async Task AvailableLanguagesTest()
        {
            ProxerResult<IEnumerable<AnimeLanguage>> lResult = await this._anime.AvailableLanguages;
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
            Assert.AreEqual(lCommentsLatest.Length, 24);
        }

        [Test]
        public void CommentsRatingTest()
        {
            Comment<Anime>[] lCommentsRating = this._anime.CommentsRating.ToArray();
            Assert.IsNotEmpty(lCommentsRating);
            Assert.AreEqual(lCommentsRating.Length, 28);
        }


        [Test]
        public async Task GetEpisodesTest()
        {
            Assert.CatchAsync<LanguageNotAvailableException>(
                () => this._anime.GetEpisodes(AnimeLanguage.EngDub).ThrowFirstForNonSuccess());

            ProxerResult<IEnumerable<Anime.Episode>> lResult = await this._anime.GetEpisodes(AnimeLanguage.EngSub);
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(lResult.Result.Count(), 22);
            Assert.IsTrue(lResult.Result.All(episode => episode.Language == AnimeLanguage.EngSub));
            Assert.IsTrue(lResult.Result.All(episode => episode.ParentObject == this._anime));
        }
    }
}