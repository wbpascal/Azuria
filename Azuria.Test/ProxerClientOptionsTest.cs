using System;
using System.Linq;
using Azuria.Authentication;
using Azuria.Middleware;
using Azuria.Requests.Http;
using Azuria.Serialization;
using Azuria.Test.Core.Helpers;
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
            //TODO: Check http client and json deserializer (!= null)
        }

        [Test]
        public void WithAuthentication_GivesTokenToLoginManagerTest()
        {
            char[] apiKey = RandomHelper.GetRandomString(32).ToCharArray();
            char[] loginToken = RandomHelper.GetRandomString(255).ToCharArray();
            IProxerClient client = ProxerClient.Create(apiKey);

            var options = new ProxerClientOptions(apiKey, client);

            Assert.AreSame(options, options.WithAuthentication(loginToken));

            var loginMiddleware =
                options.Pipeline.Middlewares.FirstOrDefault(middleware =>
                    middleware.GetType() == typeof(LoginMiddleware)) as LoginMiddleware;
            Assert.NotNull(loginMiddleware);
            Assert.NotNull(loginMiddleware.LoginManager);
            Assert.IsInstanceOf<DefaultLoginManager>(loginMiddleware.LoginManager);

            var loginManager = (DefaultLoginManager) loginMiddleware.LoginManager;
            Assert.AreEqual(loginToken, loginManager.LoginToken);
        }

        [Test]
        public void WithAuthentication_InsertsNewInstanceAfterHeaderMiddlewareTest()
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

            var loginMiddleware =
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

            IMiddleware firstLoginMiddleware = middlewares[1];

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
        }

        [Test]
        public void WithExtraUserAgent_ReplacesOldMiddlewareTest()
        {
            char[] apiKey = RandomHelper.GetRandomString(32).ToCharArray();
            IProxerClient client = ProxerClient.Create(apiKey);

            var options = new ProxerClientOptions(apiKey, client);
            string userAgentExtra = RandomHelper.GetRandomString(10);

            var oldHeaderMiddleware = options.Pipeline.Middlewares.First() as StaticHeaderMiddleware;

            Assert.AreSame(options, options.WithExtraUserAgent(userAgentExtra));

            IMiddleware[] middlewares = options.Pipeline.Middlewares.ToArray();
            Assert.IsNotEmpty(middlewares);
            Assert.IsInstanceOf<StaticHeaderMiddleware>(middlewares[0]);

            var newHeaderMiddleware = middlewares[0] as StaticHeaderMiddleware;
            Assert.AreNotSame(oldHeaderMiddleware, newHeaderMiddleware);
            Assert.True(newHeaderMiddleware.Header["User-Agent"].Contains(userAgentExtra.TrimEnd()));
        }

        [Test]
        public void WithCustomHttpClient_CreatesNewInstanceWithTimeoutTest()
        {
            var timeout = 2500;
            char[] apiKey = RandomHelper.GetRandomString(32).ToCharArray();
            IProxerClient client = ProxerClient.Create(apiKey);

            var options = new ProxerClientOptions(apiKey, client);

            Assert.AreSame(options, options.WithCustomHttpClient(timeout));

            var httpMiddleware =
                options.Pipeline.Middlewares.FirstOrDefault(middleware =>
                    middleware.GetType() == typeof(HttpJsonRequestMiddleware)) as HttpJsonRequestMiddleware;
            Assert.NotNull(httpMiddleware);
            Assert.NotNull(httpMiddleware.HttpClient);
            Assert.NotNull(httpMiddleware.JsonDeserializer);

            Assert.IsInstanceOf<HttpClient>(httpMiddleware.HttpClient);
            // This is used because the only way to check what timeout was actually set (the idea of this function), is to check 
            //  it in the underlying system http client. Because we don't want to actually expose this client to the user, we need
            //  to use reflection to get the value of the field
            var systemClient =
                typeof(HttpClient).GetPrivateFieldValueOrDefault<System.Net.Http.HttpClient>("_client",
                    httpMiddleware.HttpClient);
            Assert.AreEqual(timeout, systemClient.Timeout.TotalMilliseconds);
        }

        [Test]
        public void WithCustomHttpClient_ReplacesOldInstanceTest()
        {
            char[] apiKey = RandomHelper.GetRandomString(32).ToCharArray();
            IProxerClient client = ProxerClient.Create(apiKey);

            var options = new ProxerClientOptions(apiKey, client);
            var httpClient = new HttpClient();

            var oldHttpMiddleware =
                options.Pipeline.Middlewares.FirstOrDefault(middleware =>
                    middleware.GetType() == typeof(HttpJsonRequestMiddleware)) as HttpJsonRequestMiddleware;
            Assert.NotNull(oldHttpMiddleware);

            Assert.AreSame(options, options.WithCustomHttpClient(httpClient));

            var httpMiddleware =
                options.Pipeline.Middlewares.FirstOrDefault(middleware =>
                    middleware.GetType() == typeof(HttpJsonRequestMiddleware)) as HttpJsonRequestMiddleware;
            Assert.NotNull(httpMiddleware);
            Assert.AreNotSame(oldHttpMiddleware, httpMiddleware);
            Assert.AreSame(httpClient, httpMiddleware.HttpClient);
            Assert.NotNull(httpMiddleware.JsonDeserializer);
            Assert.AreSame(oldHttpMiddleware.JsonDeserializer, httpMiddleware.JsonDeserializer);
        }

        [Test]
        public void WithCustomHttpClient_ThrowsIfClientNullTest()
        {
            char[] apiKey = RandomHelper.GetRandomString(32).ToCharArray();
            IProxerClient client = ProxerClient.Create(apiKey);

            var options = new ProxerClientOptions(apiKey, client);

            var argumentException = Assert.Throws<ArgumentNullException>(() => options.WithCustomHttpClient(null));
            Assert.AreEqual("client", argumentException.ParamName);
        }

        [Test]
        public void WithCustomJsonDeserializer_ReplacesOldInstanceTest()
        {
            char[] apiKey = RandomHelper.GetRandomString(32).ToCharArray();
            IProxerClient client = ProxerClient.Create(apiKey);

            var options = new ProxerClientOptions(apiKey, client);
            var deserializer = new JsonDeserializer();

            var oldHttpMiddleware =
                options.Pipeline.Middlewares.FirstOrDefault(middleware =>
                    middleware.GetType() == typeof(HttpJsonRequestMiddleware)) as HttpJsonRequestMiddleware;
            Assert.NotNull(oldHttpMiddleware);

            Assert.AreSame(options, options.WithCustomJsonDeserializer(deserializer));

            var httpMiddleware =
                options.Pipeline.Middlewares.FirstOrDefault(middleware =>
                    middleware.GetType() == typeof(HttpJsonRequestMiddleware)) as HttpJsonRequestMiddleware;
            Assert.NotNull(httpMiddleware);
            Assert.AreNotSame(oldHttpMiddleware, httpMiddleware);
            Assert.AreSame(deserializer, httpMiddleware.JsonDeserializer);
            Assert.NotNull(httpMiddleware.HttpClient);
            Assert.AreSame(oldHttpMiddleware.HttpClient, httpMiddleware.HttpClient);
        }

        [Test]
        public void WithCustomJsonDeserializer_ThrowsIfDeserializerNullTest()
        {
            char[] apiKey = RandomHelper.GetRandomString(32).ToCharArray();
            IProxerClient client = ProxerClient.Create(apiKey);

            var options = new ProxerClientOptions(apiKey, client);

            var argumentException =
                Assert.Throws<ArgumentNullException>(() => options.WithCustomJsonDeserializer(null));
            Assert.AreEqual("deserializer", argumentException.ParamName);
        }

        [Test]
        public void WithCustomLoginManagerTest_InsertsNewInstanceAfterHeaderMiddlewareTest()
        {
            // This test covers the case that no login middleware is currently in the pipeline
            // before calling WithAuthentication

            char[] apiKey = RandomHelper.GetRandomString(32).ToCharArray();
            char[] loginToken = RandomHelper.GetRandomString(255).ToCharArray();
            IProxerClient client = ProxerClient.Create(apiKey);

            var options = new ProxerClientOptions(apiKey, client);
            var loginManager = new DefaultLoginManager(loginToken);

            Assert.AreSame(options, options.WithCustomLoginManager(loginManager));

            IMiddleware[] middlewares = options.Pipeline.Middlewares.ToArray();
            Assert.IsNotEmpty(middlewares);
            Assert.AreEqual(4, middlewares.Length);
            Assert.IsInstanceOf<StaticHeaderMiddleware>(middlewares[0]);
            Assert.IsInstanceOf<LoginMiddleware>(middlewares[1]);
            Assert.AreSame(loginManager, (middlewares[1] as LoginMiddleware)?.LoginManager);
        }

        [Test]
        public void WithCustomLoginManagerTest_ReplacesOldMiddlewareTest()
        {
            char[] apiKey = RandomHelper.GetRandomString(32).ToCharArray();
            char[] loginToken = RandomHelper.GetRandomString(255).ToCharArray();
            IProxerClient client = ProxerClient.Create(apiKey);

            var options = new ProxerClientOptions(apiKey, client);
            var loginManager = new DefaultLoginManager(loginToken);

            Assert.AreSame(options, options.WithCustomLoginManager(loginManager));

            IMiddleware[] middlewares = options.Pipeline.Middlewares.ToArray();
            Assert.IsNotEmpty(middlewares);
            Assert.AreEqual(4, middlewares.Length);
            Assert.IsInstanceOf<LoginMiddleware>(middlewares[1]);

            IMiddleware firstLoginMiddleware = middlewares[1];

            Assert.AreSame(options, options.WithCustomLoginManager(loginManager));

            middlewares = options.Pipeline.Middlewares.ToArray();
            Assert.IsNotEmpty(middlewares);
            Assert.AreEqual(4, middlewares.Length);
            Assert.IsInstanceOf<LoginMiddleware>(middlewares[1]);
            Assert.AreNotSame(firstLoginMiddleware, middlewares[1]);
        }

        [Test]
        public void WithCustomLoginManagerTest_ThrowsIfParamNullTest()
        {
            char[] apiKey = RandomHelper.GetRandomString(32).ToCharArray();
            IProxerClient client = ProxerClient.Create(apiKey);

            var options = new ProxerClientOptions(apiKey, client);

            var argumentException = Assert.Throws<ArgumentNullException>(() => options.WithCustomLoginManager(null));
            Assert.AreEqual("loginManager", argumentException.ParamName);
        }
    }
}