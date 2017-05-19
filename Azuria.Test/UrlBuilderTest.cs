using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Api.Builder;
using Azuria.Requests;
using Xunit;

namespace Azuria.Test
{
    public class UrlBuilderTest
    {
        private const string BaseUrl = "https://proxer.me/";
        private readonly IUrlBuilder _urlBuilder;

        public UrlBuilderTest()
        {
            this._urlBuilder = ProxerClient.Create(new char[32]).CreateRequest().FromUrl(new Uri(BaseUrl));
        }

        [Fact]
        public void WithGetParameterTest()
        {
            this._urlBuilder.WithGetParameter("test", "value1");
            Assert.Equal(1, this._urlBuilder.GetParameters.Count);
            Assert.True(this._urlBuilder.GetParameters.ContainsKey("test"));
            Assert.Equal("value1", this._urlBuilder.GetParameters["test"]);

            this._urlBuilder.WithGetParameter("test", "value2");
            Assert.Equal(1, this._urlBuilder.GetParameters.Count);
            Assert.True(this._urlBuilder.GetParameters.ContainsKey("test"));
            Assert.Equal("value2", this._urlBuilder.GetParameters["test"]);

            this._urlBuilder.WithGetParameter(
                new Dictionary<string, string>
                {
                    {"test2", "value3"},
                    {"testNew", "value"}
                });
            Assert.Equal(3, this._urlBuilder.GetParameters.Count);
            Assert.True(this._urlBuilder.GetParameters.ContainsKey("test"));
            Assert.True(this._urlBuilder.GetParameters.ContainsKey("test2"));
            Assert.True(this._urlBuilder.GetParameters.ContainsKey("testNew"));
            Assert.Equal("value2", this._urlBuilder.GetParameters["test"]);
            Assert.Equal("value3", this._urlBuilder.GetParameters["test2"]);
            Assert.Equal("value", this._urlBuilder.GetParameters["testNew"]);
        }

        [Fact]
        public void WithPostParameterTest()
        {
            this._urlBuilder.WithPostParameter("test", "value1");
            Assert.Equal(1, this._urlBuilder.PostArguments.Count());
            Assert.True(this._urlBuilder.PostArguments.Any(pair => pair.Key == "test" && pair.Value == "value1"));

            this._urlBuilder.WithPostParameter("test", "value2");
            Assert.Equal(2, this._urlBuilder.PostArguments.Count());
            Assert.True(this._urlBuilder.PostArguments.Any(pair => pair.Key == "test" && pair.Value == "value1"));
            Assert.True(this._urlBuilder.PostArguments.Any(pair => pair.Key == "test" && pair.Value == "value2"));

            this._urlBuilder.WithPostParameter(
                new Dictionary<string, string>
                {
                    {"test2", "value3"},
                    {"testNew", "value"}
                });
            Assert.Equal(4, this._urlBuilder.PostArguments.Count());
            Assert.True(this._urlBuilder.PostArguments.Any(pair => pair.Key == "test" && pair.Value == "value1"));
            Assert.True(this._urlBuilder.PostArguments.Any(pair => pair.Key == "test" && pair.Value == "value2"));
            Assert.True(this._urlBuilder.PostArguments.Any(pair => pair.Key == "test2" && pair.Value == "value3"));
            Assert.True(this._urlBuilder.PostArguments.Any(pair => pair.Key == "testNew" && pair.Value == "value"));
        }
    }
}