using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Azuria.Api.v1.Input.User;
using Azuria.ErrorHandling;

namespace Azuria.Authentication
{
    /// <summary>
    /// Contains extensions methods to authenticate a client.
    /// </summary>
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Authenticates a <see cref="IProxerClient">client</see> with a username, password and an optional 2FA-Token.
        /// </summary>
        /// <param name="client">The client that is authenticated.</param>
        /// <param name="input">The input model that contains the login data.</param>
        /// <param name="token">Optional. The cancellation token used for cancelling the request.</param>
        /// <returns>A <see cref="Task" /> that contains the result of the request.</returns>
        public static Task<IProxerResult> LoginAsync(
            this IProxerClient client, LoginInput input, CancellationToken token = default(CancellationToken))
        {
            return client.Container.Resolve<ILoginManager>().PerformLoginAsync(input, token);
        }

        /// <summary>
        /// Performs the logout of a <see cref="IProxerClient">client</see>.
        /// </summary>
        /// <param name="client">The client on which the logout is performed.</param>
        /// <param name="token">Optional. The cancellation token used for cancelling the request.</param>
        /// <returns>A <see cref="Task" /> that contains the result of the method.</returns>
        public static Task<IProxerResult> LogoutAsync(
            this IProxerClient client, CancellationToken token = default(CancellationToken))
        {
            return client.Container.Resolve<ILoginManager>().PerformLogoutAsync(token);
        }
    }
}