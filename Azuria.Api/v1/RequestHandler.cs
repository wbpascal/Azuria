using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Azuria.Api.Connection;
using Azuria.Api.ErrorHandling;
using Azuria.Api.Exceptions;
using Azuria.Api.Services;
using Newtonsoft.Json;

namespace Azuria.Api.v1
{
    /// <summary>
    /// </summary>
    public static class RequestHandler
    {
        private static char[] _apiKey;

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="forceTokenLogin"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<ProxerApiResponse<T>> ApiRequestAsync<T>(this ApiRequest<T> request,
            bool forceTokenLogin = false)
        {
            IEnumerable<JsonConverter> lDataConverter = request.CustomDataConverter == null
                ? new JsonConverter[0]
                : new[] {request.CustomDataConverter};

            IProxerResult lResult = await ApiRequestInternalAsync<ProxerApiResponse<T>>(
                request, forceTokenLogin, new JsonSerializerSettings {Converters = lDataConverter.ToList()}
            ).ConfigureAwait(false);

            return lResult.Success && lResult is ProxerApiResponse<T>
                ? lResult as ProxerApiResponse<T>
                : new ProxerApiResponse<T>(lResult.Exceptions);
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="forceTokenLogin"></param>
        /// <returns></returns>
        public static async Task<ProxerApiResponse> ApiRequestAsync(this ApiRequest request, 
            bool forceTokenLogin = false)
        {
            IProxerResult lResult = await ApiRequestInternalAsync<ProxerApiResponse>(request, forceTokenLogin)
                .ConfigureAwait(false);

            return lResult.Success && lResult is ProxerApiResponse
                ? lResult as ProxerApiResponse
                : new ProxerApiResponse(lResult.Exceptions);
        }

        private static async Task<IProxerResult> ApiRequestInternalAsync<T>(ApiRequest request, 
            bool useLoginToken = false, JsonSerializerSettings settings = null) where T : ProxerApiResponse
        {
            if (request.CheckLogin && (request.User == null || !request.User.IsProbablyLoggedIn))
                return new ProxerResult(new[] {new NotLoggedInException(request.User)});

            IProxerResult<string> lResult = await HttpClientService.GetForUser(request.User).ProxerRequestAsync(
                request.FullAddress, request.PostArguments, GetHeaders(request, useLoginToken)
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
                if (lException == null) return new ProxerResult(new ProxerApiException(lApiResponse.ErrorCode));
                if (lException is NotLoggedInException && !useLoginToken)
                    return await ApiRequestInternalAsync<T>(request, true, settings).ConfigureAwait(false);

                return new ProxerResult(lException);
            }
            catch (Exception ex)
            {
                return new ProxerResult(ex);
            }
        }

        private static Dictionary<string, string> GetHeaders(ApiRequest request, bool forceTokenLogin)
        {
            Dictionary<string, string> lHeaders = new Dictionary<string, string>
            {
                {"proxer-api-key", new string(_apiKey)}
            };
            if (request.User == null) return lHeaders;
            if (forceTokenLogin || request.CheckLogin && !request.User.IsProbablyLoggedIn)
                lHeaders.Add("proxer-api-token", new string(request.User.LoginToken));

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
                    return new NoAccessException(request.User);
                case ErrorCode.NotificationsNotLoggedIn:
                case ErrorCode.UcpNotLoggedIn:
                case ErrorCode.InfoNotLoggedIn:
                case ErrorCode.MessengerNotLoggedIn:
                case ErrorCode.ChatNotLoggedIn:
                    return new NotLoggedInException(request.User);
            }

            return null;
        }

        internal static void Init(char[] apiKey)
        {
            _apiKey = apiKey;
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