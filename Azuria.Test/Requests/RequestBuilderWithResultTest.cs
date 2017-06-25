using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1.Converters;
using Azuria.Requests;
using Azuria.Requests.Builder;
using Moq;
using Xunit;

namespace Azuria.Test.Requests
{
    public class RequestBuilderWithResultTest
    {
        private const string BaseUrl = "https://proxer.me/";
        private readonly IProxerClient _client;

        public RequestBuilderWithResultTest()
        {
            this._client = ProxerClient.Create(new char[32]);
        }

        [Fact]
        public void WithCustomDataConverterTest()
        {
            IRequestBuilderWithResult<object> lRequestBuilder =
                this._client.CreateRequest().FromUrl(new Uri(BaseUrl)).WithResult<object>();
            DataConverter<object> lDataConverter = Mock.Of<DataConverter<object>>();
            lRequestBuilder.WithCustomDataConverter(lDataConverter);
            Assert.Same(lDataConverter, lRequestBuilder.CustomDataConverter);
        }

        [Fact]
        public void WithGetParameterTest()
        {
            IRequestBuilderWithResult<object> lRequestBuilder =
                this._client.CreateRequest().FromUrl(new Uri(BaseUrl)).WithResult<object>();
            lRequestBuilder.WithGetParameter("test", "value1");
            Assert.Equal(1, lRequestBuilder.GetParameters.Count);
            Assert.True(lRequestBuilder.GetParameters.ContainsKey("test"));
            Assert.Equal("value1", lRequestBuilder.GetParameters["test"]);

            lRequestBuilder.WithGetParameter("test", "value2");
            Assert.Equal(1, lRequestBuilder.GetParameters.Count);
            Assert.True(lRequestBuilder.GetParameters.ContainsKey("test"));
            Assert.Equal("value2", lRequestBuilder.GetParameters["test"]);

            lRequestBuilder.WithGetParameter(
                new Dictionary<string, string>
                {
                    {"test2", "value3"},
                    {"testNew", "value"}
                });
            Assert.Equal(3, lRequestBuilder.GetParameters.Count);
            Assert.True(lRequestBuilder.GetParameters.ContainsKey("test"));
            Assert.True(lRequestBuilder.GetParameters.ContainsKey("test2"));
            Assert.True(lRequestBuilder.GetParameters.ContainsKey("testNew"));
            Assert.Equal("value2", lRequestBuilder.GetParameters["test"]);
            Assert.Equal("value3", lRequestBuilder.GetParameters["test2"]);
            Assert.Equal("value", lRequestBuilder.GetParameters["testNew"]);
        }

        [Fact]
        public void WithLoginCheckTest()
        {
            IRequestBuilderWithResult<object> lRequestBuilder =
                this._client.CreateRequest().FromUrl(new Uri(BaseUrl)).WithResult<object>();
            lRequestBuilder.WithLoginCheck(true);
            Assert.True(lRequestBuilder.CheckLogin);
            lRequestBuilder.WithLoginCheck(false);
            Assert.False(lRequestBuilder.CheckLogin);
        }

        [Fact]
        public void WithPostParameterTest()
        {
            IRequestBuilderWithResult<object> lRequestBuilder =
                this._client.CreateRequest().FromUrl(new Uri(BaseUrl)).WithResult<object>();
            lRequestBuilder.WithPostParameter("test", "value1");
            Assert.Equal(1, lRequestBuilder.PostArguments.Count());
            Assert.True(lRequestBuilder.PostArguments.Any(pair => pair.Key == "test" && pair.Value == "value1"));

            lRequestBuilder.WithPostParameter("test", "value2");
            Assert.Equal(2, lRequestBuilder.PostArguments.Count());
            Assert.True(lRequestBuilder.PostArguments.Any(pair => pair.Key == "test" && pair.Value == "value1"));
            Assert.True(lRequestBuilder.PostArguments.Any(pair => pair.Key == "test" && pair.Value == "value2"));

            lRequestBuilder.WithPostParameter(
                new Dictionary<string, string>
                {
                    {"test2", "value3"},
                    {"testNew", "value"}
                });
            Assert.Equal(4, lRequestBuilder.PostArguments.Count());
            Assert.True(lRequestBuilder.PostArguments.Any(pair => pair.Key == "test" && pair.Value == "value1"));
            Assert.True(lRequestBuilder.PostArguments.Any(pair => pair.Key == "test" && pair.Value == "value2"));
            Assert.True(lRequestBuilder.PostArguments.Any(pair => pair.Key == "test2" && pair.Value == "value3"));
            Assert.True(lRequestBuilder.PostArguments.Any(pair => pair.Key == "testNew" && pair.Value == "value"));
        }
    }
}