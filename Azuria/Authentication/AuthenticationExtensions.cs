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
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public static Task<IProxerResult> LoginAsync(
            this IProxerClient client, string username,
            string password, string secretKey = null)
        {
            return client.Container.Resolve<ILoginManager>().PerformLogin(username, password, secretKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static Task<IProxerResult> LogoutAsync(this IProxerClient client)
        {
            return client.Container.Resolve<ILoginManager>().PerformLogout();
        }

        #endregion
    }
}