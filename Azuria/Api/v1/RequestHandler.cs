using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
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

        internal static async Task<ProxerResult<T>> ApiCustomRequest<T>(ApiRequest request) where T : ProxerApiResponse
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.PostResponseErrorHandling(request.Address, request.PostArguments, request.Senpai,
                        new Func<string, ProxerResult>[0], checkLogin: request.CheckLogin,
                        header: new Dictionary<string, string> {{"proxer-api-key", _apiKey}});

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
                        return new ProxerResult<T>(new[] {new NoAccessException(request.Senpai) });
                }
                return new ProxerResult<T>(lApiResponse);
            }
            catch (Exception ex)
            {
                return new ProxerResult<T>(new[] {ex});
            }
        }

        internal static Task<ProxerResult<ProxerApiResponse<T>>> ApiRequest<T>(ApiRequest<T> request)
        {
            return ApiCustomRequest<ProxerApiResponse<T>>(request);
        }

        internal static Task<ProxerResult<ProxerApiResponse>> ApiRequest(ApiRequest request)
        {
            return ApiCustomRequest<ProxerApiResponse>(request);
        }

        internal static void Init(string apiKey)
        {
            _apiKey = apiKey;
        }

        #endregion
    }
}