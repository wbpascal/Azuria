using System.Threading;
using Azuria.Api.Builder;
using Azuria.Api.v1;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Requests;
using Xunit;

namespace Azuria.Test.Api.v1
{
    public class ApiClassBuilderExtensions
    {
        private readonly IApiRequestBuilder _apiRequestBuilder;

        public ApiClassBuilderExtensions()
        {
            this._apiRequestBuilder = ProxerClient.Create(new char[32]).CreateRequest();
        }

        [Fact]
        public void FromAnimeClassTest()
        {
            AnimeRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromAnimeClass();
            Assert.IsType<AnimeRequestBuilder>(lRequestBuilder);
        }

        [Fact]
        public void FromInfoClassTest()
        {
            InfoRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromInfoClass();
            Assert.IsType<InfoRequestBuilder>(lRequestBuilder);
        }

        [Fact]
        public void FromListClassTest()
        {
            ListRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromListClass();
            Assert.IsType<ListRequestBuilder>(lRequestBuilder);
        }

        [Fact]
        public void FromMangaClassTest()
        {
            MangaRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromMangaClass();
            Assert.IsType<MangaRequestBuilder>(lRequestBuilder);
        }

        [Fact]
        public void FromMediaClassTest()
        {
            MediaRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromMediaClass();
            Assert.IsType<MediaRequestBuilder>(lRequestBuilder);
        }

        [Fact]
        public void FromMessengerClassTest()
        {
            MessengerRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromMessengerClass();
            Assert.IsType<MessengerRequestBuilder>(lRequestBuilder);
        }

        [Fact]
        public void FromNotificationClassTest()
        {
            NotificationsRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromNotificationClass();
            Assert.IsType<NotificationsRequestBuilder>(lRequestBuilder);
        }

        [Fact]
        public void FromUcpClassTest()
        {
            UcpRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromUcpClass();
            Assert.IsType<UcpRequestBuilder>(lRequestBuilder);
        }

        [Fact]
        public void FromUserClassTest()
        {
            UserRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromUserClass();
            Assert.IsType<UserRequestBuilder>(lRequestBuilder);
        }
    }
}