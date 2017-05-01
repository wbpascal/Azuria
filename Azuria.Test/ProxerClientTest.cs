using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Autofac;
using Azuria.Authentication;
using Azuria.Connection;
using Azuria.Enums;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Test.Core;
using Azuria.Test.Core.Utility;
using Xunit;

namespace Azuria.Test
{
    public class ProxerClientTest
    {
        [Fact]
        public void CreateNoOptionsTest()
        {
            IProxerClient lClient = ProxerClient.Create(new char[32]);
            Assert.NotEmpty(lClient.ApiKey);
            Assert.True(lClient.Container.IsRegistered<IHttpClient>());
            Assert.True(lClient.Container.IsRegistered<IProxerClient>());
        }

        [Fact]
        public async Task LoginTest()
        {
            IProxerClient lClient = ProxerClient.Create(new char[32], options => options.WithTestingHttpClient());
            ILoginManager lLoginManager = lClient.Container.Resolve<ILoginManager>();
            Assert.False(lLoginManager.CheckIsLoginProbablyValid());

            IProxerResult lResult = await lClient.LoginAsync("InfiniteSoul", "wrong");
            Assert.False(lResult.Success);
            Assert.True(
                lResult.Exceptions.Any(
                    exception => (exception as ProxerApiException)?.ErrorCode == ErrorCode.LoginCredentialsWrong
                )
            );

            lResult = await lClient.LoginAsync("InfiniteSoul", "correct");
            Assert.True(lResult.Success);
            Assert.Empty(lResult.Exceptions);

            Assert.True(lLoginManager.CheckIsLoginProbablyValid());
        }
    }
}