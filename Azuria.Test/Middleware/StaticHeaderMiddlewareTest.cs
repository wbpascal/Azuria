using System;
using System.Collections.Generic;
using Azuria.Api.Builder;
using Azuria.Middleware;
using Azuria.Requests;
using Azuria.Requests.Builder;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Threading;
using Azuria.ErrorHandling;
using Azuria.Test.Core.Helpers;

namespace Azuria.Test.Middleware
{
    public class StaticHeaderMiddlewareTest
    {
        private readonly IApiRequestBuilder _apiRequestBuilder;

        public StaticHeaderMiddlewareTest()
        {
            IProxerClient client = ProxerClient.Create(new char[32]);
            this._apiRequestBuilder = client.CreateRequest();
        }
        
        [Test]
        public async Task Invoke_AddsHeadersFromConstructorArg() 
        {
            // Create random headers to test with
            var header = new Dictionary<string, string>() {
                {RandomHelper.GetRandomString(20), RandomHelper.GetRandomString(10)},
                {RandomHelper.GetRandomString(5), RandomHelper.GetRandomString(5)}
            };

            var middleware = new StaticHeaderMiddleware(header);
            IRequestBuilder builder = this._apiRequestBuilder.FromUrl(new Uri("https://proxer.me"));
    
            // Create mock of next middleware in pipeline that asserts needed conditions
            MiddlewareAction action = (request, token) => {
                foreach (var item in header)
                {
                    Assert.True(request.Headers.Contains(item));
                }
                return Task.FromResult((IProxerResult) new ProxerResult());
            };

            // Test with request that does not contain prior headers
            IProxerResult result = await middleware.Invoke(builder, action);
            Assert.True(result.Success);

            // Test with request that does contain prior headers
            builder.WithHeader(RandomHelper.GetRandomString(4), RandomHelper.GetRandomString(20));
            result = await middleware.Invoke(builder, action);
            Assert.True(result.Success);
        }

        [Test]
        public async Task Invoke_PassesCancellationTokenToNextMiddleware() 
        {
            var middleware = new StaticHeaderMiddleware(new Dictionary<string, string>());
            IRequestBuilder builder = this._apiRequestBuilder.FromUrl(new Uri("https://proxer.me"));
    
            var tokenSource = new CancellationTokenSource();

            // Create mock of next middleware in pipeline that asserts needed conditions
            MiddlewareAction action = (request, token) => {
                Assert.NotNull(token);
                Assert.AreEqual(tokenSource.Token, token);
                return Task.FromResult((IProxerResult) new ProxerResult());
            };

            IProxerResult result = await middleware.Invoke(builder, action, tokenSource.Token);
            Assert.True(result.Success);
        }
        
        [Test]
        public async Task InvokeWithResult_AddsHeadersFromConstructorArg() 
        {
            // Create random headers to test with
            var header = new Dictionary<string, string>() {
                {RandomHelper.GetRandomString(20), RandomHelper.GetRandomString(10)},
                {RandomHelper.GetRandomString(5), RandomHelper.GetRandomString(5)}
            };

            var middleware = new StaticHeaderMiddleware(header);
            IRequestBuilderWithResult<object> builder = 
                this._apiRequestBuilder
                    .FromUrl(new Uri("https://proxer.me"))
                    .WithResult<object>();
    
            // Create mock of next middleware in pipeline that asserts needed conditions
            MiddlewareAction<object> action = (request, token) => {
                foreach (var item in header)
                {
                    Assert.True(request.Headers.Contains(item));
                }
                return Task.FromResult((IProxerResult<object>) new ProxerResult<object>(new object()));
            };

            // Test with request that does not contain prior headers
            IProxerResult<object> result = await middleware.InvokeWithResult(builder, action);
            Assert.True(result.Success);

            // Test with request that does contain prior headers
            builder.WithHeader(RandomHelper.GetRandomString(4), RandomHelper.GetRandomString(20));
            result = await middleware.InvokeWithResult(builder, action);
            Assert.True(result.Success);
        }

        [Test]
        public async Task InvokeWithResult_PassesCancellationTokenToNextMiddleware() 
        {
            var middleware = new StaticHeaderMiddleware(new Dictionary<string, string>());
            IRequestBuilderWithResult<object> builder = 
                this._apiRequestBuilder
                    .FromUrl(new Uri("https://proxer.me"))
                    .WithResult<object>();
    
            var tokenSource = new CancellationTokenSource();

            // Create mock of next middleware in pipeline that asserts needed conditions
            MiddlewareAction<object> action = (request, token) => {
                Assert.NotNull(token);
                Assert.AreEqual(tokenSource.Token, token);
                return Task.FromResult((IProxerResult<object>) new ProxerResult<object>(new object()));
            };

            IProxerResult<object> result = await middleware.InvokeWithResult(builder, action, tokenSource.Token);
            Assert.True(result.Success);
        }

        [Test]
        public async Task InvokeWithResult_ReturnsResultFromLastMiddleware() 
        {
            var middleware = new StaticHeaderMiddleware(new Dictionary<string, string>());
            IRequestBuilderWithResult<object> builder = 
                this._apiRequestBuilder
                    .FromUrl(new Uri("https://proxer.me"))
                    .WithResult<object>();
    
            var obj = new object();

            // Create mock of next middleware in pipeline that asserts needed conditions
            MiddlewareAction<object> action = (request, token) => {
                return Task.FromResult((IProxerResult<object>) new ProxerResult<object>(obj));
            };

            IProxerResult<object> result = await middleware.InvokeWithResult(builder, action);
            Assert.True(result.Success);
            Assert.AreSame(obj, result.Result);
        }
    }
}