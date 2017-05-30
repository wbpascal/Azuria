using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azuria.ErrorHandling;

namespace Azuria.Requests.Http
{
    /// <summary>
    /// An interface that is used to send http/s requests.
    /// </summary>
    public interface IHttpClient : IDisposable
    {
        /// <summary>
        /// Creates a "GET" request to a given url with optional header.
        /// </summary>
        /// <param name="url">The url the request is send to.</param>
        /// <param name="header">Optional. The header that will be send alongside the request.</param>
        /// <param name="token">Optional. The cancellation token that can be used to cancel the request.</param>
        /// <returns>A <see cref="Task"/> that returns the result of the request.</returns>
        Task<IProxerResult<string>> GetRequestAsync(
            Uri url, Dictionary<string, string> header = null, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Creates a "POST" request to a given url with optional header.
        /// </summary>
        /// <param name="url">The url the request is send to.</param>
        /// <param name="postArgs">The post arguments that will be send alongside the request.</param>
        /// <param name="header">Optional. The header that will be send alongside the request.</param>
        /// <param name="token">Optional. The cancellation token that can be used to cancel the request.</param>
        /// <returns>A <see cref="Task"/> that returns the result of the request.</returns>
        Task<IProxerResult<string>> PostRequestAsync(
            Uri url, IEnumerable<KeyValuePair<string, string>> postArgs,
            Dictionary<string, string> header = null, CancellationToken token = default(CancellationToken));
    }
}