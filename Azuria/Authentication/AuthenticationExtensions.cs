using System.Threading;
using System.Threading.Tasks;
using Autofac;
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
        /// <param name="username">The username of the user that is being authenticated.</param>
        /// <param name="password">The password of the user that is being authenticated.</param>
        /// <param name="secretKey">Optional. The 2FA-Token used to authenticate the client.</param>
        /// <param name="token">Optional. The cancellation token used for cancelling the request.</param>
        /// <returns>A <see cref="Task"/> that contains the result of the request.</returns>
        public static Task<IProxerResult> LoginAsync(
            this IProxerClient client, string username, string password, string secretKey = null,
            CancellationToken token = default(CancellationToken))
        {
            return client.Container.Resolve<ILoginManager>().PerformLogin(username, password, secretKey, token);
        }

        /// <summary>
        /// Performs the logout of a <see cref="IProxerClient">client</see>.
        /// </summary>
        /// <param name="client">The client on which the logout is performed.</param>
        /// <param name="token">Optional. The cancellation token used for cancelling the request.</param>
        /// <returns>A <see cref="Task"/> that contains the result of the method.</returns>
        public static Task<IProxerResult> LogoutAsync(
            this IProxerClient client, CancellationToken token = default(CancellationToken))
        {
            return client.Container.Resolve<ILoginManager>().PerformLogout(token);
        }
    }
}