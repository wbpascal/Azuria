using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azuria.Authentication;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Requests.Builder;

namespace Azuria.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginMiddleware : IMiddleware
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginManager"></param>
        public LoginMiddleware(ILoginManager loginManager)
        {
            this.LoginManager = loginManager ?? throw new ArgumentNullException();
        }

        public ILoginManager LoginManager { get; }

        /// <inheritdoc />
        public async Task<IProxerResult> Invoke(IRequestBuilder request, MiddlewareAction next,
            CancellationToken cancellationToken = default)
        {
            this.LoginManager.AddAuthenticationInformation(request);
            IProxerResult lResult = await next(request, cancellationToken).ConfigureAwait(false);

            //Check if we should retry the next middleware
            if (ShouldRetry(request, lResult))
                lResult = await next(request, cancellationToken).ConfigureAwait(false);

            // Update the state of the login manager
            this.LoginManager.Update(request, lResult);

            return lResult;
        }

        /// <inheritdoc />
        public async Task<IProxerResult<T>> InvokeWithResult<T>(IRequestBuilderWithResult<T> request,
            MiddlewareAction<T> next, CancellationToken cancellationToken = default)
        {
            this.LoginManager.AddAuthenticationInformation(request);
            IProxerResult<T> lResult = await next(request, cancellationToken).ConfigureAwait(false);

            //Check if we should retry the next middleware
            if (ShouldRetry(request, lResult))
                lResult = await next(request, cancellationToken).ConfigureAwait(false);

            // Update the state of the login manager
            this.LoginManager.Update(request, lResult);

            return lResult;
        }

        private bool ShouldRetry(IRequestBuilderBase request, IProxerResultBase result)
        {
            // Check if the request failed because the client was not authenticated
            // Also check if we already added the auth information before
            if (result.Exceptions.Any(ex => ex.GetType() == typeof(NotAuthenticatedException)) ||
                this.LoginManager.ContainsAuthenticationInformation(request)) return false;

            // Force login on next request
            this.LoginManager.InvalidateLogin();
            // Only try the request again if we can actually add any authentication information to it
            return this.LoginManager.AddAuthenticationInformation(request);
        }
    }
}