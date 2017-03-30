using System.Threading.Tasks;
using Azuria.Api.Builder;
using Azuria.Api.v1;

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
        /// <returns></returns>
        public static IApiRequestBuilder CreateRequest(this IProxerClient client)
        {
            return new ApiRequestBuilder(client);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static Task<ProxerApiResponse> DoRequestAsync(this IUrlBuilder builder)
        {
            return builder.ApiRequestAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static Task<ProxerApiResponse<T>> DoRequestAsync<T>(this IUrlBuilderWithResult<T> builder)
        {
            return builder.ApiRequestAsync();
        }

        #endregion
    }
}