using System;
using Autofac;
using Azuria.Enums;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using NUnit.Framework;

namespace Azuria.Test.ErrorHandling
{
    [TestFixture]
    public class RequestErrorHandlerTest
    {
        private readonly IRequestErrorHandler _requestErrorHandler;

        public RequestErrorHandlerTest()
        {
            IProxerClient lClient = ProxerClient.Create(new char[32]);
            this._requestErrorHandler = lClient.Container.Resolve<IRequestErrorHandler>();
        }

        [Test]
        [TestCase(ErrorCode.ApiKeyNoPermission)]
        public void HandleApiKeyInsufficientErrorTest(ErrorCode errorCode)
        {
            Assert.IsAssignableFrom<ApiKeyInsufficientException>(this._requestErrorHandler.HandleError(errorCode));
        }

        [Test]
        [TestCase(ErrorCode.IpBlocked)]
        public void HandleIpBlockedErrorTest(ErrorCode errorCode)
        {
            Exception lException = this._requestErrorHandler.HandleError(errorCode);
            Assert.IsAssignableFrom<FirewallException>(lException);
            Assert.AreEqual("http://proxer.me/misc/captcha", lException.Message);
        }

        [Test]
        [TestCase(ErrorCode.UserNoPermission)]
        [TestCase(ErrorCode.ChatNoPermission)]
        public void HandleNoPermissionErrorTest(ErrorCode errorCode)
        {
            Assert.IsAssignableFrom<NoPermissionException>(this._requestErrorHandler.HandleError(errorCode));
        }

        [Test]
        [TestCase(ErrorCode.NotificationsNotLoggedIn)]
        [TestCase(ErrorCode.UcpNotLoggedIn)]
        [TestCase(ErrorCode.InfoNotLoggedIn)]
        [TestCase(ErrorCode.MessengerNotLoggedIn)]
        [TestCase(ErrorCode.ChatNotLoggedIn)]
        public void HandleNotAuthenticatedErrorTest(ErrorCode errorCode)
        {
            Assert.IsAssignableFrom<NotAuthenticatedException>(this._requestErrorHandler.HandleError(errorCode));
        }
    }
}