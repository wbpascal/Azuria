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
            Assert.AreEqual(1, this._stream.Episode.ContentIndex);
            Assert.IsNotNull(this._stream.Episode.ParentObject);
            Assert.AreEqual(9200, this._stream.Episode.ParentObject.Id);
        }

        [Test]
        public void HosterTest()
        {
            Assert.AreEqual(StreamHoster.Mp4Upload, this._stream.Hoster);
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
            Assert.AreEqual(1158, this._stream.Translator.Id);
            Assert.AreEqual("THORAnime", this._stream.Translator.Name);
            Assert.AreEqual(Language.English, this._stream.Translator.Language);
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
            Assert.AreEqual(205400, this._stream.Uploader.Id);
            Assert.AreEqual("Tadakuni", await this._stream.Uploader.UserName.ThrowFirstOnNonSuccess());
        }
    }
}