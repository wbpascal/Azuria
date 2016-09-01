using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Security;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Newtonsoft.Json;

namespace Azuria.Api.v1
{
    /// <summary>
    /// </summary>
    public static class RequestHandler
    {
        private static ISecureContainer<char[]> _apiKey;

        #region

        internal static async Task<ProxerResult<T>> ApiCustomRequest<T>(ApiRequest request, char[] loginToken = null,
            int recursion = 0) where T : ProxerApiResponse
        {
            if (request.CheckLogin && (request.Senpai == null))
                return new ProxerResult<T>(new[] {new NotLoggedInException()});

            loginToken = loginToken ?? new char[0];
            ProxerResult<string> lResult =
                await
                    ApiInfo.HttpClient.PostRequest(request.Address, request.PostArguments,
                        new Func<string, ProxerResult>[0], checkLogin: request.CheckLogin, senpai: request.Senpai,
                        header:
                        new Dictionary<string, string> {{"proxer-api-key", new string(_apiKey.ReadValue())}}
                            .AddIfAndReturn(
                                "proxer-api-token", new string(loginToken),
                                (key, value) => !string.IsNullOrEmpty(value.Trim()))
                            .AddIfAndReturn("proxer-api-token", new string(request.Senpai?.LoginToken.ReadValue()),
                                (key, value, source) =>
                                    request.CheckLogin && (!request.Senpai?.IsProbablyLoggedIn ?? false) &&
                                    !source.ContainsKey(key)));

            if (!lResult.Success || (lResult.Result == null))
            {
                if (lResult.Exceptions.Any(exception => typeof(Exception) == typeof(NotLoggedInException)))
                    return new ProxerResult<T>(lResult.Exceptions);

                #region try again

                if (recursion >= 2)
                {
                    request.Senpai?.InvalidateCookies();
                    return new ProxerResult<T>(new[] {new NotLoggedInException(request.Senpai)});
                }
                if (
                    (await request.Senpai.LoginWithToken(request.Senpai.LoginToken.ReadValue()))
                        .Success)
                    return await ApiCustomRequest<T>(request, loginToken, recursion + 1);

                #endregion

                return new ProxerResult<T>(lResult.Exceptions);
            }

            try
            {
                T lApiResponse = await
                    Task<T>.Factory.StartNew(
                        () => JsonConvert.DeserializeObject<T>(WebUtility.HtmlDecode(lResult.Result)));
                if (!lApiResponse.Error) return new ProxerResult<T>(lApiResponse);
                switch (lApiResponse.ErrorCode)
                {
                    case ErrorCode.IpBlocked:
                        return new ProxerResult<T>(new[] {new CaptchaException("http://proxer.me/misc/captcha")});
                    case ErrorCode.ApiKeyInsufficientPermissions:
                        return new ProxerResult<T>(new[] {new ApiKeyInsufficientException()});
                    case ErrorCode.UserInsufficientPermissions:
                        return new ProxerResult<T>(new[] {new NoAccessException(request.Senpai)});
                    case ErrorCode.NotificationsUserNotLoggedIn:
                    case ErrorCode.UcpUserNotLoggedIn:
                    case ErrorCode.InfoSetUserInfoUserNotLoggedIn:
                    case ErrorCode.MessengerUserNotLoggedIn:

                        #region try again

                        if (recursion >= 2)
                        {
                            request.Senpai.InvalidateCookies();
                            return new ProxerResult<T>(new[] {new NotLoggedInException(request.Senpai)});
                        }
                        if (
                            (await request.Senpai.LoginWithToken(request.Senpai.LoginToken.ReadValue()))
                                .Success)
                            return await ApiCustomRequest<T>(request, loginToken, recursion + 1);

                        #endregion

                        break;
                }
                return new ProxerResult<T>(new[] {new ProxerApiException(lApiResponse.ErrorCode)});
            }
            catch (Exception ex)
            {
                return new ProxerResult<T>(new[] {ex});
            }
        }

        internal static Task<ProxerResult<ProxerApiResponse<T>>> ApiRequest<T>(ApiRequest<T> request,
            char[] loginToken = null)
        {
            return ApiCustomRequest<ProxerApiResponse<T>>(request, loginToken);
        }

        internal static Task<ProxerResult<ProxerApiResponse>> ApiRequest(ApiRequest request, char[] loginToken = null)
        {
            return ApiCustomRequest<ProxerApiResponse>(request, loginToken);
        }

        internal static void Init(char[] apiKey)
        {
            _apiKey = ApiInfo.SecureContainerFactory.Invoke();
            _apiKey.SetValue(apiKey);
        }

        #endregion
    }
}