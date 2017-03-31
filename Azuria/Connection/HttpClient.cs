﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Helpers;

namespace Azuria.Connection
{
    /// <summary>
    /// Represents a class that communicates with the proxer servers.
    /// </summary>
    public class HttpClient : IHttpClient
    {
        /// <summary>
        /// 
        /// </summary>
        protected static readonly string UserAgent =
            "Azuria/" + VersionHelper.GetAssemblyVersion(typeof(HttpClient));

        private readonly System.Net.Http.HttpClient _client;

        /// <summary>
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="userAgentExtra"></param>
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

        #region Methods

        /// <summary>
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
        public virtual async Task<IProxerResult<string>> GetRequestAsync(
            Uri url,
            Dictionary<string, string> headers = null)
        {
            string lResponse;

            HttpResponseMessage lResponseObject;
            try
            {
                lResponseObject = await this.GetWebRequestAsync(url, headers).ConfigureAwait(false);
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

        private async Task<HttpResponseMessage> GetWebRequestAsync(Uri url, Dictionary<string, string> headers)
        {
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
        public virtual async Task<IProxerResult<string>> PostRequestAsync(
            Uri url,
            IEnumerable<KeyValuePair<string, string>> postArgs, Dictionary<string, string> headers = null)
        {
            string lResponse;

            HttpResponseMessage lResponseObject;
            try
            {
                lResponseObject = await this.PostWebRequestAsync(url, postArgs, headers).ConfigureAwait(false);
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
            Uri url,
            IEnumerable<KeyValuePair<string, string>> postArgs, Dictionary<string, string> headers)
        {
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