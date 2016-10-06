using System;
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
    public class StreamTest
    {
        private Anime.Episode.Stream _stream;

        [OneTimeSetUp]
        public async Task Setup()
        {
            Anime lAnime = await AnimeMangaObject.CreateFromId(9200).ThrowFirstForNonSuccess() as Anime;
            Assert.IsNotNull(lAnime);
            Anime.Episode lEpisode =
                (await lAnime.GetEpisodes(AnimeLanguage.EngSub).ThrowFirstForNonSuccess()).FirstOrDefault();
            Assert.IsNotNull(lEpisode);
            this._stream = (await lEpisode.Streams.ThrowFirstOnNonSuccess()).FirstOrDefault();
            Assert.IsNotNull(this._stream);
        }

        [Test]
        public void EpisodeTest()
        {
            Assert.IsNotNull(this._stream.Episode);
            Assert.AreEqual(this._stream.Episode.ContentIndex, 1);
            Assert.IsNotNull(this._stream.Episode.ParentObject);
            Assert.AreEqual(this._stream.Episode.ParentObject.Id, 9200);
        }

        [Test]
        public void HosterTest()
        {
            Assert.AreEqual(this._stream.Hoster, StreamHoster.Mp4Upload);
            Assert.IsNotNull(this._stream.HosterFullName);
            Assert.IsNotEmpty(this._stream.HosterFullName);
            Assert.IsNotNull(this._stream.HosterImage);
            Assert.IsTrue(this._stream.HosterImage.AbsoluteUri.EndsWith(".png"));
            Assert.IsNotNull(this._stream.HostingType);
            Assert.IsNotEmpty(this._stream.HostingType);
        }

        [Test]
        public void IdTest()
        {
            Assert.AreNotEqual(this._stream.Id, default(int));
        }

        [Test]
        public async Task LinkTest()
        {
            ProxerResult<Uri> lResult = await this._stream.Link;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.IsTrue(lResult.Result.AbsoluteUri.Contains("mp4upload.com"));
        }

        [Test]
        public void TranslatorTest()
        {
            Assert.IsNotNull(this._stream.Translator);
            Assert.AreEqual(this._stream.Translator.Id, 1158);
            Assert.AreEqual(this._stream.Translator.Name, "THORAnime");
            Assert.AreEqual(this._stream.Translator.Language, Language.English);
        }

        [Test]
        public void UploadDateTest()
        {
            Assert.AreNotEqual(this._stream.UploadDate, DateTime.MinValue);
            Assert.AreNotEqual(this._stream.UploadDate, DateTime.MaxValue);
        }

        [Test]
        public async Task UploaderTest()
        {
            Assert.IsNotNull(this._stream.Uploader);
            Assert.AreEqual(this._stream.Uploader.Id, 205400);
            Assert.AreEqual(await this._stream.Uploader.UserName.ThrowFirstOnNonSuccess(), "Tadakuni");
        }
    }
}