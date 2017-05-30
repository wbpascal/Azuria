using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Azuria.Api.Builder;
using Azuria.Authentication;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Requests.Builder;
using Azuria.Requests.Http;
using Azuria.Serialization;
using Newtonsoft.Json;

namespace Azuria.Requests
{
    internal class RequestHandler : IRequestHandler
    {
        private readonly IRequestHeaderManager _headerManager;
        private readonly ILoginManager _loginManager;
        private readonly IRequestErrorHandler _errorHandler;
        private readonly IJsonDeserializer _jsonDeserializer;

        public RequestHandler(
            IRequestHeaderManager headerManager, ILoginManager loginManager, IRequestErrorHandler errorHandler,
            IJsonDeserializer jsonDeserializer)
        {
            this._headerManager = headerManager;
            this._loginManager = loginManager;
            this._errorHandler = errorHandler;
            this._jsonDeserializer = jsonDeserializer;
        }

        /// <inheritdoc />
        public async Task<IProxerResult> MakeRequestAsync(IRequestBuilder request, CancellationToken token)
        {
            if (!IsApiUrl(request.BuildUri()))
                return new ProxerResult(new InvalidRequestException("The given request was not a valid api url!"));
            
            IProxerResult lResult = await this.ApiRequestInternalAsync<ProxerApiResponse>(request, token)
                                        .ConfigureAwait(false);

            return lResult.Success && lResult is ProxerApiResponse
                       ? lResult as ProxerApiResponse
                       : new ProxerResult(lResult.Exceptions);
        }

        /// <inheritdoc />
        public async Task<IProxerResult<T>> MakeRequestAsync<T>(
            IRequestBuilderWithResult<T> request, CancellationToken token)
        {
            if (!IsApiUrl(request.BuildUri()))
                return new ProxerResult<T>(new InvalidRequestException("The given request was not a valid api url!"));
            
            JsonSerializerSettings lSerializerSettings =
                new JsonSerializerSettings() {Converters = GetCustomConverter(request)};

            IProxerResult lResult =
                await this.ApiRequestInternalAsync<ProxerApiResponse<T>>(request, token, lSerializerSettings)
                    .ConfigureAwait(false);

            return lResult.Success && lResult is ProxerApiResponse<T>
                       ? lResult as ProxerApiResponse<T>
                       : (IProxerResult<T>) new ProxerResult<T>(lResult.Exceptions);
        }

        private static bool IsApiUrl(Uri url)
        {
            return url.Host.Equals("proxer.me") && url.AbsolutePath.StartsWith("/api/");
        }

        private static IList<JsonConverter> GetCustomConverter<T>(IRequestBuilderWithResult<T> request)
        {
            return request.CustomDataConverter == null
                       ? new JsonConverter[0]
                       : new JsonConverter[] {request.CustomDataConverter};
        }

        private async Task<IProxerResult> ApiRequestInternalAsync<T>(
            IRequestBuilderBase request, CancellationToken token, JsonSerializerSettings settings = null)
            where T : ProxerApiResponse
        {
            Dictionary<string, string> lHeaders = this._headerManager.GetHeader();

            if (request.CheckLogin && this._loginManager.CheckIsLoginProbablyValid() &&
                !this._headerManager.ContainsAuthenticationHeaders(lHeaders))
                return new ProxerResult(
                    new NotAuthenticatedException("The client must be authenticated to make this request!")
                );

            IProxerResult<string> lResult =
                await request.Client.Container.Resolve<IHttpClient>()
                    .ProxerRequestAsync(
                        request.BuildUri(), request.PostArguments, lHeaders, token
                    )
                    .ConfigureAwait(false);
            if (!lResult.Success || string.IsNullOrEmpty(lResult.Result))
                return new ProxerResult(
                    lResult.Exceptions.Any()
                        ? lResult.Exceptions
                        : new[] {new SerializationException("Cannot serialize empty response!")}
                );

            IProxerResult<T> lSerializationResult = await this._jsonDeserializer
                                                        .Deserialize<T>(lResult.Result, settings, token)
                                                        .ConfigureAwait(false);
            if(!lSerializationResult.Success) return new ProxerResult(lSerializationResult.Exceptions);

            T lApiResponse = lSerializationResult.Result;
            if (lApiResponse.Success) return lApiResponse;

            Exception lException = this._errorHandler.HandleError(lApiResponse.ErrorCode);
            if (!(lException is NotAuthenticatedException) || this._headerManager.ContainsAuthenticationHeaders(lHeaders))
                return lException == null
                           ? new ProxerResult(new ProxerApiException(lApiResponse.ErrorCode))
                           : new ProxerResult(lException);

            this._loginManager.QueueLoginForNextRequest();
            return await this.ApiRequestInternalAsync<T>(request, token, settings).ConfigureAwait(false);
        }
    }
}