using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Azuria.Api.Builder;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.User;
using Azuria.Authentication;
using Azuria.Enums;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Requests;
using Azuria.Requests.Builder;
using Azuria.Requests.Http;
using Azuria.Serialization;
using Azuria.Test.Core.Helpers;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace Azuria.Test
{
    public class ProxerClientTest
    {
        [Fact]
        public void CreateNoOptionsTest()
        {
            IProxerClient lClient = ProxerClient.Create(new char[32]);
            Assert.NotEmpty(lClient.ApiKey);
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