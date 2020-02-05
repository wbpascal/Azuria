using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azuria.Enums;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Requests.Builder;

namespace Azuria.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorMiddleware : IMiddleware
    {
        /// <inheritdoc />
        public async Task<IProxerResult> Invoke(IRequestBuilder request, MiddlewareAction next,
            CancellationToken cancellationToken = default)
        {
            Exception[] lRequestExceptions = GetRequestExceptions(request);
            if (lRequestExceptions.Any()) return new ProxerResult(lRequestExceptions);

            IProxerResult lResult = await next(request, cancellationToken).ConfigureAwait(false);

            Exception[] lResultExceptions = GetResultExceptions(lResult);
            return lResultExceptions.Any() ? new ProxerResult(lResultExceptions) : lResult;
        }

        /// <inheritdoc />
        public async Task<IProxerResult<T>> InvokeWithResult<T>(IRequestBuilderWithResult<T> request,
            MiddlewareAction<T> next, CancellationToken cancellationToken = default)
        {
            Exception[] lRequestExceptions = GetRequestExceptions(request);
            if (lRequestExceptions.Any()) return new ProxerResult<T>(lRequestExceptions);

            IProxerResult<T> lResult = await next(request, cancellationToken).ConfigureAwait(false);

            Exception[] lResultExceptions = GetResultExceptions(lResult);
            return lResultExceptions.Any() ? new ProxerResult<T>(lResultExceptions) : lResult;
        }

        private static Exception[] GetRequestExceptions(IRequestBuilderBase request)
        {
            if (!IsApiUrl(request.BuildUri()))
                return new Exception[] {new InvalidRequestException("The given request was not a valid api url!")};

            return new Exception[0];
        }

        private static Exception[] GetResultExceptions(IProxerResultBase result)
        {
            if (result.Success) return new Exception[0];

            switch (result)
            {
                case ProxerApiResponseBase lResponse:
                    Exception lResponseException = GetResponseException(lResponse.ErrorCode);
                    return lResponseException == null ? new Exception[0] : new[] {lResponseException};
                default:
                    return new Exception[0];
            }
        }

        private static bool IsApiUrl(Uri url)
        {
            return url.Host.Equals("proxer.me") && url.AbsolutePath.StartsWith("/api/");
        }

        private static Exception GetResponseException(ErrorCode code)
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
                case ErrorCode.NoError:
                    return null;
                default:
                    return new ProxerApiException(code);
            }
        }
    }
}