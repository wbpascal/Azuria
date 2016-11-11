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
    /// Represents a class that communicates with the proxer servers.
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

        private readonly Senpai _senpai;
        private readonly System.Net.Http.HttpClient _client;

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="timeout"></param>
        /// <param name="userAgentExtra"></param>
        public HttpClient(Senpai senpai, int timeout = 5000, string userAgentExtra = "")
        {
            this._senpai = senpai;
            this._client = new System.Net.Http.HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = true,
                CookieContainer = senpai?.LoginCookies ?? new CookieContainer(),
                UseCookies = true
            }) {Timeout = TimeSpan.FromMilliseconds(timeout)};
            this._client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent",
                $"{UserAgent} {userAgentExtra}".TrimEnd());
        }

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task<IProxerResult<string>> GetRequest(Uri url, Dictionary<string, string> headers = null)
        {
            string lResponse;

            HttpResponseMessage lResponseObject;
            try
            {
                lResponseObject = await this.GetWebRequest(url, headers);
            }
            catch (Exception ex)
            {
                return new ProxerResult<string>(new[] {ex});
            }
            string lResponseString = await lResponseObject.Content.ReadAsStringAsync();

            if ((lResponseObject.StatusCode == HttpStatusCode.OK) && !string.IsNullOrEmpty(lResponseString))
                lResponse = WebUtility.HtmlDecode(lResponseString).Replace("\n", "");
            else if ((lResponseObject.StatusCode == HttpStatusCode.ServiceUnavailable) &&
                     !string.IsNullOrEmpty(lResponseString))
                return new ProxerResult<string>(new[] {new CloudflareException()});
            else
                return
                    new ProxerResult<string>(new[] {new WrongResponseException()});

            return string.IsNullOrEmpty(lResponse)
                ? new ProxerResult<string>(new Exception[] {new WrongResponseException()})
                : new ProxerResult<string>(lResponse);
        }

        private async Task<HttpResponseMessage> GetWebRequest(Uri url, Dictionary<string, string> headers)
        {
            this._senpai?.UsedCookies();
            this._client.DefaultRequestHeaders.Clear();

            if (headers == null) return await this._client.GetAsync(url);
            foreach (KeyValuePair<string, string> header in headers)
                this._client.DefaultRequestHeaders.Add(header.Key, header.Value);

            return await this._client.GetAsync(url);
        }

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postArgs"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task<IProxerResult<string>> PostRequest(Uri url, IEnumerable<KeyValuePair<string, string>> postArgs,
            Dictionary<string, string> headers = null)
        {
            string lResponse;

            HttpResponseMessage lResponseObject;
            try
            {
                lResponseObject = await this.PostWebRequest(url, postArgs, headers);
            }
            catch (Exception ex)
            {
                return new ProxerResult<string>(new[] {ex});
            }
            string lResponseString = await lResponseObject.Content.ReadAsStringAsync();

            if ((lResponseObject.StatusCode == HttpStatusCode.OK) && !string.IsNullOrEmpty(lResponseString))
                lResponse = WebUtility.HtmlDecode(lResponseString).Replace("\n", "");
            else if ((lResponseObject.StatusCode == HttpStatusCode.ServiceUnavailable)
                     && !string.IsNullOrEmpty(lResponseString))
                return new ProxerResult<string>(new[] {new CloudflareException()});
            else
                return new ProxerResult<string>(new[] {new WrongResponseException()});

            return string.IsNullOrEmpty(lResponse)
                ? new ProxerResult<string>(new Exception[] {new WrongResponseException {Response = lResponse}})
                : new ProxerResult<string>(lResponseString);
        }

        private async Task<HttpResponseMessage> PostWebRequest(Uri url,
            IEnumerable<KeyValuePair<string, string>> postArgs, Dictionary<string, string> headers)
        {
            this._senpai?.UsedCookies();
            this._client.DefaultRequestHeaders.Clear();

            if (headers == null) return await this._client.PostAsync(url, new FormUrlEncodedContent(postArgs));
            foreach (KeyValuePair<string, string> header in headers)
                this._client.DefaultRequestHeaders.Add(header.Key, header.Value);

            return await this._client.PostAsync(url, new FormUrlEncodedContent(postArgs));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this._client.Dispose();
        }
    }
}