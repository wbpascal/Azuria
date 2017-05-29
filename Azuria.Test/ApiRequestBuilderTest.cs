using System;
using System.Linq;
using Azuria.Api.Builder;
using Azuria.Api.v1;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Requests;
using Azuria.Requests.Builder;
using Azuria.Test.Core.Helpers;
using Xunit;

namespace Azuria.Test
{
    public class ApiRequestBuilderTest
    {
        private readonly IApiRequestBuilder _apiRequestBuilder;

        public ApiRequestBuilderTest()
        {
            IProxerClient client = ProxerClient.Create(new char[32], options => options.WithTestingHttpClient());
            this._apiRequestBuilder = client.CreateRequest();
        }

        [Fact]
        public void FromUrlTest()
        {
            IRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromUrl(new Uri("https://google.com/"))
                .WithGetParameter("test", "value")
                .WithPostParameter("testPost", "postValue");

            Assert.Equal(1, lRequestBuilder.GetParameters.Count);
            Assert.True(lRequestBuilder.GetParameters.ContainsKey("test"));
            Assert.Equal("value", lRequestBuilder.GetParameters["test"]);
            Assert.Equal(1, lRequestBuilder.PostArguments.Count());
            Assert.True(lRequestBuilder.PostArguments.Any(pair => pair.Key == "testPost" && pair.Value == "postValue"));

            Assert.Equal("https://google.com/?test=value", lRequestBuilder.BuildUri().AbsoluteUri);
        }

        [Fact]
        public void FromAnimeClassTest()
        {
            AnimeRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromAnimeClass();
            Assert.NotNull(lRequestBuilder);
        }

        [Fact]
        public void FromInfoClassTest()
        {
            InfoRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromInfoClass();
            Assert.NotNull(lRequestBuilder);
        }

        [Fact]
        public void FromListClassTest()
        {
            ListRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromListClass();
            Assert.NotNull(lRequestBuilder);
        }

        [Fact]
        public void FromMangaClassTest()
        {
            MangaRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromMangaClass();
            Assert.NotNull(lRequestBuilder);
        }

        [Fact]
        public void FromMediaClassTest()
        {
            MediaRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromMediaClass();
            Assert.NotNull(lRequestBuilder);
        }

        [Fact]
        public void FromMessengerClassTest()
        {
            MessengerRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromMessengerClass();
            Assert.NotNull(lRequestBuilder);
        }

        [Fact]
        public void FromNotificationClassTest()
        {
            NotificationsRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromNotificationClass();
            Assert.NotNull(lRequestBuilder);
        }

        [Fact]
        public void FromUcpClassTest()
        {
            UcpRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromUcpClass();
            Assert.NotNull(lRequestBuilder);
        }

        [Fact]
        public void FromUserClassTest()
        {
            UserRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromUserClass();
            Assert.NotNull(lRequestBuilder);
        }
    }
}