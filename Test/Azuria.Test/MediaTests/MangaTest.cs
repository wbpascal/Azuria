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
    public class MangaTest
    {
        private Manga _manga;

        [OneTimeSetUp]
        public async Task Setup()
        {
            this._manga = await MediaObject.CreateFromId(7834).ThrowFirstForNonSuccess() as Manga;
            Assert.IsNotNull(this._manga);
        }

        [Test]
        public async Task AvailableLanguagesTest()
        {
            IProxerResult<IEnumerable<Language>> lResult = await this._manga.AvailableLanguages;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.IsNotEmpty(lResult.Result);
            Assert.IsTrue(lResult.Result.Contains(Language.English));
            Assert.IsFalse(lResult.Result.Contains(Language.German));
            Assert.IsFalse(lResult.Result.Contains(Language.Unkown));
        }

        [Test]
        public async Task GetChaptersTest()
        {
            IProxerResult<IEnumerable<Manga.Chapter>> lResult = await this._manga.GetChapters(Language.English);
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(162, lResult.Result.Count());
            Assert.IsTrue(lResult.Result.All(chapter => chapter.Language == Language.English));
        }

        [Test]
        public async Task MangaMediumTest()
        {
            IProxerResult<MangaMedium> lResult = await this._manga.MangaMedium.GetNewObject();
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.AreEqual(MangaMedium.Series, lResult.Result);
        }
    }
}