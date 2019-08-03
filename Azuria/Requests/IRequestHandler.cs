using System.Threading;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Requests.Builder;

namespace Azuria.Requests
{
    /// <summary>
    /// </summary>
    public interface IRequestHandler
    {
        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<IProxerResult<T>> MakeRequestAsync<T>(IRequestBuilderWithResult<T> request, CancellationToken token);

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IProxerResult> MakeRequestAsync(IRequestBuilder request, CancellationToken token);
    }
}