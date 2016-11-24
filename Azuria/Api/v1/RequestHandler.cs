using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Security;
using Newtonsoft.Json;

namespace Azuria.Api.v1
{
    /// <summary>
    /// </summary>
    public static class RequestHandler
    {
        private static ISecureContainer<char[]> _apiKey;

        #region Methods

        internal static async Task<ProxerApiResponse<T>> ApiRequest<T>(ApiRequest<T> request,
            bool forceTokenLogin = false)
        {
            IEnumerable<JsonConverter> lDataConverter = request.CustomDataConverter == null
                ? new JsonConverter[0]
                : new[] {request.CustomDataConverter};

            IProxerResult lResult = await ApiRequestInternal<ProxerApiResponse<T>>(
                    request, forceTokenLogin, new JsonSerializerSettings {Converters = lDataConverter.ToList()})
                .ConfigureAwait(false);

            return lResult.Success && lResult is ProxerApiResponse<T>
                ? lResult as ProxerApiResponse<T>
                : new ProxerApiResponse<T>(lResult.Exceptions);
        }

        internal static async Task<ProxerApiResponse> ApiRequest(ApiRequest request, bool forceTokenLogin = false)
        {
            IProxerResult lResult =
                await ApiRequestInternal<ProxerApiResponse>(request, forceTokenLogin).ConfigureAwait(false);

            return lResult.Success && lResult is ProxerApiResponse
                ? lResult as ProxerApiResponse
                : new ProxerApiResponse(lResult.Exceptions);
        }

        private static async Task<IProxerResult> ApiRequestInternal<T>(ApiRequest request, bool forceTokenLogin = false,
            JsonSerializerSettings settings = null, int loopCount = 0) where T : ProxerApiResponse
        {
            if (request.CheckLogin && ((request.Senpai == null) || !request.Senpai.IsProbablyLoggedIn))
                return new ProxerResult(new[] {new NotLoggedInException(request.Senpai)});

            IProxerResult<string> lResult =
                await (request.Senpai?.HttpClient ?? ApiInfo.HttpClient).PostRequest(request.Address,
                    request.PostArguments, GetHeaders(request, forceTokenLogin)).ConfigureAwait(false);
            if (!lResult.Success || string.IsNullOrEmpty(lResult.Result))
                return new ProxerResult(lResult.Exceptions);

            try
            {
                T lApiResponse = await Task<T>.Factory.StartNew(() =>
                    JsonConvert.DeserializeObject<T>(WebUtility.HtmlDecode(lResult.Result),
                        settings ?? new JsonSerializerSettings())).ConfigureAwait(false);

                if (lApiResponse.Success) return lApiResponse;

                Exception lException = HandleErrorCode(lApiResponse.ErrorCode, request);
                if (lException == null) return new ProxerResult(new ProxerApiException(lApiResponse.ErrorCode));
                if (lException is NotLoggedInException && (loopCount < 5))
                    return await ApiRequestInternal<T>(request, true, settings, loopCount + 1).ConfigureAwait(false);

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
                {"proxer-api-key", new string(_apiKey.ReadValue())}
            };
            if (request.Senpai == null) return lHeaders;
            if (forceTokenLogin || (request.CheckLogin && !request.Senpai.IsProbablyLoggedIn))
                lHeaders.Add("proxer-api-token", new string(request.Senpai.LoginToken.ReadValue()));

            return lHeaders;
        }

        private static Exception HandleErrorCode(ErrorCode code, ApiRequest request)
        {
            switch (code)
            {
                case ErrorCode.IpBlocked:
                    return new CaptchaException("http://proxer.me/misc/captcha");
                case ErrorCode.ApiKeyInsufficientPermissions:
                    return new ApiKeyInsufficientException();
                case ErrorCode.UserInsufficientPermissions:
                    return new NoAccessException(request.Senpai);
                case ErrorCode.NotificationsUserNotLoggedIn:
                case ErrorCode.UcpUserNotLoggedIn:
                case ErrorCode.InfoSetUserInfoUserNotLoggedIn:
                case ErrorCode.MessengerUserNotLoggedIn:
                    return new NotLoggedInException(request.Senpai);
            }

            return null;
        }

        internal static void Init(char[] apiKey)
        {
            _apiKey = ApiInfo.SecureContainerFactory.Invoke();
            _apiKey.SetValue(apiKey);
        }

        #endregion
    }
}