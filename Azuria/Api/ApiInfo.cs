using System;
using Azuria.Api.v1;

namespace Azuria.Api
{
    /// <summary>
    /// </summary>
    public class ApiInfo
    {
        #region

        /// <summary>
        /// </summary>
        public static void InitV1(char[] apiKey, Type secureContainer = null)
        {
            RequestHandler.Init(apiKey, secureContainer);
        }

        #endregion
    }
}