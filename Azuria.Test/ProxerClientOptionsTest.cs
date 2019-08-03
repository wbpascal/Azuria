using System;
using System.Linq;
using Autofac;
using Azuria.Authentication;
using Azuria.Requests.Http;
using Moq;
using NUnit.Framework;

namespace Azuria.Test
{
    [TestFixture]
    public class ProxerClientOptionsTest
    {
        [Test]
        public void AuthorisationTest()
        {
            Assert.Throws<ArgumentException>(
                () => ProxerClient.Create(new char[0], options => options.WithAuthorisation(new char[0])));
            char[] lLoginKey = new char[255].Select(c => (char) new Random().Next(255)).ToArray();
            IProxerClient lClient = ProxerClient.Create(new char[0], options => options.WithAuthorisation(lLoginKey));
            Assert.True(lClient.Container.IsRegistered<ILoginManager>());
            Assert.AreEqual(lLoginKey, lClient.Container.Resolve<ILoginManager>().LoginToken);
        }

        [Test]
        public void CustomHttpClientTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => ProxerClient.Create(new char[0], options => options.WithCustomHttpClient(null)));

            IHttpClient lHttpClient = Mock.Of<IHttpClient>();
            IProxerClient lClient = ProxerClient.Create(
                new char[0], options => options.WithCustomHttpClient(context => lHttpClient));
            Assert.True(lClient.Container.IsRegistered<IHttpClient>());
            Assert.AreSame(lHttpClient, lClient.Container.Resolve<IHttpClient>());

            lClient = ProxerClient.Create(
                new char[0], options => options.WithCustomHttpClient(6000, "Azuria.Test"));
            Assert.True(lClient.Container.IsRegistered<IHttpClient>());
        }

        [Test]
        public void LoginManagerTest()
        {
            ILoginManager lLoginManager = Mock.Of<ILoginManager>();
            IProxerClient lClient = ProxerClient.Create(
                "apiKey".ToCharArray(),
                options => options.WithCustomLoginManager(context => lLoginManager));
            Assert.True(lClient.Container.IsRegistered<ILoginManager>());
            Assert.AreSame(lLoginManager, lClient.Container.Resolve<ILoginManager>());
        }
    }
}