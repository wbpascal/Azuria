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
            Assert.NotNull(lClient.ClientOptions);
            Assert.IsNotEmpty(lClient.ClientOptions.ApiKey);
            Assert.NotNull(lClient.ClientOptions.LoginManager);
            Assert.NotNull(lClient.ClientOptions.Pipeline);
        }
    }
}