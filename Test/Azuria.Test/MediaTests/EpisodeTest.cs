using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Media;
using Azuria.Media.Properties;
using Azuria.Utilities.Extensions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Azuria.Test.MediaTests
{
    [TestFixture]
    public class EpisodeTest
    {
        private Anime.Episode _episode;

        [OneTimeSetUp]
        public async Task Setup()
        {
            Anime lAnime = await AnimeMangaObject.CreateFromId(9200).ThrowFirstForNonSuccess() as Anime;
            Assert.IsNotNull(lAnime);
            this._episode = (await lAnime.GetEpisodes(AnimeLanguage.EngSub).ThrowFirstForNonSuccess()).FirstOrDefault();
            Assert.IsNotNull(this._episode);
        }

        [Test]
        public void ContentIndexTest()
        {
            Assert.AreEqual(this._episode.ContentIndex, 1);
        }

        [Test]
        public void LanguageTest()
        {
            Assert.AreEqual(this._episode.Language, AnimeLanguage.EngSub);
            Assert.AreEqual(this._episode.GeneralLanguage, Language.English);
        }

        [Test]
        public void ParentObjectTest()
        {
            Assert.IsNotNull(this._episode.ParentObject);
            Assert.AreEqual(this._episode.ParentObject.Id, 9200);
        }

        [Test]
        public async Task StreamsTest()
        {
            ProxerResult<IEnumerable<Anime.Episode.Stream>> lResult = await this._episode.Streams;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(lResult.Result.Count(), 4);
        }
    }
}