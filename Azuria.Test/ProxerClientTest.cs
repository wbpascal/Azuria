using Autofac;
using Azuria.Api.Builder;
using Azuria.Authentication;
using Azuria.ErrorHandling;
using Azuria.Requests;
using Azuria.Requests.Http;
using Azuria.Serialization;
using NUnit.Framework;

namespace Azuria.Test
{
    [TestFixture]
    public class ProxerClientTest
    {
        [Test]
        public void CreateNoOptionsTest()
        {
            IProxerClient lClient = ProxerClient.Create(new char[32]);
            Assert.IsNotEmpty(lClient.ApiKey);
            Assert.True(lClient.Container.IsRegistered<IHttpClient>());
            Assert.True(lClient.Container.IsRegistered<IProxerClient>());
            Assert.True(lClient.Container.IsRegistered<ILoginManager>());
            Assert.True(lClient.Container.IsRegistered<IRequestHandler>());
            Assert.True(lClient.Container.IsRegistered<IApiRequestBuilder>());
            Assert.True(lClient.Container.IsRegistered<IRequestErrorHandler>());
            Assert.True(lClient.Container.IsRegistered<IJsonDeserializer>());
            Assert.True(lClient.Container.IsRegistered<IRequestHeaderManager>());
        }
    }
}