using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azuria.ErrorHandling;

namespace Azuria.Connection
{
    /// <summary>
    /// </summary>
    public interface IHttpClient : IDisposable
    {
        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        Task<IProxerResult<string>> GetRequestAsync(
            Uri url, CancellationToken token, Dictionary<string, string> headers = null);

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postArgs"></param>
        /// <param name="token"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        Task<IProxerResult<string>> PostRequestAsync(
            Uri url, IEnumerable<KeyValuePair<string, string>> postArgs, CancellationToken token,
            Dictionary<string, string> headers = null);

        #endregion
    }
}