using System;
using System.Threading;
using System.Threading.Tasks;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.Input.User;
using Azuria.Authentication;
using Azuria.ErrorHandling;
using Azuria.Middleware;
using Azuria.Middleware.Pipeline;
using Azuria.Requests.Builder;
using Azuria.Test.Core.Helpers;
using Moq;
using NUnit.Framework;

namespace Azuria.Test.Authentication
{
    [TestFixture]
    public class DefaultLoginManagerTest
    {
        private const string LoginTokenHeaderName = "proxer-api-token";

        [Test]
        public void AddAuthenticationInformation_AddsCorrectInfosTest()
        {
            var client = ProxerClient.Create(new char[32]);

            var loginToken = RandomHelper.GetRandomString(255).ToCharArray();
            DefaultLoginManager defaultLoginManager = new DefaultLoginManager(client, loginToken);

            var request = new RequestBuilder(new Uri("http://proxer.me/api"), client).WithLoginCheck();
            bool added = defaultLoginManager.AddAuthenticationInformation(request);
            Assert.True(added);
            Assert.True(request.Headers.ContainsKey(LoginTokenHeaderName));
            Assert.AreEqual(loginToken.ToString(), request.Headers[LoginTokenHeaderName]);
        }

        [Test]
        public void AddAuthenticationInformation_SkipsIfLoginNotNeededTest()
        {
            var client = ProxerClient.Create(new char[32]);

            var loginToken = RandomHelper.GetRandomString(255).ToCharArray();
            DefaultLoginManager defaultLoginManager = new DefaultLoginManager(client, loginToken);

            var request = new RequestBuilder(new Uri("http://proxer.me/api"), client).WithLoginCheck(false);
            bool added = defaultLoginManager.AddAuthenticationInformation(request);
            Assert.False(added);
            Assert.False(request.Headers.ContainsKey(LoginTokenHeaderName));
        }

        [Test]
        public void AddAuthenticationInformation_SkipsIfRequestContainsAuthInfosTest()
        {
            var client = ProxerClient.Create(new char[32]);

            var loginToken = RandomHelper.GetRandomString(255).ToCharArray();
            DefaultLoginManager defaultLoginManager = new DefaultLoginManager(client, loginToken);

            var request =
                new RequestBuilder(new Uri("http://proxer.me/api"), client)
                    .WithLoginCheck(false)
                    .WithHeader(LoginTokenHeaderName, "token");
            bool added = defaultLoginManager.AddAuthenticationInformation(request);
            Assert.False(added);
            Assert.True(request.Headers.ContainsKey(LoginTokenHeaderName));
            Assert.AreEqual("token", request.Headers[LoginTokenHeaderName]);
        }

        [Test]
        public void AddAuthenticationInformation_SkipsIfTokenInvalidTest()
        {
            var client = ProxerClient.Create(new char[32]);

            var loginToken = RandomHelper.GetRandomString(42).ToCharArray();
            DefaultLoginManager defaultLoginManager = new DefaultLoginManager(client, loginToken);

            var request = new RequestBuilder(new Uri("http://proxer.me/api"), client).WithLoginCheck();
            bool added = defaultLoginManager.AddAuthenticationInformation(request);
            Assert.False(added);
            Assert.False(request.Headers.ContainsKey(LoginTokenHeaderName));

            defaultLoginManager = new DefaultLoginManager(client);

            added = defaultLoginManager.AddAuthenticationInformation(request);
            Assert.False(added);
            Assert.False(request.Headers.ContainsKey(LoginTokenHeaderName));
        }

        [Test]
        public void ContainsAuthenticationInformationTest()
        {
            var client = ProxerClient.Create(new char[32]);
            var loginManager = new DefaultLoginManager(client, RandomHelper.GetRandomString(255).ToCharArray());
            var request = new RequestBuilder(new Uri("https://proxer.me/api"), client);

            Assert.False(loginManager.ContainsAuthenticationInformation(request));
            request.WithHeader("proxer-api-token", loginManager.LoginToken.ToString());
            Assert.True(loginManager.ContainsAuthenticationInformation(request));
        }

        [Test]
        public void InvalidateLoginTest()
        {
            var client = ProxerClient.Create(new char[32]);
            var loginManager = CreateLoggedInManager(new char[255], client);

            var request = new RequestBuilder(new Uri("https://proxer.me/api"), client).WithLoginCheck();
            Assert.False(loginManager.AddAuthenticationInformation(request));
            Assert.True(loginManager.IsLoginProbablyValid());
            loginManager.InvalidateLogin();
            Assert.True(loginManager.AddAuthenticationInformation(request));
            Assert.False(loginManager.IsLoginProbablyValid());
        }

        [Test]
        public void IsLoginProbablyValid_InvalidIfLoginNeverPerformedTest()
        {
            var client = ProxerClient.Create(new char[32]);
            var loginManager = new DefaultLoginManager(client, new char[255]);

            Assert.False(loginManager.IsLoginProbablyValid());
        }

        [Test]
        public void IsLoginProbablyValid_ValidIfJustLoggedInTest()
        {
            //TODO: The other cases are hard to test. Maybe search for a package on github that helps with DateTime manipulation?
            var client = ProxerClient.Create(new char[32]);
            var loginManager = CreateLoggedInManager(new char[255], client);

            Assert.True(loginManager.IsLoginProbablyValid());
        }

        [Test]
        public async Task PerformLoginTest()
        {
            // Mock of a middleware that returns a successful response
            var middlewareMock = new Mock<IMiddleware>();
            
            var successDataModel = new LoginDataModel
            {
                UserId = 1,
                Token = RandomHelper.GetRandomString(255)
            };
            middlewareMock
                .Setup(middleware => middleware.InvokeWithResult(It.IsAny<IRequestBuilderWithResult<LoginDataModel>>(),
                    It.IsAny<MiddlewareAction<LoginDataModel>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new ProxerResult<LoginDataModel>(successDataModel));

            CancellationToken cancellationToken = new CancellationTokenSource().Token;
            IProxerClient client = ProxerClient.Create(new char[32],
                options => options.Pipeline = new Pipeline(new[] {middlewareMock.Object}));
            DefaultLoginManager loginManager = new DefaultLoginManager(client);

            IProxerResult<LoginDataModel> result = await loginManager.PerformLoginAsync(
                new LoginInput("username", "password"), cancellationToken
            );
            Assert.True(result.Success);
            Assert.IsEmpty(result.Exceptions);

            Assert.AreEqual(successDataModel.Token, loginManager.LoginToken);
            Assert.True(loginManager.IsLoginProbablyValid());
        }

        [Test]
        public async Task PerformLoginWith2FaTokenTest()
        {
            // Mock of a middleware that returns a successful response
            var middlewareMock = new Mock<IMiddleware>();
            
            var successDataModel = new LoginDataModel
            {
                UserId = 1,
                Token = RandomHelper.GetRandomString(255)
            };
            middlewareMock
                .Setup(middleware => middleware.InvokeWithResult(It.IsAny<IRequestBuilderWithResult<LoginDataModel>>(),
                    It.IsAny<MiddlewareAction<LoginDataModel>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new ProxerResult<LoginDataModel>(successDataModel));
            
            // Begin tests
            CancellationToken cancellationToken = new CancellationTokenSource().Token;
            IProxerClient client = ProxerClient.Create(new char[32],
                options => options.Pipeline = new Pipeline(new[] {middlewareMock.Object}));
            DefaultLoginManager loginManager = new DefaultLoginManager(client);

            IProxerResult<LoginDataModel> result = await loginManager.PerformLoginAsync(
                new LoginInput("username", "password", new string(new char[6])),
                cancellationToken
            );
            Assert.True(result.Success);
            Assert.IsEmpty(result.Exceptions);

            Assert.AreEqual(successDataModel.Token, loginManager.LoginToken);
            Assert.True(loginManager.IsLoginProbablyValid());
        }

        [Test]
        public async Task PerformLogoutTest()
        {
            // Mock of a middleware that returns a successful response
            var middlewareMock = new Mock<IMiddleware>();
            
            var successDataModel = new LoginDataModel
            {
                UserId = 1,
                Token = RandomHelper.GetRandomString(255)
            };
            middlewareMock
                .Setup(middleware => middleware.Invoke(It.IsAny<IRequestBuilder>(),
                    It.IsAny<MiddlewareAction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new ProxerResult());

            // Begin tests
            CancellationToken cancellationToken = new CancellationTokenSource().Token;
            IProxerClient client = ProxerClient.Create(new char[32],
                options => options.Pipeline = new Pipeline(new[] {middlewareMock.Object}));
            DefaultLoginManager loginManager = new DefaultLoginManager(client);

            IProxerResult result = await loginManager.PerformLogoutAsync(cancellationToken);
            Assert.True(result.Success);
            Assert.IsEmpty(result.Exceptions);

            Assert.Null(loginManager.LoginToken);
            Assert.False(loginManager.IsLoginProbablyValid());
        }

        [Test]
        public void UpdateTest()
        {
            var client = ProxerClient.Create(new char[32]);
            var loginManager = new DefaultLoginManager(client, new char[255]);
            Assert.False(loginManager.IsLoginProbablyValid());

            var request = new RequestBuilder(new Uri("http://proxer.me/api"), client);

            // request does not contain any login information and the login manager was not logged in
            // => the login manager is still not logged in
            loginManager.Update(request, new ProxerResult());
            Assert.False(loginManager.IsLoginProbablyValid());

            bool added = loginManager.AddAuthenticationInformation(request.WithLoginCheck());
            Assert.True(added);

            loginManager.Update(request, new ProxerResult());
            Assert.True(loginManager.IsLoginProbablyValid());
        }

        private static DefaultLoginManager CreateLoggedInManager(char[] loginToken, IProxerClient client)
        {
            var loginManager = new DefaultLoginManager(client, loginToken);
            var request = new RequestBuilder(new Uri("http://proxer.me/api"), client).WithLoginCheck();
            loginManager.AddAuthenticationInformation(request);
            loginManager.Update(request, new ProxerResult());

            return loginManager;
        }
        
        // Returns the first instance from the pipeline or null
        private static DefaultLoginManager TryFindLoginManager(IPipeline pipeline)
        {
            foreach (var middleware in pipeline.Middlewares)
            {
                if (middleware is LoginMiddleware loginMiddleware)
                {
                    if (loginMiddleware.LoginManager is DefaultLoginManager instance)
                    {
                        return instance;
                    }
                }
            }
            
            return null;
        }
    }
}