using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Azuria.Authentication;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Requests;
using Azuria.Requests.Builder;
using Azuria.Requests.Http;
using Azuria.Test.Core;
using Azuria.Test.Core.Helpers;
using Azuria.Test.Serilization;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace Azuria.Test.Requests
{
    public class RequestHandlerTest
    {
        private static readonly Dictionary<string, string> StandardHeaders = new Dictionary<string, string>
        {
            {TestConstants.ApiKeyHeaderName, new string(new char[32])}
        };

        private static readonly Dictionary<string, string> StandardAuthenticationHeaders =
            new Dictionary<string, string>
            {
                {TestConstants.ApiKeyHeaderName, new string(new char[32])},
                {TestConstants.LoginTokenHeaderName, new string(ArrayHelpers.GetRandomChars(255))}
            };

        private readonly IRequestHandler _requestHandler;

        public RequestHandlerTest()
        {
            IProxerClient lClient = ProxerClient.Create(new char[32]);
            this._requestHandler = lClient.Container.Resolve<IRequestHandler>();
        }

        [Fact]
        public async Task CancelNonApiRequestsTest()
        {
            IRequestBuilder lRequestBuilder =
                new RequestBuilder(new Uri("https://google.com"), ProxerClient.Create(new char[32]));

            IProxerResult lResult =
                await this._requestHandler.MakeRequestAsync(lRequestBuilder, CancellationToken.None);
            Assert.False(lResult.Success);
            Assert.True(lResult.Exceptions.Any(exception => exception.GetType() == typeof(InvalidRequestException)));

            IProxerResult<object> lResultWithObject = await this._requestHandler.MakeRequestAsync(
                                                          lRequestBuilder.WithResult<object>(), CancellationToken.None
                                                      );
            Assert.False(lResultWithObject.Success);
            Assert.True(
                lResultWithObject.Exceptions.Any(exception => exception.GetType() == typeof(InvalidRequestException))
            );
        }

        [Fact]
        public async Task MakeRequestGetTest()
        {
            Mock<IHttpClient> lHttpClientMock = new Mock<IHttpClient>();
            lHttpClientMock.Setup(
                client =>
                    client.GetRequestAsync(
                        new Uri("https://proxer.me/api/v1/user/test?test=value"), StandardHeaders,
                        CancellationToken.None
                    )
            ).ReturnsAsync(() => new ProxerResult<string>(TestConstants.DummySuccessResponseString));

            IProxerClient lClient = ProxerClient.Create(
                new char[32], options => options.WithCustomHttpClient(context => lHttpClientMock.Object)
            );
            IRequestHandler lRequestHandler = lClient.Container.Resolve<IRequestHandler>();

            IRequestBuilder lRequestBuilder = new RequestBuilder(new Uri("https://proxer.me/api/v1/user/test"), lClient)
                .WithGetParameter("test", "value");
            IProxerResult lResult = await lRequestHandler.MakeRequestAsync(lRequestBuilder, CancellationToken.None);
            Assert.True(lResult.Success, lResult.Exceptions.GetExceptionInfo());
            Assert.Empty(lResult.Exceptions);
        }

        [Fact]
        public async Task MakeRequestPostTest()
        {
            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"test", "value"}
            };

            Mock<IHttpClient> lHttpClientMock = new Mock<IHttpClient>();
            lHttpClientMock.Setup(
                client =>
                    client.PostRequestAsync(
                        new Uri("https://proxer.me/api/v1/user/test"), lPostArgs, StandardHeaders,
                        CancellationToken.None
                    )
            ).ReturnsAsync(() => new ProxerResult<string>(TestConstants.DummySuccessResponseString));

            IProxerClient lClient = ProxerClient.Create(
                new char[32], options => options.WithCustomHttpClient(context => lHttpClientMock.Object)
            );
            IRequestHandler lRequestHandler = lClient.Container.Resolve<IRequestHandler>();

            IRequestBuilder lRequestBuilder = new RequestBuilder(new Uri("https://proxer.me/api/v1/user/test"), lClient)
                .WithPostParameter(lPostArgs);
            IProxerResult lResult = await lRequestHandler.MakeRequestAsync(lRequestBuilder, CancellationToken.None);
            Assert.True(lResult.Success, lResult.Exceptions.GetExceptionInfo());
            Assert.Empty(lResult.Exceptions);
        }

        [Fact]
        public async Task MakeRequestWithResultGetTest()
        {
            Mock<IHttpClient> lHttpClientMock = new Mock<IHttpClient>();
            lHttpClientMock.Setup(
                client =>
                    client.GetRequestAsync(
                        new Uri("https://proxer.me/api/v1/user/test?test=value"), StandardHeaders,
                        CancellationToken.None
                    )
            ).ReturnsAsync(() => new ProxerResult<string>(TestConstants.DummySuccessResponseString));

            IProxerClient lClient = ProxerClient.Create(
                new char[32], options => options.WithCustomHttpClient(context => lHttpClientMock.Object)
            );
            IRequestHandler lRequestHandler = lClient.Container.Resolve<IRequestHandler>();

            IRequestBuilderWithResult<int> lRequestBuilder =
                new RequestBuilder(new Uri("https://proxer.me/api/v1/user/test"), lClient)
                    .WithGetParameter("test", "value")
                    .WithResult<int>()
                    .WithCustomDataConverter(new TestConverter());
            IProxerResult<int> lResult =
                await lRequestHandler.MakeRequestAsync(lRequestBuilder, CancellationToken.None);
            Assert.True(lResult.Success, lResult.Exceptions.GetExceptionInfo());
            Assert.Empty(lResult.Exceptions);
            Assert.Equal(42, lResult.Result);
        }

        [Fact]
        public async Task MakeRequestWithResultPostTest()
        {
            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"test", "value"}
            };

            Mock<IHttpClient> lHttpClientMock = new Mock<IHttpClient>();
            lHttpClientMock.Setup(
                client =>
                    client.PostRequestAsync(
                        new Uri("https://proxer.me/api/v1/user/test"), lPostArgs, StandardHeaders,
                        CancellationToken.None
                    )
            ).ReturnsAsync(() => new ProxerResult<string>(TestConstants.DummySuccessResponseString));

            IProxerClient lClient = ProxerClient.Create(
                new char[32], options => options.WithCustomHttpClient(context => lHttpClientMock.Object)
            );
            IRequestHandler lRequestHandler = lClient.Container.Resolve<IRequestHandler>();

            IRequestBuilderWithResult<int> lRequestBuilder =
                new RequestBuilder(new Uri("https://proxer.me/api/v1/user/test"), lClient)
                    .WithPostParameter(lPostArgs)
                    .WithResult<int>()
                    .WithCustomDataConverter(new TestConverter());
            IProxerResult<int> lResult =
                await lRequestHandler.MakeRequestAsync(lRequestBuilder, CancellationToken.None);
            Assert.True(lResult.Success, lResult.Exceptions.GetExceptionInfo());
            Assert.Empty(lResult.Exceptions);
            Assert.Equal(42, lResult.Result);
        }

        [Fact]
        public async Task MakeRequestPerformRequestCalledTest()
        {
            Mock<ILoginManager> lLoginManagerMock = new Mock<ILoginManager>();
            lLoginManagerMock.Setup(manager => manager.PerformedRequest(It.IsAny<bool>()));
            lLoginManagerMock.SetupGet(manager => manager.LoginToken)
                .Returns(StandardAuthenticationHeaders[TestConstants.LoginTokenHeaderName].ToCharArray);
            lLoginManagerMock.Setup(manager => manager.SendTokenWithNextRequest()).Returns(true);

            Mock<IHttpClient> lHttpClientMock = new Mock<IHttpClient>();
            lHttpClientMock.Setup(
                client =>
                    client.GetRequestAsync(
                        new Uri("https://proxer.me/api/v1/user/test?test=value"), StandardAuthenticationHeaders,
                        CancellationToken.None
                    )
            ).ReturnsAsync(() => new ProxerResult<string>(TestConstants.DummySuccessResponseString));

            IProxerClient lClient = ProxerClient.Create(
                new char[32], options =>
                {
                    options.ContainerBuilder.RegisterInstance(lLoginManagerMock.Object);
                    options.ContainerBuilder.RegisterInstance(lHttpClientMock.Object);
                });
            IRequestHandler lRequestHandler = lClient.Container.Resolve<IRequestHandler>();
            IRequestBuilder lRequestBuilder = new RequestBuilder(new Uri("https://proxer.me/api/v1/user/test"), lClient)
                .WithGetParameter("test", "value");
            IProxerResult lResult = await lRequestHandler.MakeRequestAsync(lRequestBuilder, CancellationToken.None);
            Assert.True(lResult.Success);
            Assert.Empty(lResult.Exceptions);

            lLoginManagerMock.Verify(manager => manager.PerformedRequest(true), Times.Once);
        }

        [Fact]
        public async Task MakeRequestCheckLoginTest()
        {
            Mock<ILoginManager> lLoginManagerMock = new Mock<ILoginManager>();
            lLoginManagerMock.Setup(manager => manager.CheckIsLoginProbablyValid()).Returns(false);
            lLoginManagerMock.Setup(manager => manager.SendTokenWithNextRequest()).Returns(false);

            IProxerClient lClient = ProxerClient.Create(
                new char[32], options => { options.ContainerBuilder.RegisterInstance(lLoginManagerMock.Object); });
            IRequestHandler lRequestHandler = lClient.Container.Resolve<IRequestHandler>();
            IRequestBuilder lRequestBuilder = new RequestBuilder(new Uri("https://proxer.me/api/v1/user/test"), lClient)
                .WithGetParameter("test", "value")
                .WithLoginCheck();
            IProxerResult lResult = await lRequestHandler.MakeRequestAsync(lRequestBuilder, CancellationToken.None);
            Assert.False(lResult.Success);
            Assert.True(lResult.Exceptions.Any(exception => typeof(NotAuthenticatedException) == exception.GetType()));

            Mock<IHttpClient> lHttpClientMock = new Mock<IHttpClient>();
            lHttpClientMock.Setup(
                client =>
                    client.GetRequestAsync(
                        new Uri("https://proxer.me/api/v1/user/test?test=value"), StandardAuthenticationHeaders,
                        CancellationToken.None
                    )
            ).ReturnsAsync(() => new ProxerResult<string>(TestConstants.DummySuccessResponseString));
            lLoginManagerMock.Setup(manager => manager.SendTokenWithNextRequest()).Returns(true);
            lLoginManagerMock.SetupGet(manager => manager.LoginToken)
                .Returns(StandardAuthenticationHeaders[TestConstants.LoginTokenHeaderName].ToCharArray);
            lClient = ProxerClient.Create(
                new char[32], options =>
                {
                    options.ContainerBuilder.RegisterInstance(lLoginManagerMock.Object);
                    options.ContainerBuilder.RegisterInstance(lHttpClientMock.Object);
                });
            lRequestHandler = lClient.Container.Resolve<IRequestHandler>();
            lRequestBuilder = new RequestBuilder(new Uri("https://proxer.me/api/v1/user/test"), lClient)
                .WithGetParameter("test", "value")
                .WithLoginCheck();
            lResult = await lRequestHandler.MakeRequestAsync(lRequestBuilder, CancellationToken.None);
            Assert.True(lResult.Success);
            Assert.Empty(lResult.Exceptions);
        }
    }
}