using System;
using Azuria.Api.Builder;
using Azuria.Requests.Builder;
using NUnit.Framework;

namespace Azuria.Test.Api.Builder
{
    [TestFixture]
    public class ApiRequestBuilderTest
    {
        private readonly IApiRequestBuilder _apiRequestBuilder;
        private readonly IProxerClient _client;

        public ApiRequestBuilderTest()
        {
            this._client = ProxerClient.Create(new char[32]);
            this._apiRequestBuilder = new ApiRequestBuilder(this._client);
        }

        [Test]
        public void FromUrlTest()
        {
            IRequestBuilder lRequestBuilder = this._apiRequestBuilder.FromUrl(new Uri("https://proxer.me"));
            Assert.IsEmpty(lRequestBuilder.GetParameters);
            Assert.IsEmpty(lRequestBuilder.PostParameter);
            Assert.False(lRequestBuilder.CheckLogin);
            Assert.AreSame(this._client, lRequestBuilder.Client);
            Assert.AreEqual(new Uri("https://proxer.me"), lRequestBuilder.BuildUri());
        }

        [Test]
        public void ProxerClientTest()
        {
            Assert.AreSame(this._client, this._apiRequestBuilder.ProxerClient);
        }
    }
}