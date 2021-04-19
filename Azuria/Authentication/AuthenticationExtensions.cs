using System;
using System.Threading;
using System.Threading.Tasks;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.Input.User;
using Azuria.ErrorHandling;
using Azuria.Middleware;

namespace Azuria.Authentication
{
    /// <summary>
    /// Contains extensions methods to authenticate a client.
    /// </summary>
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Tries to find the first login manager found within the middleware pipeline of the given client.
        /// </summary>
        /// <param name="client"></param>
        /// <returns>An instance of a login manager or null.</returns>
        public static ILoginManager TryFindLoginManager(this IProxerClient client)
        {
            foreach (IMiddleware middleware in client.Pipeline.Middlewares)
                if (middleware is LoginMiddleware loginMiddleware)
                    return loginMiddleware.LoginManager;

            return null;
        }
    }
}