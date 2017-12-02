using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1.Converters;
using Azuria.Requests;
using Azuria.Requests.Builder;
using Moq;
using NUnit.Framework;

namespace Azuria.Test.Requests
{
    [TestFixture]
    public class RequestBuilderWithResultTest
    {
        private const string BaseUrl = "https://proxer.me/";
        private readonly IProxerClient _client;

        public RequestBuilderWithResultTest()
        {
            this._client = ProxerClient.Create(new char[32]);
        }

        [Test]
        public void WithCustomDataConverterTest()
        {
            IRequestBuilderWithResult<object> lRequestBuilder =
                this._client.CreateRequest().FromUrl(new Uri(BaseUrl)).WithResult<object>();
            DataConverter<object> lDataConverter = Mock.Of<DataConverter<object>>();
            lRequestBuilder.WithCustomDataConverter(lDataConverter);
            Assert.AreSame(lDataConverter, lRequestBuilder.CustomDataConverter);
        }

        [Test]
        public void WithGetParameterTest()
        {
            IRequestBuilderWithResult<object> lRequestBuilder =
                this._client.CreateRequest().FromUrl(new Uri(BaseUrl)).WithResult<object>();
            lRequestBuilder.WithGetParameter("test", "value1");
            Assert.AreEqual(1, lRequestBuilder.GetParameters.Count);
            Assert.True(lRequestBuilder.GetParameters.ContainsKey("test"));
            Assert.AreEqual("value1", lRequestBuilder.GetParameters["test"]);

            lRequestBuilder.WithGetParameter("test", "value2");
            Assert.AreEqual(1, lRequestBuilder.GetParameters.Count);
            Assert.True(lRequestBuilder.GetParameters.ContainsKey("test"));
            Assert.AreEqual("value2", lRequestBuilder.GetParameters["test"]);

            lRequestBuilder.WithGetParameter(
                new Dictionary<string, string>
                {
                    {"test2", "value3"},
                    {"testNew", "value"}
                });
            Assert.AreEqual(3, lRequestBuilder.GetParameters.Count);
            Assert.True(lRequestBuilder.GetParameters.ContainsKey("test"));
            Assert.True(lRequestBuilder.GetParameters.ContainsKey("test2"));
            Assert.True(lRequestBuilder.GetParameters.ContainsKey("testNew"));
            Assert.AreEqual("value2", lRequestBuilder.GetParameters["test"]);
            Assert.AreEqual("value3", lRequestBuilder.GetParameters["test2"]);
            Assert.AreEqual("value", lRequestBuilder.GetParameters["testNew"]);
        }

        [Test]
        public void WithLoginCheckTest()
        {
            IRequestBuilderWithResult<object> lRequestBuilder =
                this._client.CreateRequest().FromUrl(new Uri(BaseUrl)).WithResult<object>();
            lRequestBuilder.WithLoginCheck(true);
            Assert.True(lRequestBuilder.CheckLogin);
            lRequestBuilder.WithLoginCheck(false);
            Assert.False(lRequestBuilder.CheckLogin);
        }

        [Test]
        public void WithPostParameterTest()
        {
            IRequestBuilderWithResult<object> lRequestBuilder =
                this._client.CreateRequest().FromUrl(new Uri(BaseUrl)).WithResult<object>();
            lRequestBuilder.WithPostParameter("test", "value1");
            Assert.AreEqual(1, lRequestBuilder.PostParameter.Count());
            Assert.True(lRequestBuilder.PostParameter.Any(pair => pair.Key == "test" && pair.Value == "value1"));

            lRequestBuilder.WithPostParameter("test", "value2");
            Assert.AreEqual(2, lRequestBuilder.PostParameter.Count());
            Assert.True(lRequestBuilder.PostParameter.Any(pair => pair.Key == "test" && pair.Value == "value1"));
            Assert.True(lRequestBuilder.PostParameter.Any(pair => pair.Key == "test" && pair.Value == "value2"));

            lRequestBuilder.WithPostParameter(
                new Dictionary<string, string>
                {
                    {"test2", "value3"},
                    {"testNew", "value"}
                });
            Assert.AreEqual(4, lRequestBuilder.PostParameter.Count());
            Assert.True(lRequestBuilder.PostParameter.Any(pair => pair.Key == "test" && pair.Value == "value1"));
            Assert.True(lRequestBuilder.PostParameter.Any(pair => pair.Key == "test" && pair.Value == "value2"));
            Assert.True(lRequestBuilder.PostParameter.Any(pair => pair.Key == "test2" && pair.Value == "value3"));
            Assert.True(lRequestBuilder.PostParameter.Any(pair => pair.Key == "testNew" && pair.Value == "value"));
        }
    }
}