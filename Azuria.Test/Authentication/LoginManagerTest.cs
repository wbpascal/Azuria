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
    public class LoginManagerTest
    {
        private const string LoginTokenHeaderName = "proxer-api-token";
        
        [Test]
        public void AddAuthenticationInformation_AddsCorrectInfosTest()
        {
            var client = ProxerClient.Create(new char[32]);
            
            var loginToken = RandomHelper.GetRandomString(255).ToCharArray();
            LoginManager loginManager = new LoginManager(loginToken);
            
            var request = new RequestBuilder(new Uri("http://proxer.me/api"), client).WithLoginCheck();
            bool added = loginManager.AddAuthenticationInformation(request);
            Assert.True(added);
            Assert.True(request.Headers.ContainsKey(LoginTokenHeaderName));
            Assert.AreEqual(loginToken.ToString(), request.Headers[LoginTokenHeaderName]);
        }

        [Test]
        public void AddAuthenticationInformation_SkipsIfLoginNotNeededTest()
        {
            var client = ProxerClient.Create(new char[32]);
            
            var loginToken = RandomHelper.GetRandomString(255).ToCharArray();
            LoginManager loginManager = new LoginManager(loginToken);
            
            var request = new RequestBuilder(new Uri("http://proxer.me/api"), client).WithLoginCheck(false);
            bool added = loginManager.AddAuthenticationInformation(request);
            Assert.False(added);
            Assert.False(request.Headers.ContainsKey(LoginTokenHeaderName));
        }

        [Test]
        public void AddAuthenticationInformation_SkipsIfRequestContainsAuthInfosTest()
        {
            var client = ProxerClient.Create(new char[32]);
            
            var loginToken = RandomHelper.GetRandomString(255).ToCharArray();
            LoginManager loginManager = new LoginManager(loginToken);
            
            var request = 
                new RequestBuilder(new Uri("http://proxer.me/api"), client)
                    .WithLoginCheck(false)
                    .WithHeader(LoginTokenHeaderName, "token");
            bool added = loginManager.AddAuthenticationInformation(request);
            Assert.False(added);
            Assert.True(request.Headers.ContainsKey(LoginTokenHeaderName));
            Assert.AreEqual("token", request.Headers[LoginTokenHeaderName]);
        }
        
        [Test]
        public void AddAuthenticationInformation_SkipsIfTokenInvalidTest()
        {
            var client = ProxerClient.Create(new char[32]);
            
            var loginToken = RandomHelper.GetRandomString(42).ToCharArray();
            LoginManager loginManager = new LoginManager(loginToken);
            
            var request = new RequestBuilder(new Uri("http://proxer.me/api"), client).WithLoginCheck();
            bool added = loginManager.AddAuthenticationInformation(request);
            Assert.False(added);
            Assert.False(request.Headers.ContainsKey(LoginTokenHeaderName));
            
            loginManager = new LoginManager();
            
            added = loginManager.AddAuthenticationInformation(request);
            Assert.False(added);
            Assert.False(request.Headers.ContainsKey(LoginTokenHeaderName));
        }
        
        [Test]
        public void UpdateTest()
        {
            LoginManager lLoginManager = new LoginManager(new char[255]);
            Assert.False(lLoginManager.IsLoginProbablyValid());
            lLoginManager.Update(new ProxerResult(), false);
            Assert.False(lLoginManager.IsLoginProbablyValid());
            lLoginManager.Update(new ProxerResult(), true);
            Assert.True(lLoginManager.IsLoginProbablyValid());
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
        public void InvalidateLoginTest()
        {
            var client = ProxerClient.Create(new char[32]);
            var loginManager = new LoginManager(new char[255]);
            //Pretend, that the user logged in
            loginManager.Update(new ProxerResult(), true);

            var request = new RequestBuilder(new Uri("https://proxer.me/api"), client).WithLoginCheck();
            Assert.False(loginManager.AddAuthenticationInformation(request));
            loginManager.InvalidateLogin();
            Assert.True(loginManager.AddAuthenticationInformation(request));
        }
    }
}