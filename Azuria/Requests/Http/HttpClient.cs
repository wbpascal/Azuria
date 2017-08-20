using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Helpers;

namespace Azuria.Requests.Http
{
    /// <summary>
    /// Represents a class that sends http/s requests.
    /// </summary>
    public class HttpClient : IHttpClient, IDisposable
    {
        /// <summary>
        /// The user-agent that is send with each request.
        /// </summary>
        protected static readonly string UserAgent =
            "Azuria/" + VersionHelper.GetAssemblyVersion(typeof(HttpClient));

        private readonly System.Net.Http.HttpClient _client;

        /// <summary>
        /// Creates a new instance of <see cref="HttpClient" />.
        /// </summary>
        /// <param name="timeout">Optional. The timeout of each request send with this http client.</param>
        /// <param name="userAgentExtra">
        /// Optional. A string which is appended to the end of the user-agent that is send with each request.
        /// </param>
        public HttpClient(int timeout = 5000, string userAgentExtra = "")
        {
            this._client = new System.Net.Http.HttpClient(
                new HttpClientHandler
                {
                    AllowAutoRedirect = true,
                    UseCookies = true
                }) {Timeout = TimeSpan.FromMilliseconds(timeout)};
            this._client.DefaultRequestHeaders.TryAddWithoutValidation(
                "User-Agent",
                $"{UserAgent} {userAgentExtra}".TrimEnd());
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            this._client.Dispose();
        }

        /// <inheritdoc />
        public virtual async Task<IProxerResult<string>> GetRequestAsync(
            Uri url, Dictionary<string, string> headers = null, CancellationToken token = default(CancellationToken))
        {
            string lResponse;

            HttpResponseMessage lResponseObject;
            try
            {
                lResponseObject = await this.GetWebRequestAsync(url, headers, token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new ProxerResult<string>(ex);
            }
            string lResponseString = await lResponseObject.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseString))
                lResponse = WebUtility.HtmlDecode(lResponseString).Replace("\n", "");
            else if (lResponseObject.StatusCode == HttpStatusCode.ServiceUnavailable &&
                     !string.IsNullOrEmpty(lResponseString))
                return new ProxerResult<string>(new[] {new CloudflareException()});
            else
                return
                    new ProxerResult<string>(new[] {new InvalidResponseException()});

            return string.IsNullOrEmpty(lResponse)
                       ? new ProxerResult<string>(new Exception[] {new InvalidResponseException()})
                       : new ProxerResult<string>(lResponse);
        }

        private async Task<HttpResponseMessage> GetWebRequestAsync(
            Uri url, Dictionary<string, string> headers, CancellationToken token)
        {
            this._client.DefaultRequestHeaders.Clear();

            if (headers == null) return await this._client.GetAsync(url, token).ConfigureAwait(false);
            foreach (KeyValuePair<string, string> header in headers)
                this._client.DefaultRequestHeaders.Add(header.Key, header.Value);

            return await this._client.GetAsync(url, token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public virtual async Task<IProxerResult<string>> PostRequestAsync(
            Uri url, IEnumerable<KeyValuePair<string, string>> postArgs, Dictionary<string, string> headers = null,
            CancellationToken token = default(CancellationToken))
        {
            string lResponse;

            HttpResponseMessage lResponseObject;
            try
            {
                lResponseObject = await this.PostWebRequestAsync(url, postArgs, headers, token)
                                      .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new ProxerResult<string>(ex);
            }
            string lResponseString = await lResponseObject.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseString))
                lResponse = WebUtility.HtmlDecode(lResponseString).Replace("\n", "");
            else if (lResponseObject.StatusCode == HttpStatusCode.ServiceUnavailable
                     && !string.IsNullOrEmpty(lResponseString))
                return new ProxerResult<string>(new[] {new CloudflareException()});
            else
                return new ProxerResult<string>(new[] {new InvalidResponseException()});

            return string.IsNullOrEmpty(lResponse)
                       ? new ProxerResult<string>(new Exception[] {new InvalidResponseException {Response = lResponse}})
                       : new ProxerResult<string>(lResponseString);
        }

        private async Task<HttpResponseMessage> PostWebRequestAsync(
            Uri url, IEnumerable<KeyValuePair<string, string>> postArgs,
            Dictionary<string, string> headers, CancellationToken token)
        {
            this._client.DefaultRequestHeaders.Clear();

            if (headers == null)
                return await this._client.PostAsync(url, new FormUrlEncodedContent(postArgs), token)
                           .ConfigureAwait(false);
            foreach (KeyValuePair<string, string> header in headers)
                this._client.DefaultRequestHeaders.Add(header.Key, header.Value);

            return await this._client.PostAsync(url, new FormUrlEncodedContent(postArgs), token)
                       .ConfigureAwait(false);
        }
    }
}