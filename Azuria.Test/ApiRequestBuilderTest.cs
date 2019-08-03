using System;
using System.Linq;
using Azuria.Api.Builder;
using Azuria.Api.v1;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Requests;
using Azuria.Requests.Builder;
using NUnit.Framework;

namespace Azuria.Test
{
    [TestFixture]
    public class ApiRequestBuilderTest
    {
        private readonly IApiRequestBuilder _apiRequestBuilder;

        public ApiRequestBuilderTest()
        {
            IProxerClient client = ProxerClient.Create(new char[32]);
            this._apiRequestBuilder = client.CreateRequest();
        }

        [Test]
        public void FromAnimeClassTest()
        {
            AnimeRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromAnimeClass();
            Assert.NotNull(lRequestBuilder);
        }

        [Test]
        public void FromInfoClassTest()
        {
            InfoRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromInfoClass();
            Assert.NotNull(lRequestBuilder);
        }

        [Test]
        public void FromListClassTest()
        {
            ListRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromListClass();
            Assert.NotNull(lRequestBuilder);
        }

        [Test]
        public void FromMangaClassTest()
        {
            MangaRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromMangaClass();
            Assert.NotNull(lRequestBuilder);
        }

        [Test]
        public void FromMediaClassTest()
        {
            MediaRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromMediaClass();
            Assert.NotNull(lRequestBuilder);
        }

        [Test]
        public void FromMessengerClassTest()
        {
            MessengerRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromMessengerClass();
            Assert.NotNull(lRequestBuilder);
        }

        [Test]
        public void FromNotificationClassTest()
        {
            NotificationsRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromNotificationClass();
            Assert.NotNull(lRequestBuilder);
        }

        [Test]
        public void FromUcpClassTest()
        {
            UcpRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromUcpClass();
            Assert.NotNull(lRequestBuilder);
        }

        [Test]
        public void FromUrlTest()
        {
            IRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromUrl(new Uri("https://google.com/"))
                .WithGetParameter("test", "value")
                .WithPostParameter("testPost", "postValue");

            Assert.AreEqual(1, lRequestBuilder.GetParameters.Count);
            Assert.True(lRequestBuilder.GetParameters.ContainsKey("test"));
            Assert.AreEqual("value", lRequestBuilder.GetParameters["test"]);
            Assert.AreEqual(1, lRequestBuilder.PostParameter.Count());
            Assert.True(lRequestBuilder.PostParameter.Any(pair => pair.Key == "testPost" && pair.Value == "postValue"));

            Assert.AreEqual("https://google.com/?test=value", lRequestBuilder.BuildUri().AbsoluteUri);
        }

        [Test]
        public void FromUserClassTest()
        {
            UserRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromUserClass();
            Assert.NotNull(lRequestBuilder);
        }
    }
}