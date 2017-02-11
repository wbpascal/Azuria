using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Utilities;

namespace Azuria.Web
{
    /// <summary>
    /// Represents a class that communicates with the proxer servers.
    /// </summary>
    public class HttpClient : IHttpClient
    {
#if PORTABLE
/// <summary>
/// 
/// </summary>
        protected static readonly string UserAgent = "Azuria.Portable/" + Utility.GetAssemblyVersion(typeof(HttpClient));
#else
        /// <summary>
        /// </summary>
        protected static readonly string UserAgent = "Azuria/" + Utility.GetAssemblyVersion(typeof(HttpClient));
#endif

        private readonly System.Net.Http.HttpClient _client;

        /// <summary>
        /// </summary>
        protected Senpai Senpai { get; }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="timeout"></param>
        /// <param name="userAgentExtra"></param>
        public HttpClient(Senpai senpai, int timeout = 5000, string userAgentExtra = "")
        {
            this.Senpai = senpai;
            this._client = new System.Net.Http.HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = true,
                UseCookies = true
            }) {Timeout = TimeSpan.FromMilliseconds(timeout)};
            this._client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent",
                $"{UserAgent} {userAgentExtra}".TrimEnd());
        }

        #region Methods

        /// <summary>
        /// DO NOT dispose <see cref="Senpai" /> as this is most likely being called from a Senpai.Dispose() method.
        /// </summary>
        public virtual void Dispose()
        {
            this._client.Dispose();
        }

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public virtual async Task<IProxerResult<string>> GetRequest(Uri url, Dictionary<string, string> headers = null)
        {
            string lResponse;

            HttpResponseMessage lResponseObject;
            try
            {
                lResponseObject = await this.GetWebRequest(url, headers).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new ProxerResult<string>(new[] {ex});
            }
            string lResponseString = await lResponseObject.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseString))
                lResponse = WebUtility.HtmlDecode(lResponseString).Replace("\n", "");
            else if (lResponseObject.StatusCode == HttpStatusCode.ServiceUnavailable &&
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
            this.Senpai?.UsedCookies();
            this._client.DefaultRequestHeaders.Clear();

            if (headers == null) return await this._client.GetAsync(url).ConfigureAwait(false);
            foreach (KeyValuePair<string, string> header in headers)
                this._client.DefaultRequestHeaders.Add(header.Key, header.Value);

            return await this._client.GetAsync(url).ConfigureAwait(false);
        }

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postArgs"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public virtual async Task<IProxerResult<string>> PostRequest(Uri url,
            IEnumerable<KeyValuePair<string, string>> postArgs,
            Dictionary<string, string> headers = null)
        {
            string lResponse;

            HttpResponseMessage lResponseObject;
            try
            {
                lResponseObject = await this.PostWebRequest(url, postArgs, headers).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new ProxerResult<string>(new[] {ex});
            }
            string lResponseString = await lResponseObject.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseString))
                lResponse = WebUtility.HtmlDecode(lResponseString).Replace("\n", "");
            else if (lResponseObject.StatusCode == HttpStatusCode.ServiceUnavailable
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
            this.Senpai?.UsedCookies();
            this._client.DefaultRequestHeaders.Clear();

            if (headers == null)
                return await this._client.PostAsync(url, new FormUrlEncodedContent(postArgs)).ConfigureAwait(false);
            foreach (KeyValuePair<string, string> header in headers)
                this._client.DefaultRequestHeaders.Add(header.Key, header.Value);

            return await this._client.PostAsync(url, new FormUrlEncodedContent(postArgs)).ConfigureAwait(false);
        }

        #endregion
    }
}