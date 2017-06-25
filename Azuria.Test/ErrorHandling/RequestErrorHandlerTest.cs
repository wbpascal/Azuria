using System;
using Autofac;
using Azuria.Enums;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Xunit;

namespace Azuria.Test.ErrorHandling
{
    public class RequestErrorHandlerTest
    {
        private readonly IRequestErrorHandler _requestErrorHandler;

        public RequestErrorHandlerTest()
        {
            IProxerClient lClient = ProxerClient.Create(new char[32]);
            this._requestErrorHandler = lClient.Container.Resolve<IRequestErrorHandler>();
        }

        [Theory]
        [InlineData(ErrorCode.ApiKeyNoPermission)]
        public void HandleApiKeyInsufficientErrorTest(ErrorCode errorCode)
        {
            Assert.IsType<ApiKeyInsufficientException>(this._requestErrorHandler.HandleError(errorCode));
        }

        [Theory]
        [InlineData(ErrorCode.IpBlocked)]
        public void HandleIpBlockedErrorTest(ErrorCode errorCode)
        {
            Exception lException = this._requestErrorHandler.HandleError(errorCode);
            Assert.IsType<FirewallException>(lException);
            Assert.Equal("http://proxer.me/misc/captcha", lException.Message);
        }

        [Theory]
        [InlineData(ErrorCode.UserNoPermission)]
        [InlineData(ErrorCode.ChatNoPermission)]
        public void HandleNoPermissionErrorTest(ErrorCode errorCode)
        {
            Assert.IsType<NoPermissionException>(this._requestErrorHandler.HandleError(errorCode));
        }

        [Theory]
        [InlineData(ErrorCode.NotificationsNotLoggedIn)]
        [InlineData(ErrorCode.UcpNotLoggedIn)]
        [InlineData(ErrorCode.InfoNotLoggedIn)]
        [InlineData(ErrorCode.MessengerNotLoggedIn)]
        [InlineData(ErrorCode.ChatNotLoggedIn)]
        public void HandleNotAuthenticatedErrorTest(ErrorCode errorCode)
        {
            Assert.IsType<NotAuthenticatedException>(this._requestErrorHandler.HandleError(errorCode));
        }
    }
}