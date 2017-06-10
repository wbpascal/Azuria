using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Azuria.Api.Builder;
using Azuria.ErrorHandling;
using Azuria.Requests;
using Azuria.Requests.Builder;
using Moq;
using Xunit;

namespace Azuria.Test.Requests
{
    public class RequestExtensionsTest
    {
        private readonly IProxerClient _client;

        public RequestExtensionsTest()
        {
            this._client = ProxerClient.Create(new char[32]);
        }

        [Fact]
        public void CreateRequestTest()
        {
            Mock<IApiRequestBuilder> lApiRequestBuilderMock = new Mock<IApiRequestBuilder>();
            lApiRequestBuilderMock.Setup(builder => builder.FromUrl(new Uri("https://proxer.me"))).Returns(() => null);

            IProxerClient lClient = ProxerClient.Create(
                new char[32], options => { options.ContainerBuilder.RegisterInstance(lApiRequestBuilderMock.Object); }
            );

            IApiRequestBuilder lApiRequestBuilder = lClient.CreateRequest();
            Assert.Same(lApiRequestBuilderMock.Object, lApiRequestBuilder);

            IRequestBuilder lRequestBuilder = lApiRequestBuilder.FromUrl(new Uri("https://proxer.me"));
            Assert.Null(lRequestBuilder);

            lApiRequestBuilderMock.Verify(builder => builder.FromUrl(new Uri("https://proxer.me")), Times.Exactly(1));
        }

        [Fact]
        public async Task DoRequestAsyncTest()
        {
            Mock<IRequestHandler> lRequestHandlerMock = new Mock<IRequestHandler>();
            IProxerClient lClient = ProxerClient.Create(
                new char[32], options => { options.ContainerBuilder.RegisterInstance(lRequestHandlerMock.Object); });

            RequestBuilder lRequest = new RequestBuilder(new Uri("https://proxer.me"), lClient);
            CancellationTokenSource lCancellationTokenSource = new CancellationTokenSource();

            lRequestHandlerMock.Setup(handler => handler.MakeRequestAsync(lRequest, lCancellationTokenSource.Token))
                .Returns(() => Task.FromResult((IProxerResult) new ProxerResult()));

            IProxerResult lResult = await lRequest.DoRequestAsync(lCancellationTokenSource.Token);
            Assert.True(lResult.Success);
            Assert.Empty(lResult.Exceptions);

            lRequestHandlerMock.Verify(
                handler => handler.MakeRequestAsync(lRequest, lCancellationTokenSource.Token), Times.Once
            );
        }

        [Fact]
        public async Task DoRequestAsyncWithResultTest()
        {
            Mock<IRequestHandler> lRequestHandlerMock = new Mock<IRequestHandler>();
            IProxerClient lClient = ProxerClient.Create(
                new char[32], options => { options.ContainerBuilder.RegisterInstance(lRequestHandlerMock.Object); });

            IRequestBuilderWithResult<object> lRequest = new RequestBuilder(new Uri("https://proxer.me"), lClient)
                .WithResult<object>();
            CancellationTokenSource lCancellationTokenSource = new CancellationTokenSource();

            lRequestHandlerMock.Setup(handler => handler.MakeRequestAsync(lRequest, lCancellationTokenSource.Token))
                .Returns(() => Task.FromResult((IProxerResult<object>) new ProxerResult<object>(new object())));

            IProxerResult<object> lResult = await lRequest.DoRequestAsync(lCancellationTokenSource.Token);
            Assert.True(lResult.Success);
            Assert.Empty(lResult.Exceptions);
            Assert.NotNull(lResult.Result);

            lRequestHandlerMock.Verify(
                handler => handler.MakeRequestAsync(lRequest, lCancellationTokenSource.Token), Times.Once
            );
        }
    }
}