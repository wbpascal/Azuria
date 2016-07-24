using System;
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

        internal static void Init(string apiKey)
        {
            _apiKey = apiKey;
        }

        internal static Task<ProxerResult<ProxerApiResponse<T>>> ApiRequest<T>(ApiRequest<T> request)
        {
            return ApiCustomRequest<ProxerApiResponse<T>>(request);
        }

        internal static async Task<ProxerResult<T>> ApiCustomRequest<T>(ApiRequest request) where T : ProxerApiResponse
        {
            request.PostArguments.Add("api_key", _apiKey);
            ProxerResult<string> lResult =
                await
                    HttpUtility.PostResponseErrorHandling(request.Address, request.PostArguments, request.Senpai,
                        new Func<string, ProxerResult>[0], checkLogin: request.CheckLogin);

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
                    case ErrorCode.ApiKeyNotAuthorised:
                        return new ProxerResult<T>(new[] {new ApiKeyInsufficentException()});
                }
                return new ProxerResult<T>(lApiResponse);
            }
            catch (Exception ex)
            {
                return new ProxerResult<T>(new[] {ex});
            }
        }

        internal static Task<ProxerResult<ProxerApiResponse>> ApiRequest(ApiRequest request)
        {
            return ApiCustomRequest<ProxerApiResponse>(request);
        }

        #endregion
    }
}