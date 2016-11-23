using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Azuria.ErrorHandling;

namespace Azuria.Web
{
    /// <summary>
    /// </summary>
    public abstract class BaseHttpClient : IHttpClient
    {
#if PORTABLE
/// <summary>
/// 
/// </summary>
        protected static readonly string UserAgent = "Azuria.Portable/" + 
                                                   typeof(HttpClient).GetTypeInfo().Assembly.GetName().Version;
#else
        /// <summary>
        /// </summary>
        protected static readonly string UserAgent = "Azuria/" +
                                                     typeof(HttpClient).GetTypeInfo().Assembly.GetName().Version;
#endif

        /// <inheritdoc />
        public abstract void Dispose();

        /// <inheritdoc />
        public abstract Task<IProxerResult<string>> GetRequest(Uri url, Dictionary<string, string> headers = null);

        /// <inheritdoc />
        public abstract Task<IProxerResult<string>> PostRequest(Uri url,
            IEnumerable<KeyValuePair<string, string>> postArgs,
            Dictionary<string, string> headers = null);
    }
}