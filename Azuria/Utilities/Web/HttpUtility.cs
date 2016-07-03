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
    ///     Represents a class that communicates with the server via HTTP and HTTPS.
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
        internal static async Task<ProxerResult<string>> GetResponseErrorHandling(Uri url, [NotNull] Senpai senpai)
        {
            return await GetResponseErrorHandling(url, null, senpai);
        }

        [ItemNotNull]
        internal static async Task<ProxerResult<string>> GetResponseErrorHandling([NotNull] Uri url,
            [CanBeNull] CookieContainer loginCookies, [NotNull] Senpai senpai)
        {
            return
                await
                    GetResponseErrorHandling(url, loginCookies, senpai, new Func<string, ProxerResult>[0]);
        }

        [ItemNotNull]
        internal static async Task<ProxerResult<string>> GetResponseErrorHandling([NotNull] Uri url,
            [CanBeNull] CookieContainer loginCookies, [NotNull] Senpai senpai,
            [CanBeNull] Func<string, ProxerResult>[] checkFuncs)
        {
            ProxerResult<Tuple<string, CookieContainer>> lResult =
                await
                    GetResponseErrorHandling(url, loginCookies, senpai, checkFuncs, loginCookies == null);

            return lResult.Success && lResult.Result != null
                ? new ProxerResult<string>(lResult.Result.Item1)
                : new ProxerResult<string>(lResult.Exceptions);
        }

        [ItemNotNull]
        internal static async Task<ProxerResult<Tuple<string, CookieContainer>>> GetResponseErrorHandling(
            [NotNull] Uri url, [CanBeNull] CookieContainer loginCookies, [NotNull] Senpai senpai,
            [CanBeNull] Func<string, ProxerResult>[] checkFuncs, bool checkLogin, int recursion = 0)
        {
            if (checkLogin && loginCookies != null && !senpai.IsLoggedIn)
                return
                    new ProxerResult<Tuple<string, CookieContainer>>(new Exception[] {new NotLoggedInException()});

            loginCookies = loginCookies ?? new CookieContainer();
            string lResponse;

            HttpResponseMessage lResponseObject;
            try
            {
                lResponseObject =
                    await GetWebRequestResponse(url, loginCookies, null);
            }
            catch (Exception ex)
            {
                return new ProxerResult<Tuple<string, CookieContainer>>(new[] {ex});
            }
            string lResponseString = await lResponseObject.Content.ReadAsStringAsync();

            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseString))
                lResponse = WebUtility.HtmlDecode(lResponseString).Replace("\n", "");
            else if (lResponseObject.StatusCode == HttpStatusCode.ServiceUnavailable &&
                     !string.IsNullOrEmpty(lResponseString))
            {
                if (!SolveCloudflare)
                    return new ProxerResult<Tuple<string, CookieContainer>>(new[] {new CloudflareException()});
                ProxerResult<string> lSolveResult =
                    CloudflareSolver.Solve(WebUtility.HtmlDecode(lResponseString).Replace("\n", ""), url);

                if (!lSolveResult.Success)
                    return new ProxerResult<Tuple<string, CookieContainer>>(new[] {new CloudflareException()});

                await Task.Delay(4000);

                HttpResponseMessage lGetResult;
                try
                {
                    lGetResult =
                        await
                            PostWebRequestResponse(
                                new Uri($"{url.Scheme}://{url.Host}/cdn-cgi/l/chk_jschl?{lSolveResult.Result}"),
                                loginCookies, new Dictionary<string, string>(), null);
                }
                catch (TaskCanceledException)
                {
                    return new ProxerResult<Tuple<string, CookieContainer>>(new[] {new TimeoutException()});
                }

                if (lGetResult.StatusCode != HttpStatusCode.OK)
                    return new ProxerResult<Tuple<string, CookieContainer>>(new[] {new CloudflareException()});

                return
                    await
                        GetResponseErrorHandling(url, loginCookies, senpai, checkFuncs, checkLogin,
                            recursion + 1);
            }
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

            if (string.IsNullOrEmpty(lResponse))
                return
                    new ProxerResult<Tuple<string, CookieContainer>>(new Exception[]
                    {new WrongResponseException {Response = lResponse}});

            return
                new ProxerResult<Tuple<string, CookieContainer>>(
                    new Tuple<string, CookieContainer>(lResponse, loginCookies));
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
                        CookieContainer = cookies,
                        UseCookies = true
                    }))
#else
            using (
                HttpClient lClient =
                    new HttpClient(new HttpClientHandler
                    {
                        AllowAutoRedirect = true,
                        CookieContainer = cookies,
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
            return await PostResponseErrorHandling(url, postArgs, null, senpai);
        }

        [ItemNotNull]
        internal static async Task<ProxerResult<string>> PostResponseErrorHandling([NotNull] Uri url,
            [NotNull] Dictionary<string, string> postArgs, [CanBeNull] CookieContainer loginCookies,
            [NotNull] Senpai senpai)
        {
            return
                await
                    PostResponseErrorHandling(url, postArgs, loginCookies, senpai,
                        new Func<string, ProxerResult>[0]);
        }

        [ItemNotNull]
        internal static async Task<ProxerResult<string>> PostResponseErrorHandling([NotNull] Uri url,
            [NotNull] Dictionary<string, string> postArgs, [CanBeNull] CookieContainer loginCookies,
            [NotNull] Senpai senpai, [CanBeNull] Func<string, ProxerResult>[] checkFuncs)
        {
            ProxerResult<Tuple<string, CookieContainer>> lResult =
                await
                    PostResponseErrorHandling(url, postArgs, loginCookies, senpai, checkFuncs,
                        loginCookies == null);

            return lResult.Success && lResult.Result != null
                ? new ProxerResult<string>(lResult.Result.Item1)
                : new ProxerResult<string>(lResult.Exceptions);
        }

        [ItemNotNull]
        internal static async Task<ProxerResult<Tuple<string, CookieContainer>>> PostResponseErrorHandling(
            [NotNull] Uri url, [NotNull] Dictionary<string, string> postArgs,
            [CanBeNull] CookieContainer loginCookies, [NotNull] Senpai senpai,
            [CanBeNull] Func<string, ProxerResult>[] checkFuncs, bool checkLogin, int recursion = 0)
        {
            if (checkLogin && loginCookies != null && !senpai.IsLoggedIn)
                return
                    new ProxerResult<Tuple<string, CookieContainer>>(new Exception[] {new NotLoggedInException()});

            loginCookies = loginCookies ?? new CookieContainer();
            string lResponse;

            HttpResponseMessage lResponseObject;
            try
            {
                lResponseObject =
                    await PostWebRequestResponse(url, loginCookies, postArgs, null);
            }
            catch (Exception ex)
            {
                return new ProxerResult<Tuple<string, CookieContainer>>(new[] {ex});
            }
            string lResponseString = await lResponseObject.Content.ReadAsStringAsync();

            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseString))
                lResponse = WebUtility.HtmlDecode(lResponseString).Replace("\n", "");
            else if (lResponseObject.StatusCode == HttpStatusCode.ServiceUnavailable &&
                     !string.IsNullOrEmpty(lResponseString))
            {
                if (!SolveCloudflare)
                    return new ProxerResult<Tuple<string, CookieContainer>>(new[] {new CloudflareException()});
                ProxerResult<string> lSolveResult =
                    CloudflareSolver.Solve(WebUtility.HtmlDecode(lResponseString).Replace("\n", ""), url);

                if (!lSolveResult.Success)
                    return new ProxerResult<Tuple<string, CookieContainer>>(new[] {new CloudflareException()});

                await Task.Delay(4000);

                HttpResponseMessage lGetResult;
                try
                {
                    lGetResult =
                        await
                            PostWebRequestResponse(
                                new Uri($"{url.Scheme}://{url.Host}/cdn-cgi/l/chk_jschl?{lSolveResult.Result}"),
                                loginCookies, new Dictionary<string, string>(), null);
                }
                catch (TaskCanceledException)
                {
                    return new ProxerResult<Tuple<string, CookieContainer>>(new[] {new TimeoutException()});
                }

                if (lGetResult.StatusCode != HttpStatusCode.OK)
                    return new ProxerResult<Tuple<string, CookieContainer>>(new[] {new CloudflareException()});

                return
                    await
                        PostResponseErrorHandling(url, postArgs, loginCookies, senpai, checkFuncs,
                            checkLogin, recursion + 1);
            }
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
                        return new ProxerResult<Tuple<string, CookieContainer>>(new Exception[0])
                        {
                            Success = false
                        };
                    }
                }

            if (string.IsNullOrEmpty(lResponse))
                return
                    new ProxerResult<Tuple<string, CookieContainer>>(new Exception[]
                    {new WrongResponseException {Response = lResponse}});

            return
                new ProxerResult<Tuple<string, CookieContainer>>(
                    new Tuple<string, CookieContainer>(lResponse, loginCookies));
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
                        CookieContainer = cookies,
                        UseCookies = true
                    }))
#else
            using (
                HttpClient lClient =
                    new HttpClient(new HttpClientHandler
                    {
                        AllowAutoRedirect = true,
                        CookieContainer = cookies,
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