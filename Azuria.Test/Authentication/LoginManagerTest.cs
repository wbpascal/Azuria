using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Azuria.Api.v1.DataModels.User;
using Azuria.Authentication;
using Azuria.ErrorHandling;
using Azuria.Requests;
using Azuria.Requests.Builder;
using Azuria.Test.Core.Helpers;
using Moq;
using NUnit.Framework;

namespace Azuria.Test.Authentication
{
    [TestFixture]
    public class LoginManagerTest
    {
        [Test]
        public void LoginTokenTest()
        {
            char[] lRandomToken = RandomHelper.GetRandomString(255).ToCharArray();
            IProxerClient lClient = ProxerClient.Create(
                new char[32], options => options.WithAuthorisation(lRandomToken));
            ILoginManager lLoginManager = lClient.Container.Resolve<ILoginManager>();
            Assert.AreSame(lRandomToken, lLoginManager.LoginToken);
        }

        [Test]
        public void PerformedRequestTest()
        {
            IProxerClient lClient = ProxerClient.Create(new char[32]);
            ILoginManager lLoginManager = lClient.Container.Resolve<ILoginManager>();
            lLoginManager.LoginToken = new char[255];
            Assert.False(lLoginManager.CheckIsLoginProbablyValid());
            lLoginManager.PerformedRequest(false);
            Assert.False(lLoginManager.CheckIsLoginProbablyValid());
            lLoginManager.PerformedRequest(true);
            Assert.True(lLoginManager.CheckIsLoginProbablyValid());
        }

        [Test]
        public async Task PerformLoginTest()
        {
            CancellationToken lCancellationToken = new CancellationTokenSource().Token;

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
                                        "username", "password", token: lCancellationToken);
            Assert.True(lResult.Success);
            Assert.IsEmpty(lResult.Exceptions);

            Assert.AreEqual(lSuccessDataModel.Token, lLoginManager.LoginToken);
            Assert.True(lLoginManager.CheckIsLoginProbablyValid());
        }

        [Test]
        public async Task PerformLoginWith2FaTokenTest()
        {
            CancellationToken lCancellationToken = new CancellationTokenSource().Token;

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
                                        "username", "password", new string(new char[6]), lCancellationToken
                                    );
            Assert.True(lResult.Success);
            Assert.IsEmpty(lResult.Exceptions);

            Assert.AreEqual(lSuccessDataModel.Token, lLoginManager.LoginToken);
            Assert.True(lLoginManager.CheckIsLoginProbablyValid());
        }

        [Test]
        public async Task PerformLogoutTest()
        {
            CancellationToken lCancellationToken = new CancellationToken();

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
            Assert.False(lLoginManager.CheckIsLoginProbablyValid());
        }

        [Test]
        public void QueueLoginForNextRequestTest()
        {
            IProxerClient lClient = ProxerClient.Create(new char[32]);
            ILoginManager lLoginManager = lClient.Container.Resolve<ILoginManager>();
            lLoginManager.LoginToken = new char[255];
            //Pretend, that the user logged in
            lLoginManager.PerformedRequest(true);

            Assert.False(lLoginManager.SendTokenWithNextRequest());
            lLoginManager.QueueLoginForNextRequest();
            Assert.True(lLoginManager.SendTokenWithNextRequest());
        }

        [Test]
        public void SendTokenWithNextRequest()
        {
            IProxerClient lClient = ProxerClient.Create(new char[32]);
            ILoginManager lLoginManager = lClient.Container.Resolve<ILoginManager>();
            Assert.False(lLoginManager.SendTokenWithNextRequest());
            lLoginManager.LoginToken = new char[255];
            Assert.True(lLoginManager.SendTokenWithNextRequest());
            lLoginManager.PerformedRequest(true);
            Assert.False(lLoginManager.SendTokenWithNextRequest());
        }
    }
}