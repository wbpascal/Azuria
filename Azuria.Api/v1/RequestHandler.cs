using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using Azuria.Api.Enums;
using Azuria.Api.Exceptions;
using Azuria.Core;
using Azuria.Core.Connection;
using Azuria.Core.ErrorHandling;
using Azuria.Core.Exceptions;
using Newtonsoft.Json;

namespace Azuria.Api.v1
{
    /// <summary>
    /// </summary>
    public static class RequestHandler
    {
        #region Methods

        internal static async Task<ProxerApiResponse<T>> ApiRequestAsync<T>(this IProxerClient client,
            ApiRequest<T> request)
        {
            IEnumerable<JsonConverter> lDataConverter = request.CustomDataConverter == null
                ? new JsonConverter[0]
                : new[] {request.CustomDataConverter};

            IProxerResult lResult = await client.ApiRequestInternalAsync<ProxerApiResponse<T>>(
                request, new JsonSerializerSettings {Converters = lDataConverter.ToList()}
            ).ConfigureAwait(false);

            return lResult.Success && lResult is ProxerApiResponse<T>
                ? lResult as ProxerApiResponse<T>
                : new ProxerApiResponse<T>(lResult.Exceptions);
        }

        internal static async Task<ProxerApiResponse> ApiRequestAsync(this IProxerClient client,
            ApiRequest request)
        {
            IProxerResult lResult = await client.ApiRequestInternalAsync<ProxerApiResponse>(request)
                .ConfigureAwait(false);

            return lResult.Success && lResult is ProxerApiResponse
                ? lResult as ProxerApiResponse
                : new ProxerApiResponse(lResult.Exceptions);
        }

        private static async Task<IProxerResult> ApiRequestInternalAsync<T>(this IProxerClient client,
            ApiRequest request, JsonSerializerSettings settings = null) where T : ProxerApiResponse
        {
            IProxerResult<string> lResult = await client.Container.Resolve<IHttpClient>().ProxerRequestAsync(
                request.FullAddress, request.PostArguments, GetHeaders(request, client.ApiKey)
            ).ConfigureAwait(false);
            if (!lResult.Success || string.IsNullOrEmpty(lResult.Result))
                return new ProxerResult(lResult.Exceptions);

            try
            {
                T lApiResponse = await Task<T>.Factory.StartNew(() =>
                    JsonConvert.DeserializeObject<T>(
                        WebUtility.HtmlDecode(lResult.Result), settings ?? new JsonSerializerSettings()
                    )
                ).ConfigureAwait(false);

                if (lApiResponse.Success) return lApiResponse;

                Exception lException = HandleErrorCode(lApiResponse.ErrorCode, request);
                if (lException == null)
                    return new ProxerResult(new ProxerApiException(lApiResponse.ErrorCode));

                return new ProxerResult(lException);
            }
            catch (Exception ex)
            {
                return new ProxerResult(ex);
            }
        }

        private static Dictionary<string, string> GetHeaders(ApiRequest request, char[] apiKey)
        {
            Dictionary<string, string> lHeaders = new Dictionary<string, string>
            {
                {"proxer-api-key", new string(apiKey)}
            };

            return lHeaders;
        }

        private static Exception HandleErrorCode(ErrorCode code, ApiRequest request)
        {
            switch (code)
            {
                case ErrorCode.IpBlocked:
                    return new CaptchaException("http://proxer.me/misc/captcha");
                case ErrorCode.ApiKeyNoPermission:
                    return new ApiKeyInsufficientException();
                case ErrorCode.UserNoPermission:
                case ErrorCode.ChatNoPermission:
                    return new NoAccessException();
                case ErrorCode.NotificationsNotLoggedIn:
                case ErrorCode.UcpNotLoggedIn:
                case ErrorCode.InfoNotLoggedIn:
                case ErrorCode.MessengerNotLoggedIn:
                case ErrorCode.ChatNotLoggedIn:
                    return new NotLoggedInException();
            }

            return null;
        }

        private static Task<IProxerResult<string>> ProxerRequestAsync(this IHttpClient httpClient, Uri url,
            IEnumerable<KeyValuePair<string, string>> postArgs, Dictionary<string, string> headers)
        {
            KeyValuePair<string, string>[] lPostArgs =
                postArgs as KeyValuePair<string, string>[] ?? postArgs.ToArray();
            return lPostArgs.Any()
                ? httpClient.PostRequestAsync(url, lPostArgs, headers)
                : httpClient.GetRequestAsync(url, headers);
        }

        #endregion
    }
}