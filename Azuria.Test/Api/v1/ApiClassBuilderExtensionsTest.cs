using Azuria.Api.Builder;
using Azuria.Api.v1;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Requests;
using NUnit.Framework;

namespace Azuria.Test.Api.v1
{
    [TestFixture]
    public class ApiClassBuilderExtensionsTest
    {
        private readonly IApiRequestBuilder _apiRequestBuilder;

        public ApiClassBuilderExtensionsTest()
        {
            this._apiRequestBuilder = ProxerClient.Create(new char[32]).CreateRequest();
        }

        [Test]
        public void FromAnimeClassTest()
        {
            AnimeRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromAnimeClass();
            Assert.IsAssignableFrom<AnimeRequestBuilder>(lRequestBuilder);
        }

        [Test]
        public void FromInfoClassTest()
        {
            InfoRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromInfoClass();
            Assert.IsAssignableFrom<InfoRequestBuilder>(lRequestBuilder);
        }

        [Test]
        public void FromListClassTest()
        {
            ListRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromListClass();
            Assert.IsAssignableFrom<ListRequestBuilder>(lRequestBuilder);
        }

        [Test]
        public void FromMangaClassTest()
        {
            MangaRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromMangaClass();
            Assert.IsAssignableFrom<MangaRequestBuilder>(lRequestBuilder);
        }

        [Test]
        public void FromMediaClassTest()
        {
            MediaRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromMediaClass();
            Assert.IsAssignableFrom<MediaRequestBuilder>(lRequestBuilder);
        }

        [Test]
        public void FromMessengerClassTest()
        {
            MessengerRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromMessengerClass();
            Assert.IsAssignableFrom<MessengerRequestBuilder>(lRequestBuilder);
        }

        [Test]
        public void FromNotificationClassTest()
        {
            NotificationsRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromNotificationClass();
            Assert.IsAssignableFrom<NotificationsRequestBuilder>(lRequestBuilder);
        }

        [Test]
        public void FromUcpClassTest()
        {
            UcpRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromUcpClass();
            Assert.IsAssignableFrom<UcpRequestBuilder>(lRequestBuilder);
        }

        [Test]
        public void FromUserClassTest()
        {
            UserRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromUserClass();
            Assert.IsAssignableFrom<UserRequestBuilder>(lRequestBuilder);
        }
    }
}