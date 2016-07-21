using System;
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

        internal static async Task<ProxerResult<ProxerApiResponse<T>>> ApiRequest<T>(ApiRequest<T> request)
        {
            request.PostArguments.Add("api_key", _apiKey);
            ProxerResult<string> lResult =
                await
                    HttpUtility.PostResponseErrorHandling(request.Address, request.PostArguments, request.Senpai,
                        new Func<string, ProxerResult>[0], checkLogin: request.CheckLogin);

            if (!lResult.Success) return new ProxerResult<ProxerApiResponse<T>>(lResult.Exceptions);

            try
            {
                ProxerApiResponse<T> lApiResponse = await
                    Task<ProxerApiResponse<T>>.Factory.StartNew(
                        () => JsonConvert.DeserializeObject<ProxerApiResponse<T>>(lResult.Result));
                if (!lApiResponse.Error) return new ProxerResult<ProxerApiResponse<T>>(lApiResponse);
                switch (lApiResponse.ErrorCode)
                {
                    case ErrorCode.IpBlocked:
                        return new ProxerResult<ProxerApiResponse<T>>(new[] {new FirewallException()});
                    case ErrorCode.ApiKeyNotAuthorised:
                        return new ProxerResult<ProxerApiResponse<T>>(new[] {new ApiKeyInsufficentException()});
                }
                return new ProxerResult<ProxerApiResponse<T>>(lApiResponse);
            }
            catch (Exception ex)
            {
                return new ProxerResult<ProxerApiResponse<T>>(new[] {ex});
            }
        }

        internal static async Task<ProxerResult<ProxerApiResponse>> ApiRequest(ApiRequest request)
        {
            request.PostArguments.Add("api_key", _apiKey);
            ProxerResult<string> lResult =
                await HttpUtility.PostResponseErrorHandling(request.Address, request.PostArguments, request.Senpai);

            if (!lResult.Success) return new ProxerResult<ProxerApiResponse>(lResult.Exceptions);

            try
            {
                ProxerApiResponse lApiResponse = await
                    Task<ProxerApiResponse>.Factory.StartNew(
                        () => JsonConvert.DeserializeObject<ProxerApiResponse>(lResult.Result));
                if (!lApiResponse.Error) return new ProxerResult<ProxerApiResponse>(lApiResponse);
                switch (lApiResponse.ErrorCode)
                {
                    case ErrorCode.IpBlocked:
                        return new ProxerResult<ProxerApiResponse>(new[] {new FirewallException()});
                    case ErrorCode.ApiKeyNotAuthorised:
                        return new ProxerResult<ProxerApiResponse>(new[] {new ApiKeyInsufficentException()});
                    case ErrorCode.LoginUserAlreadyLoggedIn:
                    case ErrorCode.LoginDifferentUserLoggedIn:
                        return new ProxerResult<ProxerApiResponse>(new[] {new UserAlreadyLoggedInException()});
                }
                return new ProxerResult<ProxerApiResponse>(lApiResponse);
            }
            catch (Exception ex)
            {
                return new ProxerResult<ProxerApiResponse>(new[] {ex});
            }
        }

        #endregion
    }
}