using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Core;

namespace Azuria.Api
{
    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestExtensions
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Task<ProxerApiResponse> MakeRawApiRequestAsync(this IProxerClient client,
            ApiRequest request)
        {
            return client.ApiRequestAsync(request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Task<ProxerApiResponse<T>> MakeRawApiRequestAsync<T>(this IProxerClient client,
            ApiRequest<T> request)
        {
            return client.ApiRequestAsync(request);
        }

        #endregion
    }
}