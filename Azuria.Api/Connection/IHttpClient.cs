using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azuria.Api.ErrorHandling;

namespace Azuria.Api.Connection
{
    /// <summary>
    /// </summary>
    public interface IHttpClient : IDisposable
    {
        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        Task<IProxerResult<string>> GetRequest(Uri url, Dictionary<string, string> headers = null);

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postArgs"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        Task<IProxerResult<string>> PostRequest(Uri url, IEnumerable<KeyValuePair<string, string>> postArgs,
            Dictionary<string, string> headers = null);

        #endregion
    }
}