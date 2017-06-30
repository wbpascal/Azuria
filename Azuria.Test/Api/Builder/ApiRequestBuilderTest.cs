using System;
using Autofac;
using Azuria.Api.Builder;
using Azuria.Requests.Builder;
using Xunit;

namespace Azuria.Test.Api.Builder
{
    public class ApiRequestBuilderTest
    {
        private readonly IApiRequestBuilder _apiRequestBuilder;
        private readonly IProxerClient _client;

        public ApiRequestBuilderTest()
        {
            this._client = ProxerClient.Create(new char[32]);
            this._apiRequestBuilder = this._client.Container.Resolve<IApiRequestBuilder>();
        }

        [Fact]
        public void FromUrlTest()
        {
            IRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromUrl(new Uri("https://proxer.me"));
            Assert.Empty(lRequestBuilder.GetParameters);
            Assert.Empty(lRequestBuilder.PostParameter);
            Assert.False(lRequestBuilder.CheckLogin);
            Assert.Same(this._client, lRequestBuilder.Client);
            Assert.Equal(new Uri("https://proxer.me"), lRequestBuilder.BuildUri());
        }

        [Fact]
        public void ProxerClientTest()
        {
            Assert.Same(this._client, this._apiRequestBuilder.ProxerClient);
        }
    }
}