using System;
using System.Threading;
using System.Threading.Tasks;
using Azuria.Api.Builder;
using Azuria.ErrorHandling;
using Azuria.Middleware;
using Azuria.Middleware.Pipeline;
using Azuria.Requests;
using Azuria.Requests.Builder;
using Moq;
using NUnit.Framework;

namespace Azuria.Test.Requests
{
    [TestFixture]
    public class RequestExtensionsTest
    {
        private readonly IProxerClient _client;

        public RequestExtensionsTest()
        {
            this._client = ProxerClient.Create(new char[32]);
        }

        [Test]
        public void CreateRequestTest()
        {
            IProxerClient lClient = ProxerClient.Create(new char[32]);

            IApiRequestBuilder lApiRequestBuilder = lClient.CreateRequest();
            Assert.NotNull(lApiRequestBuilder);
            Assert.AreSame(lClient, lApiRequestBuilder.ProxerClient);
        }

        [Test]
        public async Task DoRequestAsyncTest()
        {
            var middlewareMock = new Mock<IMiddleware>();
            middlewareMock
                .Setup(middleware => middleware.Invoke(It.IsAny<IRequestBuilder>(), It.IsAny<MiddlewareAction>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult((IProxerResult) new ProxerResult()));

            // Create a client with a custom pipeline that only contains the mocked middleware
            var lClient = ProxerClient.Create(new char[32],
                options => options.Pipeline = new Pipeline(new[] {middlewareMock.Object}));

            var lRequest = new RequestBuilder(new Uri("https://proxer.me/api"), lClient);
            CancellationTokenSource lCancellationTokenSource = new CancellationTokenSource();

            IProxerResult lResult = await lRequest.DoRequestAsync(lCancellationTokenSource.Token);
            Assert.NotNull(lResult);
            Assert.True(lResult.Success);
            Assert.IsEmpty(lResult.Exceptions);

            // Verify that the invoke function of the middleware mock was invoked exactly once
            middlewareMock.Verify(
                middleware => middleware.Invoke(It.IsAny<IRequestBuilder>(), It.IsAny<MiddlewareAction>(),
                    It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task DoRequestAsyncWithResultTest()
        {
            var middlewareMock = new Mock<IMiddleware>();
            middlewareMock
                .Setup(middleware => middleware.InvokeWithResult(It.IsAny<IRequestBuilderWithResult<object>>(),
                    It.IsAny<MiddlewareAction<object>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult((IProxerResult<object>) new ProxerResult<object>(new object())));

            // Create a client with a custom pipeline that only contains the mocked middleware
            var lClient = ProxerClient.Create(new char[32],
                options => options.Pipeline = new Pipeline(new[] {middlewareMock.Object}));

            IRequestBuilderWithResult<object> lRequest = new RequestBuilder(new Uri("https://proxer.me"), lClient)
                .WithResult<object>();
            CancellationTokenSource lCancellationTokenSource = new CancellationTokenSource();

            IProxerResult<object> lResult = await lRequest.DoRequestAsync(lCancellationTokenSource.Token);
            Assert.NotNull(lResult);
            Assert.True(lResult.Success);
            Assert.IsEmpty(lResult.Exceptions);
            Assert.NotNull(lResult.Result);

            middlewareMock.Verify(
                middleware => middleware.InvokeWithResult(It.IsAny<IRequestBuilderWithResult<object>>(),
                    It.IsAny<MiddlewareAction<object>>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}