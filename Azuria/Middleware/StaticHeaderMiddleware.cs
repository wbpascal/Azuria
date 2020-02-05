using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Requests.Builder;

namespace Azuria.Middleware
{
    /// <summary>
    /// Adds some static headers to the request. They indicate things like application, version and api key used.
    /// </summary>
    public class StaticHeaderMiddleware : IMiddleware
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="staticHeaders"></param>
        public StaticHeaderMiddleware(IDictionary<string, string> staticHeaders)
        {
            this.Header = staticHeaders;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public IDictionary<string, string> Header { get; }

        /// <inheritdoc />
        public Task<IProxerResult> Invoke(IRequestBuilder request, MiddlewareAction next,
            CancellationToken cancellationToken = default)
        {
            return next(request.WithHeader(this.Header), cancellationToken);
        }

        /// <inheritdoc />
        public Task<IProxerResult<T>> InvokeWithResult<T>(IRequestBuilderWithResult<T> request,
            MiddlewareAction<T> next, CancellationToken cancellationToken = default)
        {
            return next(request.WithHeader(this.Header), cancellationToken);
        }
    }
}