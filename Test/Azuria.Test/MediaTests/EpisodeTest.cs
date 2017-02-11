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
        private Episode _episode;

        [OneTimeSetUp]
        public async Task Setup()
        {
            Anime lAnime = await MediaObject.CreateFromId(9200).ThrowFirstForNonSuccess() as Anime;
            Assert.IsNotNull(lAnime);
            this._episode = (await lAnime.GetEpisodes(AnimeLanguage.EngSub).ThrowFirstForNonSuccess()).FirstOrDefault();
            Assert.IsNotNull(this._episode);
        }

        [Test]
        public void ContentIndexTest()
        {
            Assert.AreEqual(1, this._episode.ContentIndex);
        }

        [Test]
        public void LanguageTest()
        {
            Assert.AreEqual(AnimeLanguage.EngSub, this._episode.Language);
            Assert.AreEqual(Language.English, this._episode.GeneralLanguage);
        }

        [Test]
        public void ParentObjectTest()
        {
            Assert.IsNotNull(this._episode.ParentObject);
            Assert.AreEqual(9200, this._episode.ParentObject.Id);
        }

        [Test]
        public async Task StreamsTest()
        {
            IProxerResult<IEnumerable<Stream>> lResult = await this._episode.Streams;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(4, lResult.Result.Count());
        }
    }
}