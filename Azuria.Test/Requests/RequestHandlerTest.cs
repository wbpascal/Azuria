using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Requests;
using Azuria.Requests.Builder;
using Azuria.Requests.Http;
using Azuria.Test.Core;
using Azuria.Test.Core.Helpers;
using Moq;
using Xunit;

namespace Azuria.Test.Requests
{
    public class RequestHandlerTest
    {
        private static readonly Dictionary<string, string> StandardHeaders = new Dictionary<string, string>
        {
            {TestConstants.ApiKeyHeaderName, new string(new char[32])}
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

            IRequestBuilderWithResult<string> lRequestBuilder =
                new RequestBuilder(new Uri("https://proxer.me/api/v1/user/test"), lClient)
                    .WithGetParameter("test", "value")
                    .WithResult<string>();
            IProxerResult<string> lResult =
                await lRequestHandler.MakeRequestAsync(lRequestBuilder, CancellationToken.None);
            Assert.True(lResult.Success, lResult.Exceptions.GetExceptionInfo());
            Assert.Empty(lResult.Exceptions);
            Assert.Equal("dataValue", lResult.Result);
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

            IRequestBuilderWithResult<string> lRequestBuilder =
                new RequestBuilder(new Uri("https://proxer.me/api/v1/user/test"), lClient)
                    .WithPostParameter(lPostArgs)
                    .WithResult<string>();
            IProxerResult<string> lResult =
                await lRequestHandler.MakeRequestAsync(lRequestBuilder, CancellationToken.None);
            Assert.True(lResult.Success, lResult.Exceptions.GetExceptionInfo());
            Assert.Empty(lResult.Exceptions);
            Assert.Equal("dataValue", lResult.Result);
        }
    }
}