using System;
using System.Threading.Tasks;
using Azuria.Authentication;
using Azuria.ErrorHandling;
using Azuria.Requests.Builder;
using NUnit.Framework;

namespace Azuria.Test.Authentication
{
    [TestFixture]
    public class LoginManagerTest
    {
        [Test]
        public void PerformedRequestTest()
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

            Assert.False(
                loginManager.AddAuthenticationInformation(
                    new RequestBuilder(new Uri("https://proxer.me/api"), client)
                )
            );
            loginManager.InvalidateLogin();
            Assert.True(
                loginManager.AddAuthenticationInformation(
                    new RequestBuilder(new Uri("https://proxer.me/api"), client)
                )
            );
        }

        [Test]
        public void SendTokenWithNextRequest()
        {
            /*IProxerClient lClient = ProxerClient.Create(new char[32]);
            ILoginManager lLoginManager = lClient.Container.Resolve<ILoginManager>();
            Assert.False(lLoginManager.SendTokenWithNextRequest());
            lLoginManager.LoginToken = new char[255];
            Assert.True(lLoginManager.SendTokenWithNextRequest());
            lLoginManager.PerformedRequest(true);
            Assert.False(lLoginManager.SendTokenWithNextRequest());*/
            Assert.Fail("Authentication not implemented. Not possible to modify the token");
        }
    }
}