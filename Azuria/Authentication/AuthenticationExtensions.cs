using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Azuria.ErrorHandling;

namespace Azuria.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public static class AuthenticationExtensions
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="token"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public static Task<IProxerResult> LoginAsync(
            this IProxerClient client, string username, string password, string secretKey = null,
            CancellationToken token = new CancellationToken())
        {
            return client.Container.Resolve<ILoginManager>().PerformLogin(username, password, secretKey, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Task<IProxerResult> LogoutAsync(
            this IProxerClient client,
            CancellationToken token = new CancellationToken())
        {
            return client.Container.Resolve<ILoginManager>().PerformLogout(token);
        }

        #endregion
    }
}