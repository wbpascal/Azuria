using System;
using Azuria.Core.Connection;

namespace Azuria.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ProxerClientOptions
    {
        #region Properties

        internal IHttpClient HttpClient { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public ProxerClientOptions WithCustomHttpClient(IHttpClient client)
        {
            this.HttpClient = client ??
            throw new ArgumentNullException(nameof(client));
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="userAgentExtra"></param>
        /// <returns></returns>
        public ProxerClientOptions WithCustomHttpClient(int timeout = 5000, string userAgentExtra = "")
        {
            this.HttpClient = new HttpClient(timeout, userAgentExtra);
            return this;
        }

        #endregion
    }
}