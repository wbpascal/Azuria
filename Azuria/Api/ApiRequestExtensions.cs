using System.Threading;
using System.Threading.Tasks;
using Autofac;
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
            return client.Container.Resolve<IApiRequestBuilder>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="token"></param>
        public static Task<IProxerResult> DoRequestAsync(this IUrlBuilder builder, CancellationToken token)
        {
            return builder.ApiRequestAsync(token);
        }

        /// <inheritdoc cref="DoRequestAsync(IUrlBuilder,CancellationToken)" />
        public static Task<IProxerResult> DoRequestAsync(this IUrlBuilder builder)
        {
            return builder.DoRequestAsync(new CancellationToken());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Task<IProxerResult<T>> DoRequestAsync<T>(
            this IUrlBuilderWithResult<T> builder, CancellationToken token)
        {
            return builder.ApiRequestAsync(token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static Task<IProxerResult<T>> DoRequestAsync<T>(this IUrlBuilderWithResult<T> builder)
        {
            return builder.DoRequestAsync(new CancellationToken());
        }

        #endregion
    }
}