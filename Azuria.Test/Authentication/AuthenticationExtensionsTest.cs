using System.Threading;
using System.Threading.Tasks;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.Input.User;
using Azuria.Authentication;
using Azuria.ErrorHandling;
using Azuria.Test.Core.Helpers;
using Moq;
using NUnit.Framework;

namespace Azuria.Test.Authentication
{
    [TestFixture]
    public class AuthenticationExtensionsTest
    {
        [Test]
        public async Task PerformLoginAsyncCalledLoginManagerMethodTest()
        {
            var loginInput = new LoginInput("username", "password", "ACGES1");
            var successDataModel = new LoginDataModel
            {
                UserId = 1,
                Token = RandomHelper.GetRandomString(255)
            };

            Mock<DefaultLoginManager> loginManagerMock = null;
            IProxerClient client = ProxerClient.Create(
                new char[32],
                options =>
                {
                    loginManagerMock = new Mock<DefaultLoginManager>(options.Client, new char[255]);
                    loginManagerMock
                        .Setup(manager => manager.PerformLoginAsync(loginInput, It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult(
                            (IProxerResult<LoginDataModel>) new ProxerResult<LoginDataModel>(successDataModel))
                        );
                    options.WithCustomLoginManager(loginManagerMock.Object);
                });

            IProxerResult<LoginDataModel> result = await client.LoginAsync(
                new LoginInput("username", "password", "ACGES1"), CancellationToken.None
            );
            Assert.True(result.Success);
            Assert.IsEmpty(result.Exceptions);
            Assert.AreSame(successDataModel, result.Result);

            loginManagerMock.Verify(manager => manager.PerformLoginAsync(loginInput, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task PerformLogoutAsyncCalledLoginManagerMethodTest()
        {
            Mock<DefaultLoginManager> loginManagerMock = null;
            IProxerClient client = ProxerClient.Create(
                new char[32],
                options =>
                {
                    loginManagerMock = new Mock<DefaultLoginManager>(options.Client, new char[255]);
                    loginManagerMock
                        .Setup(manager => manager.PerformLogoutAsync(It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult((IProxerResult) new ProxerResult())
                        );
                    options.WithCustomLoginManager(loginManagerMock.Object);
                });

            IProxerResult result = await client.LogoutAsync(CancellationToken.None);
            Assert.True(result.Success);
            Assert.IsEmpty(result.Exceptions);

            loginManagerMock.Verify(manager => manager.PerformLogoutAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}