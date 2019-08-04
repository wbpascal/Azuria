using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Azuria.Api.Builder;
using Azuria.ErrorHandling;
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
            IProxerClient lClient = ProxerClient.Create(new char[32]);

            RequestBuilder lRequest = new RequestBuilder(new Uri("https://proxer.me"), lClient);
            CancellationTokenSource lCancellationTokenSource = new CancellationTokenSource();

            IProxerResult lResult = await lRequest.DoRequestAsync(lCancellationTokenSource.Token);
            Assert.True(lResult.Success);
            Assert.IsEmpty(lResult.Exceptions);
        }

        [Test]
        public async Task DoRequestAsyncWithResultTest()
        {
            IProxerClient lClient = ProxerClient.Create(new char[32]);

            IRequestBuilderWithResult<object> lRequest = new RequestBuilder(new Uri("https://proxer.me"), lClient)
                .WithResult<object>();
            CancellationTokenSource lCancellationTokenSource = new CancellationTokenSource();

            IProxerResult<object> lResult = await lRequest.DoRequestAsync(lCancellationTokenSource.Token);
            Assert.True(lResult.Success);
            Assert.IsEmpty(lResult.Exceptions);
            Assert.NotNull(lResult.Result);
        }
    }
}