using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Requests;
using Azuria.Requests.Builder;
using Xunit;

namespace Azuria.Test.Requests
{
    public class RequestBuilderTest
    {
        private const string BaseUrl = "https://proxer.me/";
        private readonly IProxerClient _client;

        public RequestBuilderTest()
        {
            this._client = ProxerClient.Create(new char[32]);
        }

        [Fact]
        public void WithGetParameterTest()
        {
            IRequestBuilder lRequestBuilder = this._client.CreateRequest().FromUrl(new Uri(BaseUrl));
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
            IRequestBuilder lRequestBuilder = this._client.CreateRequest().FromUrl(new Uri(BaseUrl));
            lRequestBuilder.WithLoginCheck(true);
            Assert.True(lRequestBuilder.CheckLogin);
            lRequestBuilder.WithLoginCheck(false);
            Assert.False(lRequestBuilder.CheckLogin);
        }

        [Fact]
        public void WithPostParameterTest()
        {
            IRequestBuilder lRequestBuilder = this._client.CreateRequest().FromUrl(new Uri(BaseUrl));
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

        [Fact]
        public void WithResultTest()
        {
            IRequestBuilder lRequestBuilder = this._client.CreateRequest().FromUrl(new Uri(BaseUrl));
            lRequestBuilder.WithLoginCheck(true);
            lRequestBuilder.WithGetParameter("test", "value");
            lRequestBuilder.WithPostParameter("test", "value");
            IRequestBuilderWithResult<int> lWithResult = lRequestBuilder.WithResult<int>();
            Assert.Equal(lRequestBuilder.CheckLogin, lWithResult.CheckLogin);
            Assert.Equal(lRequestBuilder.GetParameters, lWithResult.GetParameters);
            Assert.Equal(lRequestBuilder.PostArguments, lWithResult.PostArguments);
            Assert.Same(lRequestBuilder.Client, lWithResult.Client);
        }
    }
}