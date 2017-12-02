using System;
using Azuria.Enums;
using Azuria.Exceptions;

namespace Azuria.ErrorHandling
{
    /// <summary>
    /// </summary>
    public class RequestErrorHandler : IRequestErrorHandler
    {
        /// <inheritdoc />
        public Exception HandleError(ErrorCode code)
        {
            switch (code)
            {
                case ErrorCode.IpBlocked:
                    return new FirewallException("http://proxer.me/misc/captcha");
                case ErrorCode.ApiKeyNoPermission:
                    return new ApiKeyInsufficientException();
                case ErrorCode.UserNoPermission:
                case ErrorCode.ChatNoPermission:
                    return new NoPermissionException();
                case ErrorCode.NotificationsNotLoggedIn:
                case ErrorCode.UcpNotLoggedIn:
                case ErrorCode.InfoNotLoggedIn:
                case ErrorCode.MessengerNotLoggedIn:
                case ErrorCode.ChatNotLoggedIn:
                    return new NotAuthenticatedException();
            }

            return null;
        }
    }
}