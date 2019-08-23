using System;
using System.Linq;
using Azuria.Authentication;
using Azuria.Middleware;
using Azuria.Middleware.Pipeline;
using Azuria.Test.Core.Helpers;
using Moq;
using NUnit.Framework;

namespace Azuria.Test
{
    [TestFixture]
    public class ProxerClientOptionsTest
    {
        [Test]
        public void Constructor_PopulatesPropertiesTest()
        {
            char[] apiKey = RandomHelper.GetRandomString(32).ToCharArray();
            IProxerClient client = ProxerClient.Create(apiKey);

            var options = new ProxerClientOptions(apiKey, client);
            
            Assert.AreSame(apiKey, options.ApiKey);
            Assert.AreSame(client, options.Client);
            Assert.NotNull(options.Pipeline);
        }

        [Test]
        public void Constructor_CreatesDefaultPipeline()
        {
            char[] apiKey = RandomHelper.GetRandomString(32).ToCharArray();
            IProxerClient client = ProxerClient.Create(apiKey);

            var options = new ProxerClientOptions(apiKey, client);

            IPipeline pipeline = options.Pipeline;
            Assert.NotNull(pipeline);
            Assert.IsInstanceOf<Pipeline>(pipeline);
            Assert.NotNull(pipeline.Middlewares);

            IMiddleware[] middlewares = pipeline.Middlewares.ToArray();
            Assert.IsNotEmpty(middlewares);
            Assert.AreEqual(3, middlewares.Length);
            Assert.IsInstanceOf<StaticHeaderMiddleware>(middlewares[0]);
            Assert.IsInstanceOf<ErrorMiddleware>(middlewares[1]);
            Assert.IsInstanceOf<HttpJsonRequestMiddleware>(middlewares[2]);

            //TODO: Check headers of StaticHeaderMiddleware
        }

        [Test]
        public void WithAuthentication_GivesTokenToLoginManagerTest()
        {
            char[] apiKey = RandomHelper.GetRandomString(32).ToCharArray();
            char[] loginToken = RandomHelper.GetRandomString(255).ToCharArray();
            IProxerClient client = ProxerClient.Create(apiKey);

            var options = new ProxerClientOptions(apiKey, client);

            Assert.AreSame(options, options.WithAuthentication(loginToken));

            LoginMiddleware loginMiddleware =
                options.Pipeline.Middlewares.FirstOrDefault(middleware =>
                    middleware.GetType() == typeof(LoginMiddleware)) as LoginMiddleware;
            Assert.NotNull(loginMiddleware);
            Assert.NotNull(loginMiddleware.LoginManager);
            Assert.IsInstanceOf<DefaultLoginManager>(loginMiddleware.LoginManager);

            var loginManager = (DefaultLoginManager) loginMiddleware.LoginManager;
            Assert.AreEqual(loginToken, loginManager.LoginToken);
        }

        [Test]
        public void WithAuthentication_InsertsMiddlewareAfterHeaderMiddlewareTest()
        {
            // This test covers the case that no login middleware is currently in the pipeline
            // before calling WithAuthentication
            
            char[] apiKey = RandomHelper.GetRandomString(32).ToCharArray();
            char[] loginToken = RandomHelper.GetRandomString(255).ToCharArray();
            IProxerClient client = ProxerClient.Create(apiKey);

            var options = new ProxerClientOptions(apiKey, client);

            Assert.AreSame(options, options.WithAuthentication(loginToken));
            
            IMiddleware[] middlewares = options.Pipeline.Middlewares.ToArray();
            Assert.IsNotEmpty(middlewares);
            Assert.AreEqual(4, middlewares.Length);
            Assert.IsInstanceOf<StaticHeaderMiddleware>(middlewares[0]);
            Assert.IsInstanceOf<LoginMiddleware>(middlewares[1]);
        }
        
        [Test]
        public void WithAuthentication_NoTokenEnablesDefaultLoginMiddlewareTest()
        {
            char[] apiKey = RandomHelper.GetRandomString(32).ToCharArray();
            IProxerClient client = ProxerClient.Create(apiKey);

            var options = new ProxerClientOptions(apiKey, client);

            Assert.AreSame(options, options.WithAuthentication());

            LoginMiddleware loginMiddleware =
                options.Pipeline.Middlewares.FirstOrDefault(middleware =>
                    middleware.GetType() == typeof(LoginMiddleware)) as LoginMiddleware;
            Assert.NotNull(loginMiddleware);
            Assert.NotNull(loginMiddleware.LoginManager);
            Assert.IsInstanceOf<DefaultLoginManager>(loginMiddleware.LoginManager);
        }

        [Test]
        public void WithAuthentication_ReplacesOldMiddlewareTest()
        {
            char[] apiKey = RandomHelper.GetRandomString(32).ToCharArray();
            char[] loginToken = RandomHelper.GetRandomString(255).ToCharArray();
            IProxerClient client = ProxerClient.Create(apiKey);

            var options = new ProxerClientOptions(apiKey, client);

            Assert.AreSame(options, options.WithAuthentication(loginToken));
            
            IMiddleware[] middlewares = options.Pipeline.Middlewares.ToArray();
            Assert.IsNotEmpty(middlewares);
            Assert.AreEqual(4, middlewares.Length);
            Assert.IsInstanceOf<LoginMiddleware>(middlewares[1]);

            var firstLoginMiddleware = middlewares[1];
            
            
            Assert.AreSame(options, options.WithAuthentication(loginToken));
            
            middlewares = options.Pipeline.Middlewares.ToArray();
            Assert.IsNotEmpty(middlewares);
            Assert.AreEqual(4, middlewares.Length);
            Assert.IsInstanceOf<LoginMiddleware>(middlewares[1]);
            Assert.AreNotSame(firstLoginMiddleware, middlewares[1]);
        }
        
        [Test]
        public void WithAuthentication_ThrowsIfInvalidTokenTest()
        {
            char[] apiKey = RandomHelper.GetRandomString(32).ToCharArray();
            IProxerClient client = ProxerClient.Create(apiKey);

            var options = new ProxerClientOptions(apiKey, client);

            var argumentException = Assert.Throws<ArgumentException>(() => options.WithAuthentication(new char[14]));
            Assert.AreEqual("loginToken", argumentException.ParamName);
            /*char[] lLoginKey = new char[255].Select(c => (char) new Random().Next(255)).ToArray();
            IProxerClient lClient = ProxerClient.Create(new char[0], options => options.WithAuthentication(lLoginKey));
            Assert.True(lClient.Container.IsRegistered<ILoginManager>());
            Assert.AreEqual(lLoginKey, lClient.Container.Resolve<ILoginManager>().LoginToken);*/
        }

        [Test]
        public void WithCustomHttpClientTest()
        {
            /*Assert.Throws<ArgumentNullException>(
                () => ProxerClient.Create(new char[0], options => options.WithCustomHttpClient(null)));

            IHttpClient lHttpClient = Mock.Of<IHttpClient>();
            IProxerClient lClient = ProxerClient.Create(
                new char[0], options => options.WithCustomHttpClient(context => lHttpClient));
            Assert.True(lClient.Container.IsRegistered<IHttpClient>());
            Assert.AreSame(lHttpClient, lClient.Container.Resolve<IHttpClient>());

            lClient = ProxerClient.Create(
                new char[0], options => options.WithCustomHttpClient(6000, "Azuria.Test"));
            Assert.True(lClient.Container.IsRegistered<IHttpClient>());*/

            Assert.Fail("Not implemented");
        }

        [Test]
        public void WithLoginManagerTest()
        {
            /*ILoginManager lLoginManager = Mock.Of<ILoginManager>();
            IProxerClient lClient = ProxerClient.Create(
                "apiKey".ToCharArray(),
                options => options.WithCustomLoginManager(context => lLoginManager));
            Assert.True(lClient.Container.IsRegistered<ILoginManager>());
            Assert.AreSame(lLoginManager, lClient.Container.Resolve<ILoginManager>());*/

            Assert.Fail("Not implemented");
        }
    }
}