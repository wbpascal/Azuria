using System;
using System.Threading;
using System.Threading.Tasks;
using Azuria.Api.v1.Input.User;
using Azuria.ErrorHandling;

namespace Azuria.Authentication
{
    /// <summary>
    /// An interface that is used to authenticate a <see cref="IProxerClient">client</see> and keep it authenticated.
    /// </summary>
    public interface ILoginManager
    {
        /// <summary>
        /// The login token that is used to keep the client authenticated.
        /// </summary>
        char[] LoginToken { get; set; }

        /// <summary>
        /// Gets if the client is probably logged in (based on <see cref="DateTime" /> so results
        /// may be inaccurate).
        /// </summary>
        /// <returns>A boolean that indicates if the client is probably logged in.</returns>
        bool CheckIsLoginProbablyValid();

        /// <summary>
        /// </summary>
        void PerformedRequest(bool sendLoginToken = false);

        /// <summary>
        /// Performs the login of the client with the given username, password and optional 2FA-Token.
        /// </summary>
        /// <param name="input">The input model that contains the login data.</param>
        /// <param name="token">Optional. The cancellation token used for cancelling the request.</param>
        /// <returns>A <see cref="Task" /> that returns the result of the request.</returns>
        Task<IProxerResult> PerformLoginAsync(
            LoginInput input, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Performs the logout of the client.
        /// </summary>
        /// <param name="token">Optional. The cancellation token used for cancelling the request.</param>
        /// <returns>A <see cref="Task" /> that returns the result of the request.</returns>
        Task<IProxerResult> PerformLogoutAsync(CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Queues the login token to be send with the next request.
        /// </summary>
        void QueueLoginForNextRequest();

        /// <summary>
        /// Gets wether or not the login token should be send with the current request.
        /// </summary>
        /// <returns>A boolean that indicates if the login token should be send with the next request.</returns>
        bool SendTokenWithNextRequest();
    }
}