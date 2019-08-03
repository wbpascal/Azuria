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
        private readonly ILoginManager _loginManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginManager"></param>
        public LoginMiddleware(ILoginManager loginManager)
        {
            this._loginManager = loginManager ?? throw new ArgumentNullException();
        }

        /// <inheritdoc />
        public async Task<IProxerResult> Invoke(IRequestBuilder request,
            Func<IRequestBuilder, Task<IProxerResult>> next, CancellationToken cancellationToken = default)
        {
            this._loginManager.AddAuthenticationInformation(request);
            IProxerResult lResult = await next.Invoke(request).ConfigureAwait(false);

            //Check if we should retry the next middleware
            if (ShouldRetry(request, lResult))
                lResult = await next.Invoke(request).ConfigureAwait(false);

            // Update the state of the login manager
            this._loginManager.Update(lResult);

            return lResult;
        }

        /// <inheritdoc />
        public async Task<IProxerResult<T>> InvokeWithResult<T>(IRequestBuilderWithResult<T> request,
            Func<IRequestBuilderWithResult<T>, Task<IProxerResult<T>>> next,
            CancellationToken cancellationToken = default)
        {
            this._loginManager.AddAuthenticationInformation(request);
            IProxerResult<T> lResult = await next.Invoke(request).ConfigureAwait(false);

            //Check if we should retry the next middleware
            if (ShouldRetry(request, lResult))
                lResult = await next.Invoke(request).ConfigureAwait(false);

            // Update the state of the login manager
            this._loginManager.Update(lResult);

            return lResult;
        }

        private bool ShouldRetry(IRequestBuilderBase request, IProxerResultBase result)
        {
            // Check if the request failed because the client was not authenticated
            // Also check if we already added the auth information before
            if (result.Exceptions.All(ex => ex.GetType() != typeof(NotAuthenticatedException)) ||
                this._loginManager.ContainsAuthenticationInformation(request)) return false;

            // Force login on next request
            this._loginManager.InvalidateLogin();
            // Only try the request again if we can actually add any authentication information to it
            return this._loginManager.AddAuthenticationInformation(request);
        }
    }
}