using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Azuria.Api.Builder;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.User;
using Azuria.Authentication;
using Azuria.Enums;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Requests;
using Azuria.Requests.Http;
using Azuria.Serialization;
using Azuria.Test.Core.Helpers;
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
            Assert.True(lClient.Container.IsRegistered<ILoginManager>());
            Assert.True(lClient.Container.IsRegistered<IRequestHandler>());
            Assert.True(lClient.Container.IsRegistered<IApiRequestBuilder>());
            Assert.True(lClient.Container.IsRegistered<IRequestErrorHandler>());
            Assert.True(lClient.Container.IsRegistered<IJsonDeserializer>());
            Assert.True(lClient.Container.IsRegistered<IRequestHeaderManager>());
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
            Assert.True(lResult.Success, lResult.Exceptions.GetExceptionInfo());
            Assert.Empty(lResult.Exceptions);

            Assert.True(lLoginManager.CheckIsLoginProbablyValid());
        }

        [Fact]
        public async Task LoginTokenTest()
        {
            IProxerClient lClient = ProxerClient.Create(
                new char[32],
                options =>
                    options.WithTestingHttpClient()
                        .WithAuthorisation(new char[255].Select(_ => 'a').ToArray())
            );
            ILoginManager lLoginManager = lClient.Container.Resolve<ILoginManager>();
            Assert.False(lLoginManager.CheckIsLoginProbablyValid());

            IProxerResult<UserInfoDataModel> lResult =
                await lClient.CreateRequest().FromUserClass().GetInfo().DoRequestAsync();
            Assert.True(lResult.Success, lResult.Exceptions.GetExceptionInfo());
            Assert.Empty(lResult.Exceptions);
            Assert.NotNull(lResult.Result);
            Assert.Equal(lResult.Result.UserId, 177103);
            Assert.Equal(lResult.Result.Username, "InfiniteSoul");

            Assert.True(lLoginManager.CheckIsLoginProbablyValid());
        }

        [Fact]
        public async Task LogoutTest()
        {
            IProxerClient lClient = ProxerClient.Create(
                new char[32], options => options.WithTestingHttpClient());
            IProxerResult lResult = await lClient.LoginAsync("InfiniteSoul", "correct");
            Assert.True(lResult.Success, lResult.Exceptions.GetExceptionInfo());
            Assert.True(lClient.Container.Resolve<ILoginManager>().CheckIsLoginProbablyValid());

            lResult = await lClient.LogoutAsync();
            Assert.True(lResult.Success, lResult.Exceptions.GetExceptionInfo());
            Assert.False(lClient.Container.Resolve<ILoginManager>().CheckIsLoginProbablyValid());
        }
    }
}