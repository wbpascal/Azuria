using System.Threading;
using System.Threading.Tasks;
using Azuria.Api.Builder;
using Azuria.ErrorHandling;
using Azuria.Requests.Builder;

namespace Azuria.Requests
{
    /// <summary>
    ///
    /// </summary>
    public interface IRequestHandler
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<IProxerResult<T>> ApiRequestAsync<T>(IRequestBuilderWithResult<T> request, CancellationToken token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IProxerResult> ApiRequestAsync(IRequestBuilder request, CancellationToken token);
    }
}