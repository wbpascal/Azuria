using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace Azuria.Utilities.Net
{
    /// <summary>
    ///     Eine Klasse, die alle Methoden darstellt, um per HTTP- und HTTPS-
    ///     Protokol mit dem Server zu kommunizieren.
    /// </summary>
    public class HttpUtility
    {
        /// <summary>
        ///     Gibt die Zeit in Millisekunden zurück, die der Client auf eine Antwort wartet bis er abbricht, oder legt diese
        ///     fest.
        ///     Standartwert = 0
        /// </summary>
        public static int Timeout = 0;

        [NotNull] private static readonly string UserAgent = "Azuria.Portable/" +
                                                             new AssemblyName(
                                                                 typeof(HttpUtility).GetTypeInfo().Assembly.FullName)
                                                                 .Version +
                                                             "RestSharp.Portable/3.1.0.0";

        #region

        [ItemNotNull]
        internal static async Task<ProxerResult<string>> GetResponseErrorHandling(Uri url,
            [NotNull] ErrorHandler errorHandler, [NotNull] Senpai senpai)
        {
            return await GetResponseErrorHandling(url, null, errorHandler, senpai);
        }

        [ItemNotNull]
        internal static async Task<ProxerResult<string>> GetResponseErrorHandling([NotNull] Uri url,
            [CanBeNull] CookieContainer loginCookies, [NotNull] ErrorHandler errorHandler, [NotNull] Senpai senpai)
        {
            return
                await
                    GetResponseErrorHandling(url, loginCookies, errorHandler, senpai, new Func<string, ProxerResult>[0]);
        }

        [ItemNotNull]
        internal static async Task<ProxerResult<string>> GetResponseErrorHandling([NotNull] Uri url,
            [CanBeNull] CookieContainer loginCookies, [NotNull] ErrorHandler errorHandler, [NotNull] Senpai senpai,
            [CanBeNull] Func<string, ProxerResult>[] checkFuncs)
        {
            ProxerResult<Tuple<string, CookieContainer>> lResult =
                await
                    GetResponseErrorHandling(url, loginCookies, errorHandler, senpai, checkFuncs, loginCookies == null);

            return lResult.Success && lResult.Result != null
                ? new ProxerResult<string>(lResult.Result.Item1)
                : new ProxerResult<string>(lResult.Exceptions);
        }

        [ItemNotNull]
        internal static async Task<ProxerResult<Tuple<string, CookieContainer>>> GetResponseErrorHandling(
            [NotNull] Uri url, [CanBeNull] CookieContainer loginCookies, [NotNull] ErrorHandler errorHandler,
            [NotNull] Senpai senpai, [CanBeNull] Func<string, ProxerResult>[] checkFuncs, bool checkLogin)
        {
            if (checkLogin && loginCookies != null && !senpai.IsLoggedIn)
                return
                    new ProxerResult<Tuple<string, CookieContainer>>(new Exception[] {new NotLoggedInException()});

            string lResponse;

            IRestResponse lResponseObject =
                await GetWebRequestResponse(url, loginCookies);
            string lResponseString = Encoding.UTF8.GetString(lResponseObject.RawBytes, 0,
                lResponseObject.RawBytes.Length);

            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseString))
                lResponse = WebUtility.HtmlDecode(lResponseString).Replace("\n", "");
            else
                return
                    new ProxerResult<Tuple<string, CookieContainer>>(new[]
                    {new WrongResponseException()});

            if (checkFuncs != null)
                foreach (Func<string, ProxerResult> checkFunc in checkFuncs)
                {
                    try
                    {
                        ProxerResult lResult = checkFunc?.Invoke(lResponse) ?? new ProxerResult {Success = false};
                        if (!lResult.Success)
                            return new ProxerResult<Tuple<string, CookieContainer>>(lResult.Exceptions);
                    }
                    catch
                    {
                        return new ProxerResult<Tuple<string, CookieContainer>>(new Exception[0]) {Success = false};
                    }
                }

            if (string.IsNullOrEmpty(lResponse) || !Utility.CheckForCorrectResponse(lResponse, errorHandler))
                return
                    new ProxerResult<Tuple<string, CookieContainer>>(new Exception[]
                    {new WrongResponseException {Response = lResponse}});

            return
                new ProxerResult<Tuple<string, CookieContainer>>(
                    new Tuple<string, CookieContainer>(lResponse, loginCookies));
        }

        [ItemNotNull]
        internal static async Task<IRestResponse> GetWebRequestResponse([NotNull] Uri url,
            [CanBeNull] CookieContainer cookies)
        {
            RestClient lClient = new RestClient(url)
            {
                CookieContainer = cookies,
                Timeout = TimeSpan.FromMilliseconds(Timeout),
                UserAgent = UserAgent
            };
            RestRequest lRequest = new RestRequest(Method.GET);
            return await lClient.Execute(lRequest);
        }

        [ItemNotNull]
        internal static async Task<ProxerResult<string>> PostResponseErrorHandling([NotNull] Uri url,
            [NotNull] Dictionary<string, string> postArgs, [NotNull] ErrorHandler errorHandler, [NotNull] Senpai senpai)
        {
            return await PostResponseErrorHandling(url, postArgs, null, errorHandler, senpai);
        }

        [ItemNotNull]
        internal static async Task<ProxerResult<string>> PostResponseErrorHandling([NotNull] Uri url,
            [NotNull] Dictionary<string, string> postArgs, [CanBeNull] CookieContainer loginCookies,
            [NotNull] ErrorHandler errorHandler, [NotNull] Senpai senpai)
        {
            return
                await
                    PostResponseErrorHandling(url, postArgs, loginCookies, errorHandler, senpai,
                        new Func<string, ProxerResult>[0]);
        }

        [ItemNotNull]
        internal static async Task<ProxerResult<string>> PostResponseErrorHandling([NotNull] Uri url,
            [NotNull] Dictionary<string, string> postArgs, [CanBeNull] CookieContainer loginCookies,
            [NotNull] ErrorHandler errorHandler, [NotNull] Senpai senpai,
            [CanBeNull] Func<string, ProxerResult>[] checkFuncs)
        {
            ProxerResult<KeyValuePair<string, CookieContainer>> lResult =
                await
                    PostResponseErrorHandling(url, postArgs, loginCookies, errorHandler, senpai, checkFuncs,
                        loginCookies == null);

            return lResult.Success
                ? new ProxerResult<string>(lResult.Result.Key)
                : new ProxerResult<string>(lResult.Exceptions);
        }

        [ItemNotNull]
        internal static async Task<ProxerResult<KeyValuePair<string, CookieContainer>>> PostResponseErrorHandling(
            [NotNull] Uri url, [NotNull] Dictionary<string, string> postArgs,
            [CanBeNull] CookieContainer loginCookies, [NotNull] ErrorHandler errorHandler, [NotNull] Senpai senpai,
            [CanBeNull] Func<string, ProxerResult>[] checkFuncs, bool checkLogin)
        {
            if (checkLogin && loginCookies != null && !senpai.IsLoggedIn)
                return
                    new ProxerResult<KeyValuePair<string, CookieContainer>>(new Exception[] {new NotLoggedInException()});

            string lResponse;

            IRestResponse lResponseObject =
                await PostWebRequestResponse(url, loginCookies, postArgs);
            string lResponseString = Encoding.UTF8.GetString(lResponseObject.RawBytes, 0,
                lResponseObject.RawBytes.Length);

            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseString))
                lResponse = WebUtility.HtmlDecode(lResponseString).Replace("\n", "");
            else
                return
                    new ProxerResult<KeyValuePair<string, CookieContainer>>(new[]
                    {new WrongResponseException()});

            if (checkFuncs != null)
                foreach (Func<string, ProxerResult> checkFunc in checkFuncs)
                {
                    try
                    {
                        ProxerResult lResult = checkFunc?.Invoke(lResponse) ?? new ProxerResult {Success = false};
                        if (!lResult.Success)
                            return new ProxerResult<KeyValuePair<string, CookieContainer>>(lResult.Exceptions);
                    }
                    catch
                    {
                        return new ProxerResult<KeyValuePair<string, CookieContainer>>(new Exception[0])
                        {
                            Success = false
                        };
                    }
                }

            if (string.IsNullOrEmpty(lResponse) || !Utility.CheckForCorrectResponse(lResponse, errorHandler))
                return
                    new ProxerResult<KeyValuePair<string, CookieContainer>>(new Exception[]
                    {new WrongResponseException {Response = lResponse}});

            return
                new ProxerResult<KeyValuePair<string, CookieContainer>>(
                    new KeyValuePair<string, CookieContainer>(lResponse, loginCookies));
        }

        [ItemNotNull]
        internal static async Task<IRestResponse> PostWebRequestResponse([NotNull] Uri url,
            [CanBeNull] CookieContainer cookies, [NotNull] Dictionary<string, string> postArgs)
        {
            RestClient lClient = new RestClient(url)
            {
                CookieContainer = cookies,
                Timeout = TimeSpan.FromMilliseconds(Timeout),
                UserAgent = UserAgent
            };
            RestRequest lRequest = new RestRequest(Method.POST);
            foreach (KeyValuePair<string, string> pair in postArgs)
                lRequest.AddParameter(pair.Key, pair.Value);

            return await lClient.Execute(lRequest);
        }

        #endregion
    }
}