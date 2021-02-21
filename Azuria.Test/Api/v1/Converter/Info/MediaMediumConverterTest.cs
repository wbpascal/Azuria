using Azuria.Api.v1.Converter;
using Azuria.Enums;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.Converter.Info
{
    public class MediaMediumConverterTest : DataConverterTestBase<MediaMedium>
    {
        /// <inheritdoc />
        public MediaMediumConverterTest() : base(new MediumConverter())
        {
        }

        private void ConvertTest(string toConvert, MediaMedium result)
        {
            MediaMedium lValue = this.DeserializeValue($"'{toConvert}'");
            Assert.AreEqual(result, lValue);
        }

        [Test]
        public void CanConvertAnimeseriesTest()
        {
            this.ConvertTest("animeseries", MediaMedium.Animeseries);
        }

        [Test]
        public void CanConvertMovieTest()
        {
            this.ConvertTest("movie", MediaMedium.Movie);
        }

        [Test]
        public void CanConvertOvaTest()
        {
            this.ConvertTest("ova", MediaMedium.Ova);
        }

        [Test]
        public void CanConvertHentaiTest()
        {
            this.ConvertTest("hentai", MediaMedium.Hentai);
        }

        [Test]
        public void CanConvertMangaseriesTest()
        {
            this.ConvertTest("mangaseries", MediaMedium.Mangaseries);
        }

        [Test]
        public void CanConvertOneshotTest()
        {
            this.ConvertTest("oneshot", MediaMedium.OneShot);
        }

        [Test]
        public void CanConvertDoujinTest()
        {
            this.ConvertTest("doujin", MediaMedium.Doujin);
        }

        [Test]
        public void CanConvertHmangaTest()
        {
            this.ConvertTest("hmanga", MediaMedium.HManga);
        }

        [Test]
        public void CanConvertLightNovelTest()
        {
            this.ConvertTest("lightnovel", MediaMedium.LightNovel);
        }

        [Test]
        public void CanConvertWebNovelTest()
        {
            this.ConvertTest("webnovel", MediaMedium.WebNovel);
        }

        [Test]
        public void CanConvertVisualNovelTest()
        {
            this.ConvertTest("visualnovel", MediaMedium.VisualNovel);
        }
    }
}