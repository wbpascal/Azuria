using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azuria.Api.Builder;
using Azuria.ErrorHandling;
using Azuria.Requests.Builder;
using Azuria.Requests.Http;

namespace Azuria.Requests
{
    /// <summary>
    /// </summary>
    public static class RequestExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static IApiRequestBuilder CreateRequest(this IProxerClient client)
        {
            return new ApiRequestBuilder(client);
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="token"></param>
        public static Task<IProxerResult> DoRequestAsync(
            this IRequestBuilder builder, CancellationToken token = new CancellationToken())
        {
            return builder.Client.Pipeline.BuildPipeline()(builder, CancellationToken.None);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Task<IProxerResult<T>> DoRequestAsync<T>(
            this IRequestBuilderWithResult<T> builder, CancellationToken token = new CancellationToken())
        {
            return builder.Client.Pipeline.BuildPipelineWithResult<T>()(builder, CancellationToken.None);
        }

        internal static Task<IProxerResult<string>> ProxerRequestAsync(
            this IHttpClient httpClient, Uri url, IEnumerable<KeyValuePair<string, string>> postArgs,
            IDictionary<string, string> headers, CancellationToken token)
        {
            KeyValuePair<string, string>[] lPostArgs =
                postArgs as KeyValuePair<string, string>[] ?? postArgs.ToArray();
            return lPostArgs.Any()
                ? httpClient.PostRequestAsync(url, lPostArgs, headers, token)
                : httpClient.GetRequestAsync(url, headers, token);
        }
    }
}