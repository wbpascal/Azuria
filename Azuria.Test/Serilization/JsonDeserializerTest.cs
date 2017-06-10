using System.Threading.Tasks;
using Autofac;
using Azuria.ErrorHandling;
using Azuria.Serialization;
using Azuria.Test.Core;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Azuria.Test.Serilization
{
    public class JsonDeserializerTest
    {
        private readonly IJsonDeserializer _jsonDeserializer;

        public JsonDeserializerTest()
        {
            IProxerClient lClient = ProxerClient.Create(new char[32]);
            this._jsonDeserializer = lClient.Container.Resolve<IJsonDeserializer>();
        }

        [Fact]
        public async Task DeserializeTest()
        {
            IProxerResult<ProxerApiResponse<string>> lDeserializeResult =
                await this._jsonDeserializer.Deserialize<ProxerApiResponse<string>>(
                    TestConstants.DummySuccessResponseString, null
                );

            Assert.True(lDeserializeResult.Success);
            Assert.Empty(lDeserializeResult.Exceptions);
            Assert.NotNull(lDeserializeResult.Result);
            Assert.True(lDeserializeResult.Result.Success);
            Assert.Empty(lDeserializeResult.Result.Exceptions);
            Assert.Equal("dataValue", lDeserializeResult.Result.Result);
        }

        [Fact]
        public async Task DeserializeWithSettingsTest()
        {
            JsonSerializerSettings lSettings = new JsonSerializerSettings();
            lSettings.Converters.Add(new TestConverter());

            IProxerResult<ProxerApiResponse<int>> lDeserializeResult =
                await this._jsonDeserializer.Deserialize<ProxerApiResponse<int>>(
                    TestConstants.DummySuccessResponseString, lSettings
                );

            Assert.True(lDeserializeResult.Success);
            Assert.Empty(lDeserializeResult.Exceptions);
            Assert.NotNull(lDeserializeResult.Result);
            Assert.True(lDeserializeResult.Result.Success);
            Assert.Empty(lDeserializeResult.Result.Exceptions);
            Assert.Equal(42, lDeserializeResult.Result.Result);
        }
    }
}