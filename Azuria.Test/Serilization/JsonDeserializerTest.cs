using System.Collections.Generic;
using Azuria.ErrorHandling;
using Azuria.Serialization;
using Azuria.Test.Core;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Azuria.Test.Serilization
{
    [TestFixture]
    public class JsonDeserializerTest
    {
        private readonly IJsonDeserializer _jsonDeserializer;

        public JsonDeserializerTest()
        {
            this._jsonDeserializer = new JsonDeserializer();
        }

        [Test]
        public void DeserializeTest()
        {
            IProxerResult<ProxerApiResponse<string>> lDeserializeResult =
                this._jsonDeserializer.Deserialize<ProxerApiResponse<string>>(
                    TestConstants.DummySuccessResponseString, null
                );

            Assert.True(lDeserializeResult.Success);
            Assert.IsEmpty(lDeserializeResult.Exceptions);
            Assert.NotNull(lDeserializeResult.Result);
            Assert.True(lDeserializeResult.Result.Success);
            Assert.IsEmpty(lDeserializeResult.Result.Exceptions);
            Assert.AreEqual("dataValue", lDeserializeResult.Result.Result);
        }

        [Test]
        public void DeserializeWithSettingsTest()
        {
            var lSettings = new JsonSerializerSettings();
            lSettings.Converters.Add(new TestConverter());

            IProxerResult<ProxerApiResponse<int>> lDeserializeResult =
                this._jsonDeserializer.Deserialize<ProxerApiResponse<int>>(
                    TestConstants.DummySuccessResponseString, lSettings
                );

            Assert.True(lDeserializeResult.Success);
            Assert.IsEmpty(lDeserializeResult.Exceptions);
            Assert.NotNull(lDeserializeResult.Result);
            Assert.True(lDeserializeResult.Result.Success);
            Assert.IsEmpty(lDeserializeResult.Result.Exceptions);
            Assert.AreEqual(42, lDeserializeResult.Result.Result);
        }

        [Test]
        public void DeserializeInvalidJsonTest()
        {
            const string lInvalidJson = "{\"test:}";
            IProxerResult<Dictionary<string, string>> lDeserializeResult =
                this._jsonDeserializer.Deserialize<Dictionary<string, string>>(lInvalidJson, null);
            Assert.False(lDeserializeResult.Success);
            Assert.Null(lDeserializeResult.Result);
            Assert.NotNull(lDeserializeResult.Exceptions);
            Assert.IsNotEmpty(lDeserializeResult.Exceptions);
        }
    }
}