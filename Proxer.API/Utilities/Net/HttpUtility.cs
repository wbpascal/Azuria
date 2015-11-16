using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Proxer.API.Exceptions;
using RestSharp;

namespace Proxer.API.Utilities.Net
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

        #region

        internal static async Task<IRestResponse> GetWebRequestResponse(string url, CookieContainer cookies)
        {
            RestClient lClient = new RestClient(url)
            {
                CookieContainer = cookies,
                Encoding = Encoding.UTF8,
                Timeout = Timeout
            };
            RestRequest lRequest = new RestRequest(Method.GET);
            return await lClient.ExecuteTaskAsync(lRequest);
        }

        internal static async Task<IRestResponse> PostWebRequestResponse(string url, CookieContainer cookies,
                                                                         Dictionary<string, string> postArgs)
        {
            RestClient lClient = new RestClient(url)
            {
                CookieContainer = cookies,
                Encoding = Encoding.UTF8,
                Timeout = Timeout
            };
            RestRequest lRequest = new RestRequest(Method.POST);
            postArgs.ToList().ForEach(x => lRequest.AddParameter(x.Key, x.Value));
            return await lClient.ExecuteTaskAsync(lRequest);
        }

        internal static async Task<ProxerResult<string>> GetResponseErrorHandling(string url, ErrorHandler errorHandler,
                                                                                  Senpai senpai)
        {
            return await GetResponseErrorHandling(url, null, errorHandler, senpai);
        }

        internal static async Task<ProxerResult<string>> GetResponseErrorHandling(string url,
                                                                                  CookieContainer loginCookies,
                                                                                  ErrorHandler errorHandler,
                                                                                  Senpai senpai)
        {
            return
                await
                    GetResponseErrorHandling(url, loginCookies, errorHandler, senpai, new Func<string, ProxerResult>[0]);
        }

        internal static async Task<ProxerResult<string>> GetResponseErrorHandling(string url,
                                                                                  CookieContainer loginCookies,
                                                                                  ErrorHandler errorHandler,
                                                                                  Senpai senpai,
                                                                                  Func<string, ProxerResult>[]
                                                                                      checkFuncs)
        {
            ProxerResult<Tuple<string, CookieContainer>> lResult =
                await
                    GetResponseErrorHandling(url, loginCookies, errorHandler, senpai, checkFuncs, loginCookies == null);

            return lResult.Success
                ? new ProxerResult<string>(lResult.Result.Item1)
                : new ProxerResult<string>(lResult.Exceptions);
        }

        internal static async Task<ProxerResult<Tuple<string, CookieContainer>>> GetResponseErrorHandling(
            string url, CookieContainer loginCookies, ErrorHandler errorHandler, Senpai senpai,
            Func<string, ProxerResult>[] checkFuncs, bool checkLogin)
        {
            if (checkLogin && loginCookies != null && !senpai.LoggedIn)
                return
                    new ProxerResult<Tuple<string, CookieContainer>>(new Exception[] {new NotLoggedInException()});

            string lResponse;

            IRestResponse lResponseObject =
                await GetWebRequestResponse(url, loginCookies);
            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseObject.Content))
                lResponse = System.Web.HttpUtility.HtmlDecode(lResponseObject.Content).Replace("\n", "");
            else
                return
                    new ProxerResult<Tuple<string, CookieContainer>>(new[]
                    {new WrongResponseException(), lResponseObject.ErrorException});

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
                    return new ProxerResult<Tuple<string, CookieContainer>> {Success = false};
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

        internal static async Task<ProxerResult<string>> PostResponseErrorHandling(string url,
                                                                                   Dictionary<string, string> postArgs,
                                                                                   ErrorHandler errorHandler,
                                                                                   Senpai senpai)
        {
            return await PostResponseErrorHandling(url, postArgs, null, errorHandler, senpai);
        }

        internal static async Task<ProxerResult<string>> PostResponseErrorHandling(string url,
                                                                                   Dictionary<string, string> postArgs,
                                                                                   CookieContainer loginCookies,
                                                                                   ErrorHandler errorHandler,
                                                                                   Senpai senpai)
        {
            return
                await
                    PostResponseErrorHandling(url, postArgs, loginCookies, errorHandler, senpai,
                        new Func<string, ProxerResult>[0]);
        }

        internal static async Task<ProxerResult<string>> PostResponseErrorHandling(string url,
                                                                                   Dictionary<string, string> postArgs,
                                                                                   CookieContainer loginCookies,
                                                                                   ErrorHandler errorHandler,
                                                                                   Senpai senpai,
                                                                                   Func<string, ProxerResult>[]
                                                                                       checkFuncs)
        {
            ProxerResult<KeyValuePair<string, CookieContainer>> lResult =
                await
                    PostResponseErrorHandling(url, postArgs, loginCookies, errorHandler, senpai, checkFuncs,
                        loginCookies == null);

            return lResult.Success
                ? new ProxerResult<string>(lResult.Result.Key)
                : new ProxerResult<string>(lResult.Exceptions);
        }

        internal static async Task<ProxerResult<KeyValuePair<string, CookieContainer>>> PostResponseErrorHandling(
            string url, Dictionary<string, string> postArgs, CookieContainer loginCookies, ErrorHandler errorHandler,
            Senpai senpai,
            Func<string, ProxerResult>[] checkFuncs, bool checkLogin)
        {
            if (checkLogin && loginCookies != null && !senpai.LoggedIn)
                return
                    new ProxerResult<KeyValuePair<string, CookieContainer>>(new Exception[] {new NotLoggedInException()});

            string lResponse;

            IRestResponse lResponseObject =
                await PostWebRequestResponse(url, loginCookies, postArgs);
            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseObject.Content))
                lResponse = System.Web.HttpUtility.HtmlDecode(lResponseObject.Content).Replace("\n", "");
            else
                return
                    new ProxerResult<KeyValuePair<string, CookieContainer>>(new[]
                    {new WrongResponseException(), lResponseObject.ErrorException});

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
                    return new ProxerResult<KeyValuePair<string, CookieContainer>> {Success = false};
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

        #endregion
    }
}