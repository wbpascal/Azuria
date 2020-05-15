using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.Builder;
using Azuria.Enums;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Middleware;
using Azuria.Requests;
using Azuria.Requests.Builder;
using NUnit.Framework;

namespace Azuria.Test.Middleware
{
    public class ErrorMiddlewareTest
    {
        private readonly IApiRequestBuilder _apiRequestBuilder;

        public ErrorMiddlewareTest()
        {
            IProxerClient client = ProxerClient.Create(new char[32]);
            this._apiRequestBuilder = client.CreateRequest();
        }

        [Test]
        public async Task Invoke_PassesResultThroughIfNoError()
        {
            var middleware = new ErrorMiddleware();

            IRequestBuilder request = this._apiRequestBuilder.FromUrl(new Uri("https://proxer.me/api/v1"));
            IProxerResult result = await middleware.Invoke(request, CreateNextMiddlewareStub());
            Assert.True(result.Success);
        }

        [Test]
        public async Task Invoke_ReturnsProxerApiExceptionOnlyForCorrespondingErrorCodes([Values] ErrorCode code)
        {
            var middleware = new ErrorMiddleware();

            IRequestBuilder request = this._apiRequestBuilder.FromUrl(new Uri("https://proxer.me/api/v1"));
            var result = new ProxerApiResponse {ErrorCode = (int) code};
            (bool success, IEnumerable<Exception> exceptions) =
                await middleware.Invoke(request, CreateNextMiddlewareStub(result));
            Assert.False(success);
            Assert.NotNull(exceptions);

            var apiException =
                exceptions.FirstOrDefault(exception => exception is ProxerApiException) as ProxerApiException;
            if (code == ErrorCode.NoError)
            {
                Assert.Null(apiException);
            }
            else
            {
                Assert.NotNull(apiException);
                Assert.AreEqual(code, apiException.ErrorCode);
            }
        }

        [Test]
        public async Task Invoke_StopsIfRequestNotApiUrl()
        {
            var middleware = new ErrorMiddleware();

            IRequestBuilder request = this._apiRequestBuilder.FromUrl(new Uri("https://google.com"));
            (bool success, IEnumerable<Exception> exceptions) =
                await middleware.Invoke(request, CreateNextMiddlewareStub());
            Assert.False(success);
            Assert.NotNull(exceptions);
            Assert.IsNotEmpty(exceptions);
            Assert.True(exceptions.Any(exception => exception is InvalidRequestException));

            request = this._apiRequestBuilder.FromUrl(new Uri("https://proxer.me"));
            (success, exceptions) = await middleware.Invoke(request, CreateNextMiddlewareStub());
            Assert.False(success);
            Assert.NotNull(exceptions);
            Assert.IsNotEmpty(exceptions);
            Assert.True(exceptions.Any(exception => exception is InvalidRequestException));
        }

        [Test]
        public async Task InvokeWithResult_PassesResultThroughIfNoError()
        {
            var middleware = new ErrorMiddleware();

            IRequestBuilderWithResult<object> request = this._apiRequestBuilder
                .FromUrl(new Uri("https://proxer.me/api/v1")).WithResult<object>();
            IProxerResult<object> result =
                await middleware.InvokeWithResult(request, CreateNextMiddlewareStub<object>());
            Assert.True(result.Success);
        }

        [Test]
        public async Task InvokeWithResult_ReturnsProxerApiExceptionOnlyForCorrespondingErrorCodes(
            [Values] ErrorCode code)
        {
            var middleware = new ErrorMiddleware();

            IRequestBuilderWithResult<object> request = this._apiRequestBuilder
                .FromUrl(new Uri("https://proxer.me/api/v1")).WithResult<object>();
            var apiResult = new ProxerApiResponse<object> {ErrorCode = (int) code};
            (bool success, IEnumerable<Exception> exceptions, _) =
                await middleware.InvokeWithResult(request, CreateNextMiddlewareStub(apiResult));
            Assert.False(success);
            Assert.NotNull(exceptions);

            var apiException =
                exceptions.FirstOrDefault(exception => exception is ProxerApiException) as ProxerApiException;
            if (code == ErrorCode.NoError)
            {
                Assert.Null(apiException);
            }
            else
            {
                Assert.NotNull(apiException);
                Assert.AreEqual(code, apiException.ErrorCode);
            }
        }

        [Test]
        public async Task InvokeWithResult_StopsIfRequestNotApiUrl()
        {
            var middleware = new ErrorMiddleware();

            IRequestBuilderWithResult<object> request =
                this._apiRequestBuilder.FromUrl(new Uri("https://google.com")).WithResult<object>();
            (bool success, IEnumerable<Exception> exceptions, _) =
                await middleware.InvokeWithResult(request, CreateNextMiddlewareStub<object>());
            Assert.False(success);
            Assert.NotNull(exceptions);
            Assert.IsNotEmpty(exceptions);
            Assert.True(exceptions.Any(exception => exception is InvalidRequestException));

            request = this._apiRequestBuilder.FromUrl(new Uri("https://proxer.me")).WithResult<object>();
            (success, exceptions, _) = await middleware.InvokeWithResult(request, CreateNextMiddlewareStub<object>());
            Assert.False(success);
            Assert.NotNull(exceptions);
            Assert.IsNotEmpty(exceptions);
            Assert.True(exceptions.Any(exception => exception is InvalidRequestException));
        }

        private static MiddlewareAction CreateNextMiddlewareStub(IProxerResult result = null)
        {
            return (request, token) => Task.FromResult(result ?? new ProxerResult());
        }

        private static MiddlewareAction<T> CreateNextMiddlewareStub<T>(IProxerResult<T> result = null)
        {
            return (request, token) => Task.FromResult(result ?? new ProxerResult<T>(default(T)));
        }
    }
}