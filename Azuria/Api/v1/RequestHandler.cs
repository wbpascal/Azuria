using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using Azuria.Api.Builder;
using Azuria.Authentication;
using Azuria.Connection;
using Azuria.Enums;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Newtonsoft.Json;

namespace Azuria.Api.v1
{
    internal static class RequestHandler
    {
        #region Methods

        internal static async Task<IProxerResult<T>> ApiRequestAsync<T>(this IUrlBuilderWithResult<T> request)
        {
            IEnumerable<JsonConverter> lDataConverter = request.CustomDataConverter == null
                                                            ? new JsonConverter[0]
                                                            : new[] {request.CustomDataConverter};

            IProxerResult lResult = await request.ApiRequestInternalAsync<ProxerApiResponse<T>>(
                                            new JsonSerializerSettings {Converters = lDataConverter.ToList()}
                                        )
                                        .ConfigureAwait(false);

            return lResult.Success && lResult is ProxerApiResponse<T>
                       ? lResult as ProxerApiResponse<T>
                       : (IProxerResult<T>) new ProxerResult<T>(lResult.Exceptions);
        }

        internal static async Task<IProxerResult> ApiRequestAsync(this IUrlBuilder request)
        {
            IProxerResult lResult = await request.ApiRequestInternalAsync<ProxerApiResponse>()
                                        .ConfigureAwait(false);

            return lResult.Success && lResult is ProxerApiResponse
                       ? lResult as ProxerApiResponse
                       : new ProxerResult(lResult.Exceptions);
        }

        private static async Task<IProxerResult> ApiRequestInternalAsync<T>(
            this IUrlBuilderBase request,
            JsonSerializerSettings settings = null) where T : ProxerApiResponse
        {
            ILoginManager lLoginManager = request.Client.Container.Resolve<ILoginManager>();

            IProxerResult<string> lResult = await request.Client.Container.Resolve<IHttpClient>()
                                                .ProxerRequestAsync(
                                                    request.BuildUri(), request.PostArguments,
                                                    GetHeaders(request.Client.ApiKey, lLoginManager)
                                                )
                                                .ConfigureAwait(false);
            if (!lResult.Success || string.IsNullOrEmpty(lResult.Result))
                return new ProxerResult(lResult.Exceptions);

            try
            {
                T lApiResponse = await Task<T>.Factory.StartNew(
                                         () =>
                                             JsonConvert.DeserializeObject<T>(
                                                 WebUtility.HtmlDecode(lResult.Result),
                                                 settings ?? new JsonSerializerSettings()
                                             )
                                     )
                                     .ConfigureAwait(false);

                if (lApiResponse.Success) return lApiResponse;

                Exception lException = HandleErrorCode(lApiResponse.ErrorCode);
                if (!(lException is NotAuthenticatedException))
                    return lException == null
                               ? new ProxerResult(new ProxerApiException(lApiResponse.ErrorCode))
                               : new ProxerResult(lException);

                lLoginManager.QueueLoginForNextRequest();
                return await ApiRequestInternalAsync<T>(request, settings).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new ProxerResult(ex);
            }
        }

        private static Dictionary<string, string> GetHeaders(char[] apiKey, ILoginManager loginManager)
        {
            Dictionary<string, string> lHeaders = new Dictionary<string, string>
            {
                {"proxer-api-key", new string(apiKey)}
            };
            if (loginManager.SendTokenWithNextRequest())
                lHeaders.Add("proxer-api-token", new string(loginManager.LoginToken));

            return lHeaders;
        }

        private static Exception HandleErrorCode(ErrorCode code)
        {
            switch (code)
            {
                case ErrorCode.IpBlocked:
                    return new FirewallException("http://proxer.me/misc/captcha");
                case ErrorCode.ApiKeyNoPermission:
                    return new ApiKeyInsufficientException();
                case ErrorCode.UserNoPermission:
                case ErrorCode.ChatNoPermission:
                    return new NoPermissionException();
                case ErrorCode.NotificationsNotLoggedIn:
                case ErrorCode.UcpNotLoggedIn:
                case ErrorCode.InfoNotLoggedIn:
                case ErrorCode.MessengerNotLoggedIn:
                case ErrorCode.ChatNotLoggedIn:
                    return new NotAuthenticatedException();
            }

            return null;
        }

        private static Task<IProxerResult<string>> ProxerRequestAsync(
            this IHttpClient httpClient, Uri url,
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