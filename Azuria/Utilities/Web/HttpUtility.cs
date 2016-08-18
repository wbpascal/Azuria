using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.Utilities.Web
{
    /// <summary>
    ///     Represents a class that communicates with the proxer servers.
    /// </summary>
    public class HttpUtility
    {
        /// <summary>
        ///     The time in milliseconds that represents the timeout on requests.
        ///     Default value = 5000
        /// </summary>
        public static int Timeout = 5000;

        /// <summary>
        ///     Whether cloudflare firewall encounters should be automatically solved.
        /// </summary>
        public static bool SolveCloudflare = true;

#if PORTABLE
        [NotNull] private static readonly string UserAgent =
            "Azuria.Portable/" + typeof(HttpUtility).GetTypeInfo().Assembly.GetName().Version;
#else
        [NotNull] private static readonly string UserAgent =
            "Azuria/" + typeof(HttpUtility).GetTypeInfo().Assembly.GetName().Version;
#endif

        #region

        [ItemNotNull]
        internal static Task<ProxerResult<string>> GetResponseErrorHandling(Uri url, [CanBeNull] Senpai senpai = null)
        {
            return GetResponseErrorHandling(url, new Func<string, ProxerResult>[0], senpai);
        }

        [ItemNotNull]
        internal static async Task<ProxerResult<string>> GetResponseErrorHandling(
            [NotNull] Uri url, [CanBeNull] Func<string, ProxerResult>[] checkFuncs, [CanBeNull] Senpai senpai = null,
            bool useMobileCookies = false, bool checkLogin = true, int recursion = 0)
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
                        GetWebRequestResponse(url, useMobileCookies ? senpai?.MobileLoginCookies : senpai?.LoginCookies,
                            null);
            }
            catch (Exception ex)
            {
                return new ProxerResult<string>(new[] {ex});
            }
            senpai?.UsedCookies();
            string lResponseString = await lResponseObject.Content.ReadAsStringAsync();

            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseString))
                lResponse = WebUtility.HtmlDecode(lResponseString).Replace("\n", "");
            else if (lResponseObject.StatusCode == HttpStatusCode.ServiceUnavailable &&
                     !string.IsNullOrEmpty(lResponseString))
            {
                ProxerResult<string> lSolveResult =
                    await
                        SolveCloudflareRecursive(lResponseString, url,
                            () =>
                                GetResponseErrorHandling(url, checkFuncs, checkLogin: checkLogin, senpai: senpai,
                                    useMobileCookies: useMobileCookies, recursion: recursion + 1), useMobileCookies,
                            senpai);

                if (!lSolveResult.Success || string.IsNullOrEmpty(lSolveResult.Result?.Trim() ?? ""))
                    return new ProxerResult<string>(lSolveResult.Exceptions);

                lResponse = lSolveResult.Result;
            }
            else
                return
                    new ProxerResult<string>(new[] {new WrongResponseException()});

            if (checkFuncs != null)
                foreach (Func<string, ProxerResult> checkFunc in checkFuncs)
                {
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
                }

            return string.IsNullOrEmpty(lResponse)
                ? new ProxerResult<string>(new Exception[] {new WrongResponseException()})
                : new ProxerResult<string>(lResponse);
        }

        [ItemNotNull]
        internal static async Task<HttpResponseMessage> GetWebRequestResponse([NotNull] Uri url,
            [CanBeNull] CookieContainer cookies, [CanBeNull] Dictionary<string, string> headers)
        {
#if PORTABLE
            using (
                HttpClient lClient =
                    new HttpClient(new NativeMessageHandler
                    {
                        AllowAutoRedirect = true,
                        CookieContainer = cookies ?? new CookieContainer(),
                        UseCookies = true
                    }))
#else
            using (
                HttpClient lClient =
                    new HttpClient(new HttpClientHandler
                    {
                        AllowAutoRedirect = true,
                        CookieContainer = cookies ?? new CookieContainer(),
                        UseCookies = true
                    }))
#endif
            {
                lClient.Timeout = TimeSpan.FromSeconds(Timeout);
                lClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
                if (headers == null) return await lClient.GetAsync(url);
                foreach (KeyValuePair<string, string> header in headers)
                {
                    lClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }

                return await lClient.GetAsync(url);
            }
        }

        [ItemNotNull]
        internal static async Task<ProxerResult<string>> PostResponseErrorHandling([NotNull] Uri url,
            [NotNull] Dictionary<string, string> postArgs, [NotNull] Senpai senpai)
        {
            return await PostResponseErrorHandling(url, postArgs, new Func<string, ProxerResult>[0], senpai);
        }

        [ItemNotNull]
        internal static async Task<ProxerResult<string>> PostResponseErrorHandling(
            [NotNull] Uri url, [NotNull] Dictionary<string, string> postArgs,
            [CanBeNull] Func<string, ProxerResult>[] checkFuncs, [CanBeNull] Senpai senpai = null,
            bool useMobileCookies = false, bool checkLogin = true,
            int recursion = 0, Dictionary<string, string> header = null)
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
                        PostWebRequestResponse(url, useMobileCookies ? senpai?.MobileLoginCookies : senpai?.LoginCookies,
                            postArgs, header);
            }
            catch (Exception ex)
            {
                return new ProxerResult<string>(new[] {ex});
            }
            senpai?.UsedCookies();
            string lResponseString = await lResponseObject.Content.ReadAsStringAsync();

            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseString))
                lResponse = WebUtility.HtmlDecode(lResponseString).Replace("\n", "");
            else if (lResponseObject.StatusCode == HttpStatusCode.ServiceUnavailable &&
                     !string.IsNullOrEmpty(lResponseString))
            {
                ProxerResult<string> lSolveResult = await SolveCloudflareRecursive(lResponseString, url,
                    () =>
                        PostResponseErrorHandling(url, postArgs, checkFuncs, senpai, checkLogin: checkLogin,
                            useMobileCookies: useMobileCookies, recursion: recursion + 1, header: header),
                    useMobileCookies, senpai);

                if (!lSolveResult.Success || string.IsNullOrEmpty(lSolveResult.Result?.Trim() ?? ""))
                    return new ProxerResult<string>(lSolveResult.Exceptions);

                lResponse = lSolveResult.Result;
            }
            else
                return
                    new ProxerResult<string>(new[] {new WrongResponseException()});

            if (checkFuncs != null)
                foreach (Func<string, ProxerResult> checkFunc in checkFuncs)
                {
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
                }

            return string.IsNullOrEmpty(lResponse)
                ? new ProxerResult<string>(new Exception[] {new WrongResponseException {Response = lResponse}})
                : new ProxerResult<string>(lResponseString);
        }

        private static async Task<ProxerResult<string>> SolveCloudflareRecursive(string responseString, Uri url,
            Func<Task<ProxerResult<string>>> recursiveCall, bool useMobileCookies = false, Senpai senpai = null)
        {
            if (!SolveCloudflare)
                return new ProxerResult<string>(new[] {new CloudflareException()});
            ProxerResult<string> lSolveResult =
                CloudflareSolver.Solve(WebUtility.HtmlDecode(responseString).Replace("\n", ""), url);

            if (!lSolveResult.Success)
                return new ProxerResult<string>(new[] {new CloudflareException()});

            await Task.Delay(4000);

            HttpResponseMessage lGetResult;
            try
            {
                lGetResult =
                    await
                        PostWebRequestResponse(
                            new Uri($"{url.Scheme}://{url.Host}/cdn-cgi/l/chk_jschl?{lSolveResult.Result}"),
                            useMobileCookies ? senpai?.MobileLoginCookies : senpai?.LoginCookies,
                            new Dictionary<string, string>(), null);
            }
            catch (TaskCanceledException)
            {
                return new ProxerResult<string>(new[] {new TimeoutException()});
            }

            if (lGetResult.StatusCode != HttpStatusCode.OK)
                return new ProxerResult<string>(new[] {new CloudflareException()});

            return await recursiveCall.Invoke();
        }

        [ItemNotNull]
        internal static async Task<HttpResponseMessage> PostWebRequestResponse([NotNull] Uri url,
            [CanBeNull] CookieContainer cookies, [NotNull] Dictionary<string, string> postArgs,
            [CanBeNull] Dictionary<string, string> headers)
        {
#if PORTABLE
            using (
                HttpClient lClient =
                    new HttpClient(new NativeMessageHandler
                    {
                        AllowAutoRedirect = true,
                        CookieContainer = cookies ?? new CookieContainer(),
                        UseCookies = true
                    }))
#else
            using (
                HttpClient lClient =
                    new HttpClient(new HttpClientHandler
                    {
                        AllowAutoRedirect = true,
                        CookieContainer = cookies ?? new CookieContainer(),
                        UseCookies = true
                    }))
#endif
            {
                lClient.Timeout = TimeSpan.FromSeconds(Timeout);
                lClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
                if (headers == null) return await lClient.PostAsync(url, new FormUrlEncodedContent(postArgs));
                foreach (KeyValuePair<string, string> header in headers)
                {
                    lClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                return await lClient.PostAsync(url, new FormUrlEncodedContent(postArgs));
            }
        }

        #endregion
    }
}