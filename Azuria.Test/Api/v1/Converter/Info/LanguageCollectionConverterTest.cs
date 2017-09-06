using Azuria.Api.v1.Converters.Info;
using Azuria.Enums.Info;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.Converter.Info
{
    [TestFixture]
    public class LanguageCollectionConverterTest : DataConverterTestBase<MediaLanguage[]>
    {
        /// <inheritdoc />
        public LanguageCollectionConverterTest() : base(new LanguageCollectionConverter())
        {
        }

        [Test]
        public void CanConvertTest()
        {
            const string lJson = "['de','gerdub','gersub','en','engsub','engdub']";
            MediaLanguage[] lValue = this.DeserializeValue(lJson);
            Assert.AreEqual(6, lValue.Length);
            Assert.Contains(MediaLanguage.German, lValue);
            Assert.Contains(MediaLanguage.GerDub, lValue);
            Assert.Contains(MediaLanguage.GerSub, lValue);
            Assert.Contains(MediaLanguage.English, lValue);
            Assert.Contains(MediaLanguage.EngSub, lValue);
            Assert.Contains(MediaLanguage.EngDub, lValue);
        }
    }
}