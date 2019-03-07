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
    public interface IHttpClient
    {
        /// <summary>
        /// Creates a "GET" request to a given url with optional header.
        /// </summary>
        /// <param name="url">The url the request is send to.</param>
        /// <param name="headers">Optional. The header that will be send alongside the request.</param>
        /// <param name="token">Optional. The cancellation token that can be used to cancel the request.</param>
        /// <returns>A <see cref="Task" /> that returns the result of the request.</returns>
        Task<IProxerResult<string>> GetRequestAsync(
            Uri url, IDictionary<string, string> headers = null, CancellationToken token = default);

        /// <summary>
        /// Creates a "POST" request to a given url with optional header.
        /// </summary>
        /// <param name="url">The url the request is send to.</param>
        /// <param name="postArgs">The post arguments that will be send alongside the request.</param>
        /// <param name="headers">Optional. The header that will be send alongside the request.</param>
        /// <param name="token">Optional. The cancellation token that can be used to cancel the request.</param>
        /// <returns>A <see cref="Task" /> that returns the result of the request.</returns>
        Task<IProxerResult<string>> PostRequestAsync(
            Uri url, IEnumerable<KeyValuePair<string, string>> postArgs,
            IDictionary<string, string> headers = null, CancellationToken token = default);
    }
}