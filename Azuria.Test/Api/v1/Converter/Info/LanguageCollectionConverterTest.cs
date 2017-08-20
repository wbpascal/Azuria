using Azuria.Api.v1.Converters.Info;
using Azuria.Enums.Info;
using Xunit;

namespace Azuria.Test.Api.v1.Converter.Info
{
    public class LanguageCollectionConverterTest : DataConverterTestBase
    {
        private LanguageCollectionConverter Converter { get; } = new LanguageCollectionConverter();

        [Fact]
        public void CanConvertTest()
        {
            const string lJson = "['de','gerdub','gersub','en','engsub','engdub']";
            MediaLanguage[] lValue = this.DeserializeValue(lJson, this.Converter);
            Assert.Equal(6, lValue.Length);
            Assert.Contains(MediaLanguage.German, lValue);
            Assert.Contains(MediaLanguage.GerDub, lValue);
            Assert.Contains(MediaLanguage.GerSub, lValue);
            Assert.Contains(MediaLanguage.English, lValue);
            Assert.Contains(MediaLanguage.EngSub, lValue);
            Assert.Contains(MediaLanguage.EngDub, lValue);
        }
    }
}