using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Web;
using Newtonsoft.Json;

namespace Azuria.Api.v1
{
    /// <summary>
    /// </summary>
    public static class RequestHandler
    {
        private static string _apiKey;

        #region

        internal static async Task<ProxerResult<T>> ApiCustomRequest<T>(ApiRequest request, char[] loginToken = null,
            int recursion = 0) where T : ProxerApiResponse
        {
            loginToken = loginToken ?? new char[0];
            ProxerResult<string> lResult =
                await
                    HttpUtility.PostResponseErrorHandling(request.Address, request.PostArguments, request.Senpai,
                        new Func<string, ProxerResult>[0], checkLogin: request.CheckLogin,
                        header:
                            new Dictionary<string, string> {{"proxer-api-key", _apiKey}}.AddIfAndReturn(
                                "proxer-api-token", new string(loginToken), (key, value) => !string.IsNullOrEmpty(value.Trim()))
                                .AddIfAndReturn("proxer-api-token", new string(request.Senpai.LoginToken),
                                    (key, value, source) =>
                                        request.CheckLogin && !request.Senpai.IsProbablyLoggedIn &&
                                        !source.ContainsKey(key)));

            if (!lResult.Success || lResult.Result == null) return new ProxerResult<T>(lResult.Exceptions);

            try
            {
                T lApiResponse = await
                    Task<T>.Factory.StartNew(
                        () => JsonConvert.DeserializeObject<T>(WebUtility.HtmlDecode(lResult.Result)));
                if (!lApiResponse.Error) return new ProxerResult<T>(lApiResponse);
                switch (lApiResponse.ErrorCode)
                {
                    case ErrorCode.IpBlocked:
                        return new ProxerResult<T>(new[] {new FirewallException()});
                    case ErrorCode.ApiKeyInsufficientPermissions:
                        return new ProxerResult<T>(new[] {new ApiKeyInsufficientException()});
                    case ErrorCode.UserInsufficientPermissions:
                        return new ProxerResult<T>(new[] {new NoAccessException(request.Senpai)});
                    case ErrorCode.NotificationsUserNotLoggedIn:
                    case ErrorCode.UcpUserNotLoggedIn:
                    case ErrorCode.InfoSetUserInfoUserNotLoggedIn:
                        if (recursion >= 5)
                            return new ProxerResult<T>(new[] {new NotLoggedInException(request.Senpai)});
                        if (
                            (await request.Senpai.LoginWithToken(request.Senpai.LoginToken ?? new char[0]))
                                .Success)
                            return await ApiCustomRequest<T>(request, loginToken, recursion + 1);
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

        internal static void Init(string apiKey)
        {
            _apiKey = apiKey;
        }

        #endregion
    }
}