using System.Threading;
using System.Threading.Tasks;
using Azuria.Api.Builder;
using Azuria.ErrorHandling;

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
        Task<IProxerResult<T>> ApiRequestAsync<T>(IUrlBuilderWithResult<T> request, CancellationToken token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IProxerResult> ApiRequestAsync(IUrlBuilder request, CancellationToken token);
    }
}