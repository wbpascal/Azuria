using System;
using System.Threading;
using System.Threading.Tasks;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.Input.User;
using Azuria.ErrorHandling;
using Azuria.Middleware;
using Azuria.Middleware.Pipeline;

namespace Azuria.Authentication
{
    /// <summary>
    /// Contains extensions methods to authenticate a client.
    /// </summary>
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Authenticates a <see cref="IProxerClient">client</see> with a username, password and an optional 2FA-Token.
        /// TODO: Rename to TryLoginAsync?
        /// </summary>
        /// <param name="client">The client that is authenticated.</param>
        /// <param name="input">The input model that contains the login data.</param>
        /// <param name="token">Optional. The cancellation token used for cancelling the request.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if the pipeline of the given client does not contain a login middleware with a DefaultLoginManager.
        /// </exception>
        /// <returns>A <see cref="Task" /> that contains the result of the request.</returns>
        public static Task<IProxerResult<LoginDataModel>> LoginAsync(
            this IProxerClient client, LoginInput input, CancellationToken token = default)
        {
            DefaultLoginManager loginManager = TryFindLoginManager(client.Pipeline);
            if (loginManager == null)
                throw new ArgumentException(
                    "The pipeline of the client does not contain a login middleware with a DefaultLoginManager",
                    nameof(client)
                );

            return loginManager.PerformLoginAsync(input, token);
        }

        /// <summary>
        /// Performs the logout of a <see cref="IProxerClient">client</see>.
        /// </summary>
        /// <param name="client">The client on which the logout is performed.</param>
        /// <param name="token">Optional. The cancellation token used for cancelling the request.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if the pipeline of the given client does not contain a login middleware with a DefaultLoginManager.
        /// </exception>
        /// <returns>A <see cref="Task" /> that contains the result of the method.</returns>
        public static Task<IProxerResult> LogoutAsync(this IProxerClient client, CancellationToken token = default)
        {
            DefaultLoginManager loginManager = TryFindLoginManager(client.Pipeline);
            if (loginManager == null)
                throw new ArgumentException(
                    "The pipeline of the client does not contain a login middleware with a DefaultLoginManager",
                    nameof(client)
                );

            return loginManager.PerformLogoutAsync(token);
        }

        // Returns the first instance from the pipeline or null
        private static DefaultLoginManager TryFindLoginManager(IPipeline pipeline)
        {
            foreach (var middleware in pipeline.Middlewares)
            {
                if (middleware is LoginMiddleware loginMiddleware)
                {
                    if (loginMiddleware.LoginManager is DefaultLoginManager instance)
                    {
                        return instance;
                    }
                }
            }

            return null;
        }
    }
}