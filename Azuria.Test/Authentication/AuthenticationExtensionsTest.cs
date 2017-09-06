using System.Threading;
using System.Threading.Tasks;
using Azuria.Authentication;
using Azuria.ErrorHandling;
using Moq;
using NUnit.Framework;

namespace Azuria.Test.Authentication
{
    [TestFixture]
    public class AuthenticationExtensionsTest
    {
        private const string Password = "password";
        private const string SecretKey = "ACGES1";
        private const string Username = "username";
        private readonly CancellationToken _cancellationToken = new CancellationToken();
        private readonly Mock<ILoginManager> _loginMangerMock;
        private readonly IProxerClient _proxerClient;

        public AuthenticationExtensionsTest()
        {
            this._loginMangerMock = new Mock<ILoginManager>();
            this._loginMangerMock
                .Setup(manager => manager.PerformLoginAsync(Username, Password, SecretKey, this._cancellationToken))
                .ReturnsAsync(new ProxerResult());
            this._loginMangerMock
                .Setup(manager => manager.PerformLogoutAsync(this._cancellationToken))
                .ReturnsAsync(new ProxerResult());
            this._proxerClient = ProxerClient.Create(
                new char[32], options => options.WithCustomLoginManager(context => this._loginMangerMock.Object)
            );
        }

        [Test]
        public async Task PerformLoginAsyncCalledLoginManagerMethodTest()
        {
            IProxerResult lResult =
                await this._proxerClient.LoginAsync("username", "password", SecretKey, this._cancellationToken);
            Assert.True(lResult.Success);
            Assert.IsEmpty(lResult.Exceptions);
            this._loginMangerMock.Verify(
                manager => manager.PerformLoginAsync(Username, Password, SecretKey, this._cancellationToken),
                Times.Once
            );
        }

        [Test]
        public async Task PerformLogoutAsyncCalledLoginManagerMethodTest()
        {
            IProxerResult lResult = await this._proxerClient.LogoutAsync(this._cancellationToken);
            Assert.True(lResult.Success);
            Assert.IsEmpty(lResult.Exceptions);
            this._loginMangerMock.Verify(manager => manager.PerformLogoutAsync(this._cancellationToken), Times.Once);
        }
    }
}