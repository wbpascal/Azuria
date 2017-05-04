using System.Threading;
using System.Threading.Tasks;
using Azuria.Api.Builder;
using Azuria.Api.v1;
using Azuria.ErrorHandling;

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
        /// <param name="token"></param>
        public static Task<IProxerResult> DoRequestAsync(
            this IUrlBuilder builder, CancellationToken token = new CancellationToken())
        {
            return builder.ApiRequestAsync(token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Task<IProxerResult<T>> DoRequestAsync<T>(
            this IUrlBuilderWithResult<T> builder, CancellationToken token = new CancellationToken())
        {
            return builder.ApiRequestAsync(token);
        }

        #endregion
    }
}