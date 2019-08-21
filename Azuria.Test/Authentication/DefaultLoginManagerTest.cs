using System;
using System.Threading.Tasks;
using Azuria.Authentication;
using Azuria.ErrorHandling;
using Azuria.Requests.Builder;
using Azuria.Test.Core.Helpers;
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
            DefaultLoginManager defaultLoginManager = new DefaultLoginManager(loginToken);

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
            DefaultLoginManager defaultLoginManager = new DefaultLoginManager(loginToken);

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
            DefaultLoginManager defaultLoginManager = new DefaultLoginManager(loginToken);

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
            DefaultLoginManager defaultLoginManager = new DefaultLoginManager(loginToken);

            var request = new RequestBuilder(new Uri("http://proxer.me/api"), client).WithLoginCheck();
            bool added = defaultLoginManager.AddAuthenticationInformation(request);
            Assert.False(added);
            Assert.False(request.Headers.ContainsKey(LoginTokenHeaderName));

            defaultLoginManager = new DefaultLoginManager();

            added = defaultLoginManager.AddAuthenticationInformation(request);
            Assert.False(added);
            Assert.False(request.Headers.ContainsKey(LoginTokenHeaderName));
        }

        [Test]
        public void ContainsAuthenticationInformationTest()
        {
            var client = ProxerClient.Create(new char[32]);
            var loginManager = new DefaultLoginManager(RandomHelper.GetRandomString(255).ToCharArray());
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
            var loginManager = new DefaultLoginManager(new char[255]);

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
            /*CancellationToken lCancellationToken = new CancellationTokenSource().Token;

            IRequestHandler lRequestHandler = Mock.Of<IRequestHandler>();
            LoginDataModel lSuccessDataModel = new LoginDataModel
            {
                UserId = 1,
                Token = RandomHelper.GetRandomString(255)
            };
            Mock.Get(lRequestHandler).Setup(
                    handler => handler.MakeRequestAsync(
                        It.IsAny<IRequestBuilderWithResult<LoginDataModel>>(), lCancellationToken
                    )
                )
                .ReturnsAsync(() => new ProxerResult<LoginDataModel>(lSuccessDataModel));

            IProxerClient lClient = ProxerClient.Create(
                new char[32], options => { options.ContainerBuilder.RegisterInstance(lRequestHandler); });
            ILoginManager lLoginManager = lClient.Container.Resolve<ILoginManager>();

            IProxerResult lResult = await lLoginManager.PerformLoginAsync(
                new LoginInput("username", "password"), token: lCancellationToken
            );
            Assert.True(lResult.Success);
            Assert.IsEmpty(lResult.Exceptions);

            Assert.AreEqual(lSuccessDataModel.Token, lLoginManager.LoginToken);
            Assert.True(lLoginManager.CheckIsLoginProbablyValid());*/
            Assert.Fail("Authentication not implemented");
        }

        [Test]
        public async Task PerformLoginWith2FaTokenTest()
        {
            /*CancellationToken lCancellationToken = new CancellationTokenSource().Token;

            IRequestHandler lRequestHandler = Mock.Of<IRequestHandler>();
            LoginDataModel lSuccessDataModel = new LoginDataModel
            {
                UserId = 1,
                Token = RandomHelper.GetRandomString(255)
            };
            Mock.Get(lRequestHandler).Setup(
                    handler => handler.MakeRequestAsync(
                        It.IsAny<IRequestBuilderWithResult<LoginDataModel>>(), lCancellationToken
                    )
                )
                .ReturnsAsync(() => new ProxerResult<LoginDataModel>(lSuccessDataModel));

            IProxerClient lClient = ProxerClient.Create(
                new char[32], options => { options.ContainerBuilder.RegisterInstance(lRequestHandler); });
            ILoginManager lLoginManager = lClient.Container.Resolve<ILoginManager>();

            IProxerResult lResult = await lLoginManager.PerformLoginAsync(
                new LoginInput("username", "password", new string(new char[6])),
                lCancellationToken
            );
            Assert.True(lResult.Success);
            Assert.IsEmpty(lResult.Exceptions);

            Assert.AreEqual(lSuccessDataModel.Token, lLoginManager.LoginToken);
            Assert.True(lLoginManager.CheckIsLoginProbablyValid());*/
            Assert.Fail("Authentication not implemented");
        }

        [Test]
        public async Task PerformLogoutTest()
        {
            /*CancellationToken lCancellationToken = new CancellationToken();

            IRequestHandler lRequestHandler = Mock.Of<IRequestHandler>();
            Mock.Get(lRequestHandler).Setup(
                handler => handler.MakeRequestAsync(
                    It.Is<IRequestBuilder>(result => result.BuildUri().PathAndQuery.StartsWith("/api/v1/user/logout")),
                    lCancellationToken
                )
            ).ReturnsAsync(() => new ProxerResult());

            IProxerClient lClient = ProxerClient.Create(
                new char[32], options => { options.ContainerBuilder.RegisterInstance(lRequestHandler); });
            ILoginManager lLoginManager = lClient.Container.Resolve<ILoginManager>();

            IProxerResult lResult = await lLoginManager.PerformLogoutAsync(lCancellationToken);
            Assert.True(lResult.Success);
            Assert.IsEmpty(lResult.Exceptions);

            Assert.Null(lLoginManager.LoginToken);
            Assert.False(lLoginManager.CheckIsLoginProbablyValid());*/
            Assert.Fail("Authentication not implemented");
        }

        [Test]
        public void UpdateTest()
        {
            var client = ProxerClient.Create(new char[32]);
            var loginManager = new DefaultLoginManager(new char[255]);
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
            var loginManager = new DefaultLoginManager(loginToken);
            var request = new RequestBuilder(new Uri("http://proxer.me/api"), client).WithLoginCheck();
            loginManager.AddAuthenticationInformation(request);
            loginManager.Update(request, new ProxerResult());

            return loginManager;
        }
    }
}