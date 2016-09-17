using System;
using System.Collections.Generic;
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

        internal static async Task<ProxerResult<T>> ApiCustomRequest<T>(ApiRequest request, bool forceTokenLogin = false)
            where T : ProxerApiResponse
        {
            if (request.CheckLogin && (request.Senpai == null))
                return new ProxerResult<T>(new[] {new NotLoggedInException()});

            ProxerResult<string> lResult =
                await
                    ApiInfo.HttpClient.PostRequest(request.Address, request.PostArguments,
                        new Func<string, ProxerResult>[0], checkLogin: request.CheckLogin, senpai: request.Senpai,
                        headers: GetHeaders(request, forceTokenLogin));
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult<T>(lResult.Exceptions);

            try
            {
                T lApiResponse = await
                    Task<T>.Factory.StartNew(
                        () => JsonConvert.DeserializeObject<T>(WebUtility.HtmlDecode(lResult.Result)));
                return !lApiResponse.Error
                    ? new ProxerResult<T>(lApiResponse)
                    : await HandleErrorCode<T>(lApiResponse.ErrorCode, request);
            }
            catch (Exception ex)
            {
                return new ProxerResult<T>(new[] {ex});
            }
        }

        internal static Task<ProxerResult<ProxerApiResponse<T>>> ApiRequest<T>(ApiRequest<T> request,
            char[] loginToken = null)
        {
            return ApiCustomRequest<ProxerApiResponse<T>>(request);
        }

        internal static Task<ProxerResult<ProxerApiResponse>> ApiRequest(ApiRequest request)
        {
            return ApiCustomRequest<ProxerApiResponse>(request);
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

        private static async Task<ProxerResult<T>> HandleErrorCode<T>(ErrorCode code, ApiRequest request)
            where T : ProxerApiResponse
        {
            List<Exception> lExceptions = new List<Exception>(new[] {new ProxerApiException(code)});
            switch (code)
            {
                case ErrorCode.IpBlocked:
                    lExceptions.Add(new CaptchaException("http://proxer.me/misc/captcha"));
                    break;
                case ErrorCode.ApiKeyInsufficientPermissions:
                    lExceptions.Add(new ApiKeyInsufficientException());
                    break;
                case ErrorCode.UserInsufficientPermissions:
                    lExceptions.Add(new NoAccessException(request.Senpai));
                    break;
                case ErrorCode.LoginTokenInvalid:
                    lExceptions.Add(new NotLoggedInException(request.Senpai));
                    break;
                case ErrorCode.NotificationsUserNotLoggedIn:
                case ErrorCode.UcpUserNotLoggedIn:
                case ErrorCode.InfoSetUserInfoUserNotLoggedIn:
                case ErrorCode.MessengerUserNotLoggedIn:
                    return await ApiCustomRequest<T>(request, true);
            }

            return new ProxerResult<T>(lExceptions);
        }

        internal static void Init(char[] apiKey)
        {
            _apiKey = ApiInfo.SecureContainerFactory.Invoke();
            _apiKey.SetValue(apiKey);
        }

        #endregion
    }
}