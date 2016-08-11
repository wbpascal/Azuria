using System;
using Azuria.Api.v1;
using Azuria.Security;

namespace Azuria.Api
{
    /// <summary>
    /// </summary>
    public static class ApiInfo
    {
        internal static Func<ISecureContainer<char[]>> SecureContainerFactory;

        #region

        /// <summary>
        /// </summary>
        public static void InitV1(char[] apiKey, Func<ISecureContainer<char[]>> secureContainerFactory = null)
        {
            SecureContainerFactory = secureContainerFactory ?? (() => new SecureStringContainer());
            RequestHandler.Init(apiKey);
        }

        #endregion
    }
}