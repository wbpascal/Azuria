using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azuria.Api.Builder;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Middleware;
using Azuria.Requests;
using Azuria.Requests.Builder;
using NUnit.Framework;

namespace Azuria.Test.Middleware
{
    public class LoginMiddlewareTest
    {
        private readonly IApiRequestBuilder _apiRequestBuilder;

        public LoginMiddlewareTest()
        {
            IProxerClient client = ProxerClient.Create(new char[32]);
            _apiRequestBuilder = client.CreateRequest();
        }

        [Test]
        public void Constructor_SetsPropertiesTest()
        {
            var loginManager = new TestLoginManager();

            var loginMiddleware = new LoginMiddleware(loginManager);
            Assert.AreSame(loginManager, loginMiddleware.LoginManager);
        }

        [Test]
        public async Task Invoke_AddsAuthenticationInformationFromLoginManagerTest()
        {
            var middleware = new LoginMiddleware(new TestLoginManager());
            var uri = new Uri("https://proxer.me/api/v1/login/test?addLogin=1");
            IRequestBuilder builder = _apiRequestBuilder.FromUrl(uri);

            MiddlewareAction action = (request, token) =>
            {
                Assert.IsTrue(request.Headers.ContainsKey(TestLoginManager.LOGIN_HEADER_KEY));
                Assert.AreEqual(TestLoginManager.LOGIN_HEADER_VALUE,
                    request.Headers[TestLoginManager.LOGIN_HEADER_KEY]);
                return Task.FromResult((IProxerResult) new ProxerResult());
            };

            IProxerResult result = await middleware.Invoke(builder, action).ConfigureAwait(false);
            Assert.True(result.Success);
        }

        [Test]
        public async Task Invoke_DoNotRetryIfDifferentExceptionTest()
        {
            var middleware = new LoginMiddleware(new TestLoginManager());
            var uri = new Uri("https://proxer.me/api/v1/login/test?addLogin=0");
            IRequestBuilder builder = _apiRequestBuilder.FromUrl(uri);

            var actionCalled = 0;

            MiddlewareAction action = (request, token) =>
            {
                actionCalled++;
                if (actionCalled == 1)
                    return Task.FromResult((IProxerResult) new ProxerResult(new Exception()));
                return Task.FromResult((IProxerResult) new ProxerResult());
            };

            IProxerResult result = await middleware.Invoke(builder, action).ConfigureAwait(false);
            Assert.False(result.Success);
            Assert.AreEqual(1, actionCalled);
        }

        [Test]
        public async Task Invoke_DoNotRetryIfLoginManagerAlreadyAddedInformationTest()
        {
            var middleware = new LoginMiddleware(new TestLoginManager());
            var uri = new Uri("https://proxer.me/api/v1/login/test?addLogin=1");
            IRequestBuilder builder = _apiRequestBuilder.FromUrl(uri);

            var actionCalled = 0;

            MiddlewareAction action = (request, token) =>
            {
                actionCalled++;
                if (actionCalled == 1)
                    return Task.FromResult((IProxerResult) new ProxerResult(new NotAuthenticatedException()));
                return Task.FromResult((IProxerResult) new ProxerResult());
            };

            IProxerResult result = await middleware.Invoke(builder, action).ConfigureAwait(false);
            Assert.False(result.Success);
            Assert.AreEqual(1, actionCalled);
        }

        [Test]
        public async Task Invoke_RetriesIfNotAuthenticatedTest()
        {
            var middleware = new LoginMiddleware(new TestLoginManager());
            var uri = new Uri("https://proxer.me/api/v1/login/test?addLogin=0");
            IRequestBuilder builder = _apiRequestBuilder.FromUrl(uri);

            var actionCalled = 0;

            MiddlewareAction action = (request, token) =>
            {
                actionCalled++;
                if (actionCalled == 1)
                    return Task.FromResult((IProxerResult) new ProxerResult(new NotAuthenticatedException()));
                return Task.FromResult((IProxerResult) new ProxerResult());
            };

            IProxerResult result = await middleware.Invoke(builder, action).ConfigureAwait(false);
            Assert.True(result.Success);
            Assert.AreEqual(2, actionCalled);
        }

        [Test]
        public async Task Invoke_UpdateFunctionOfLoginManagerIsCalledTest()
        {
            var onUpdateCalled = 0;

            var middleware = new LoginMiddleware(new TestLoginManager((req, res) => onUpdateCalled++));
            var uri = new Uri("https://proxer.me/api/v1/login/test");
            IRequestBuilder builder = _apiRequestBuilder.FromUrl(uri);

            MiddlewareAction action = (request, token) => Task.FromResult((IProxerResult) new ProxerResult());

            IProxerResult result = await middleware.Invoke(builder, action).ConfigureAwait(false);
            Assert.True(result.Success);
            Assert.AreEqual(1, onUpdateCalled);
        }

        [Test]
        public async Task InvokeWithResult_AddsAuthenticationInformationFromLoginManagerTest()
        {
            var middleware = new LoginMiddleware(new TestLoginManager());
            var uri = new Uri("https://proxer.me/api/v1/login/test?addLogin=1");
            IRequestBuilderWithResult<object> builder = _apiRequestBuilder.FromUrl(uri).WithResult<object>();

            MiddlewareAction<object> action = (request, token) =>
            {
                Assert.IsTrue(request.Headers.ContainsKey(TestLoginManager.LOGIN_HEADER_KEY));
                Assert.AreEqual(TestLoginManager.LOGIN_HEADER_VALUE,
                    request.Headers[TestLoginManager.LOGIN_HEADER_KEY]);
                return Task.FromResult((IProxerResult<object>) new ProxerResult<object>(new object()));
            };

            IProxerResult<object> result = await middleware.InvokeWithResult(builder, action).ConfigureAwait(false);
            Assert.True(result.Success);
        }

        [Test]
        public async Task InvokeWithResult_DoNotRetryIfDifferentExceptionTest()
        {
            var middleware = new LoginMiddleware(new TestLoginManager());
            var uri = new Uri("https://proxer.me/api/v1/login/test?addLogin=0");
            IRequestBuilderWithResult<object> builder = _apiRequestBuilder.FromUrl(uri).WithResult<object>();

            var actionCalled = 0;

            MiddlewareAction<object> action = (request, token) =>
            {
                actionCalled++;
                if (actionCalled == 1)
                    return Task.FromResult((IProxerResult<object>) new ProxerResult<object>(new Exception()));
                return Task.FromResult((IProxerResult<object>) new ProxerResult<object>(new object()));
            };

            IProxerResult<object> result = await middleware.InvokeWithResult(builder, action).ConfigureAwait(false);
            Assert.False(result.Success);
            Assert.AreEqual(1, actionCalled);
        }

        [Test]
        public async Task InvokeWithResult_DoNotRetryIfLoginManagerAlreadyAddedInformationTest()
        {
            var middleware = new LoginMiddleware(new TestLoginManager());
            var uri = new Uri("https://proxer.me/api/v1/login/test?addLogin=1");
            IRequestBuilderWithResult<object> builder = _apiRequestBuilder.FromUrl(uri).WithResult<object>();

            var actionCalled = 0;

            MiddlewareAction<object> action = (request, token) =>
            {
                actionCalled++;
                if (actionCalled == 1)
                    return Task.FromResult(
                        (IProxerResult<object>) new ProxerResult<object>(new NotAuthenticatedException()));
                return Task.FromResult((IProxerResult<object>) new ProxerResult<object>(new object()));
            };

            IProxerResult<object> result = await middleware.InvokeWithResult(builder, action).ConfigureAwait(false);
            Assert.False(result.Success);
            Assert.AreEqual(1, actionCalled);
        }

        [Test]
        public async Task InvokeWithResult_RetriesIfNotAuthenticatedTest()
        {
            var middleware = new LoginMiddleware(new TestLoginManager());
            var uri = new Uri("https://proxer.me/api/v1/login/test?addLogin=0");
            IRequestBuilderWithResult<object> builder = _apiRequestBuilder.FromUrl(uri).WithResult<object>();

            var actionCalled = 0;

            MiddlewareAction<object> action = (request, token) =>
            {
                actionCalled++;
                if (actionCalled == 1)
                    return Task.FromResult(
                        (IProxerResult<object>) new ProxerResult<object>(new NotAuthenticatedException()));
                return Task.FromResult((IProxerResult<object>) new ProxerResult<object>(new object()));
            };

            IProxerResult<object> result = await middleware.InvokeWithResult(builder, action).ConfigureAwait(false);
            Assert.True(result.Success);
            Assert.AreEqual(2, actionCalled);
        }

        [Test]
        public async Task InvokeWithResult_UpdateFunctionOfLoginManagerIsCalledTest()
        {
            var onUpdateCalled = 0;

            var middleware = new LoginMiddleware(new TestLoginManager((req, res) => onUpdateCalled++));
            var uri = new Uri("https://proxer.me/api/v1/login/test");
            IRequestBuilderWithResult<object> builder = _apiRequestBuilder.FromUrl(uri).WithResult<object>();

            MiddlewareAction<object> action = (request, token) =>
                Task.FromResult((IProxerResult<object>) new ProxerResult<object>(new object()));

            IProxerResult<object> result = await middleware.InvokeWithResult(builder, action).ConfigureAwait(false);
            Assert.True(result.Success);
            Assert.AreEqual(1, onUpdateCalled);
        }
    }
}