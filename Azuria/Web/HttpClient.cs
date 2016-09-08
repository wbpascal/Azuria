using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Exceptions;

namespace Azuria.Web
{
    /// <summary>
    ///     Represents a class that communicates with the proxer servers.
    /// </summary>
    public class HttpClient : IHttpClient
    {
#if PORTABLE
        private static readonly string UserAgent = "Azuria.Portable/" + 
                                                   typeof(HttpClient).GetTypeInfo().Assembly.GetName().Version;
#else
        private static readonly string UserAgent = "Azuria/" +
                                                   typeof(HttpClient).GetTypeInfo().Assembly.GetName().Version;
#endif

        private readonly int _timeout;
        private readonly string _userAgentExtra;

        /// <summary>
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="userAgentExtra"></param>
        public HttpClient(int timeout = 5000, string userAgentExtra = "")
        {
            this._timeout = timeout;
            this._userAgentExtra = userAgentExtra;
        }

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public Task<ProxerResult<string>> GetRequest(Uri url, Senpai senpai = null)
        {
            return this.GetRequest(url, new Func<string, ProxerResult>[0], senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="checkFuncs"></param>
        /// <param name="senpai"></param>
        /// <param name="useMobileCookies"></param>
        /// <param name="checkLogin"></param>
        /// <param name="recursion"></param>
        /// <returns></returns>
        public async Task<ProxerResult<string>> GetRequest(Uri url, Func<string, ProxerResult>[] checkFuncs,
            Senpai senpai = null, bool useMobileCookies = false, bool checkLogin = true, int recursion = 0)
        {
            if (checkLogin && (!senpai?.IsProbablyLoggedIn ?? true))
                return
                    new ProxerResult<string>(new Exception[] {new NotLoggedInException()});

            string lResponse;

            HttpResponseMessage lResponseObject;
            try
            {
                lResponseObject =
                    await
                        this.GetWebRequest(url,
                            useMobileCookies ? senpai?.MobileLoginCookies : senpai?.LoginCookies,
                            null);
            }
            catch (Exception ex)
            {
                return new ProxerResult<string>(new[] {ex});
            }
            senpai?.UsedCookies();
            string lResponseString = await lResponseObject.Content.ReadAsStringAsync();

            if ((lResponseObject.StatusCode == HttpStatusCode.OK) && !string.IsNullOrEmpty(lResponseString))
                lResponse = WebUtility.HtmlDecode(lResponseString).Replace("\n", "");
            else if ((lResponseObject.StatusCode == HttpStatusCode.ServiceUnavailable) &&
                     !string.IsNullOrEmpty(lResponseString))
                return new ProxerResult<string>(new[] {new CloudflareException()});
            else
                return
                    new ProxerResult<string>(new[] {new WrongResponseException()});

            if (checkFuncs != null)
                foreach (Func<string, ProxerResult> checkFunc in checkFuncs)
                    try
                    {
                        ProxerResult lResult = checkFunc?.Invoke(lResponse) ?? new ProxerResult {Success = false};
                        if (!lResult.Success)
                            return new ProxerResult<string>(lResult.Exceptions);
                    }
                    catch
                    {
                        return new ProxerResult<string>(new Exception[0]) {Success = false};
                    }

            return string.IsNullOrEmpty(lResponse)
                ? new ProxerResult<string>(new Exception[] {new WrongResponseException()})
                : new ProxerResult<string>(lResponse);
        }

        private async Task<HttpResponseMessage> GetWebRequest(Uri url, CookieContainer cookies,
            Dictionary<string, string> headers)
        {
            using (
                System.Net.Http.HttpClient lClient =
                    new System.Net.Http.HttpClient(new HttpClientHandler
                    {
                        AllowAutoRedirect = true,
                        CookieContainer = cookies ?? new CookieContainer(),
                        UseCookies = true
                    }))
            {
                lClient.Timeout = TimeSpan.FromSeconds(this._timeout);
                lClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent",
                    $"{UserAgent} {this._userAgentExtra}".TrimEnd());
                if (headers == null) return await lClient.GetAsync(url);
                foreach (KeyValuePair<string, string> header in headers)
                    lClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);

                return await lClient.GetAsync(url);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postArgs"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public Task<ProxerResult<string>> PostRequest(Uri url, IEnumerable<KeyValuePair<string, string>> postArgs,
            Senpai senpai)
        {
            return this.PostRequest(url, postArgs, new Func<string, ProxerResult>[0], senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postArgs"></param>
        /// <param name="checkFuncs"></param>
        /// <param name="senpai"></param>
        /// <param name="useMobileCookies"></param>
        /// <param name="checkLogin"></param>
        /// <param name="recursion"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task<ProxerResult<string>> PostRequest(Uri url, IEnumerable<KeyValuePair<string, string>> postArgs,
            Func<string, ProxerResult>[] checkFuncs, Senpai senpai = null, bool useMobileCookies = false,
            bool checkLogin = true, int recursion = 0, Dictionary<string, string> headers = null)
        {
            if (checkLogin && (!senpai?.IsProbablyLoggedIn ?? true))
                return
                    new ProxerResult<string>(new Exception[] {new NotLoggedInException()});

            string lResponse;

            HttpResponseMessage lResponseObject;
            try
            {
                lResponseObject =
                    await
                        this.PostWebRequest(url,
                            useMobileCookies ? senpai?.MobileLoginCookies : senpai?.LoginCookies,
                            postArgs, headers);
            }
            catch (Exception ex)
            {
                return new ProxerResult<string>(new[] {ex});
            }
            senpai?.UsedCookies();
            string lResponseString = await lResponseObject.Content.ReadAsStringAsync();

            if ((lResponseObject.StatusCode == HttpStatusCode.OK) && !string.IsNullOrEmpty(lResponseString))
                lResponse = WebUtility.HtmlDecode(lResponseString).Replace("\n", "");
            else if ((lResponseObject.StatusCode == HttpStatusCode.ServiceUnavailable) &&
                     !string.IsNullOrEmpty(lResponseString))
                return new ProxerResult<string>(new[] {new CloudflareException()});
            else
                return
                    new ProxerResult<string>(new[] {new WrongResponseException()});

            if (checkFuncs != null)
                foreach (Func<string, ProxerResult> checkFunc in checkFuncs)
                    try
                    {
                        ProxerResult lResult = checkFunc?.Invoke(lResponse) ?? new ProxerResult {Success = false};
                        if (!lResult.Success)
                            return new ProxerResult<string>(lResult.Exceptions);
                    }
                    catch
                    {
                        return new ProxerResult<string>(new Exception[0]);
                    }

            return string.IsNullOrEmpty(lResponse)
                ? new ProxerResult<string>(new Exception[] {new WrongResponseException {Response = lResponse}})
                : new ProxerResult<string>(lResponseString);
        }

        private async Task<HttpResponseMessage> PostWebRequest(Uri url, CookieContainer cookies,
            IEnumerable<KeyValuePair<string, string>> postArgs, Dictionary<string, string> headers)
        {
            using (
                System.Net.Http.HttpClient lClient =
                    new System.Net.Http.HttpClient(new HttpClientHandler
                    {
                        AllowAutoRedirect = true,
                        CookieContainer = cookies ?? new CookieContainer(),
                        UseCookies = true
                    }))
            {
                lClient.Timeout = TimeSpan.FromSeconds(this._timeout);
                lClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent",
                    $"{UserAgent} {this._userAgentExtra}".TrimEnd());
                if (headers == null) return await lClient.PostAsync(url, new FormUrlEncodedContent(postArgs));
                foreach (KeyValuePair<string, string> header in headers)
                    lClient.DefaultRequestHeaders.Add(header.Key, header.Value);

                return await lClient.PostAsync(url, new FormUrlEncodedContent(postArgs));
            }
        }
    }
}