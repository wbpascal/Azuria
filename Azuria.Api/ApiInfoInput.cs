using System;
using Azuria.Api.Connection;

namespace Azuria.Api
{
    /// <summary>
    /// </summary>
    public class ApiInfoInput
    {
        #region Properties

        /// <summary>
        /// </summary>
        public char[] ApiKeyV1 { get; set; }

        /// <summary>
        /// </summary>
        public Func<IProxerUser, IHttpClient> HttpClientFactory { get; set; } = user => new HttpClient(user);

        #endregion
    }
}