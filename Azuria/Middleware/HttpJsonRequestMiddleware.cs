using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Requests;
using Azuria.Requests.Builder;
using Azuria.Requests.Http;
using Azuria.Serialization;
using Newtonsoft.Json;

namespace Azuria.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpJsonRequestMiddleware : IMiddleware
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="deserializer"></param>
        public HttpJsonRequestMiddleware(IHttpClient httpClient, IJsonDeserializer deserializer)
        {
            this.HttpClient = httpClient;
            this.JsonDeserializer = deserializer;
        }

        /// <summary>
        /// 
        /// </summary>
        public IHttpClient HttpClient { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IJsonDeserializer JsonDeserializer { get; set; }

        /// <summary>
        /// </summary>
        /// <typeparam name="T">Either ProxerApiResponse{T} or ProxerApiResponse</typeparam>
        /// <returns></returns>
        private async Task<IProxerResultBase> ApiRequestInternalAsync<T>(
            IRequestBuilderBase request, CancellationToken token = default, JsonSerializerSettings settings = null)
            where T : ProxerApiResponseBase
        {
            IProxerResult<string> lResult =
                await this.HttpClient.ProxerRequestAsync(
                        request.BuildUri(), request.PostParameter, request.Headers, token
                    )
                    .ConfigureAwait(false);
            if (!lResult.Success || string.IsNullOrEmpty(lResult.Result))
                return new ProxerResult(
                    lResult.Exceptions.Any()
                        ? lResult.Exceptions
                        : new[] {new SerializationException("Cannot serialize empty response!")}
                );

            IProxerResult<T> lSerializationResult = this.JsonDeserializer.Deserialize<T>(lResult.Result, settings);
            if (!lSerializationResult.Success) return new ProxerResult(lSerializationResult.Exceptions);

            return lSerializationResult.Result;
        }

        private static IList<JsonConverter> GetCustomConverter<T>(IRequestBuilderWithResult<T> request)
        {
            return request.CustomDataConverter == null
                ? new JsonConverter[0]
                : new JsonConverter[] {request.CustomDataConverter};
        }

        /// <inheritdoc />
        public async Task<IProxerResult> Invoke(IRequestBuilder request, MiddlewareAction next,
            CancellationToken cancellationToken = default)
        {
            IProxerResultBase lResult =
                await this.ApiRequestInternalAsync<ProxerApiResponse>(request, cancellationToken)
                    .ConfigureAwait(false);

            return lResult.Success
                ? lResult as ProxerApiResponse
                : (IProxerResult) new ProxerResult(lResult.Exceptions);
        }

        /// <inheritdoc />
        public async Task<IProxerResult<T>> InvokeWithResult<T>(IRequestBuilderWithResult<T> request,
            MiddlewareAction<T> next, CancellationToken cancellationToken = default)
        {
            JsonSerializerSettings lSerializerSettings =
                new JsonSerializerSettings {Converters = GetCustomConverter(request)};

            IProxerResultBase lResult =
                await this.ApiRequestInternalAsync<ProxerApiResponse<T>>(request, cancellationToken, lSerializerSettings)
                    .ConfigureAwait(false);

            return lResult.Success
                ? lResult as ProxerApiResponse<T>
                : (IProxerResult<T>) new ProxerResult<T>(lResult.Exceptions);
        }
    }
}