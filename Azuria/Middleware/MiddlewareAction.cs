using System.Threading;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Requests.Builder;

namespace Azuria.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    public delegate Task<IProxerResult> MiddlewareAction(IRequestBuilder request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <typeparam name="T"></typeparam>
    public delegate Task<IProxerResult<T>> MiddlewareAction<T>(IRequestBuilderWithResult<T> request,
        CancellationToken cancellationToken = default);
}