using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Proxer.API.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpUtility
    {
        /// <summary>
        /// 
        /// </summary>
        public class CookieResponse
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="response"></param>
            /// <param name="cookieContainer"></param>
            public CookieResponse(string response, CookieContainer cookieContainer)
            {
                this.Response = response;
                this.Cookies = cookieContainer;
            }

            /// <summary>
            /// 
            /// </summary>
            public string Response { get; private set; }
            /// <summary>
            /// 
            /// </summary>
            public CookieContainer Cookies { get; private set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adresse"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="postArguments"></param>
        /// <returns></returns>
        public static async Task<CookieResponse> PostWebRequestResponseAsync(Uri adresse, CookieContainer cookieContainer, Dictionary<string, string> postArguments)
        {
            using (HttpClientHandler handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            {
                using (HttpClient client = new HttpClient(handler))
                {

                    FormUrlEncodedContent content = new FormUrlEncodedContent(postArguments);
                    HttpResponseMessage response = await client.PostAsync(adresse, content);
                    return new CookieResponse(await response.Content.ReadAsStringAsync(), handler.CookieContainer);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adresse"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="postArguments"></param>
        /// <returns></returns>
        public static async Task<string> PostWebRequestResponseAsync(string adresse, CookieContainer cookieContainer, Dictionary<string, string> postArguments)
        {
            using (HttpClientHandler handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    FormUrlEncodedContent content = new FormUrlEncodedContent(postArguments);
                    HttpResponseMessage response = await client.PostAsync(adresse, content);
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adresse"></param>
        /// <param name="postArguments"></param>
        /// <returns></returns>
        public static async Task<string> PostWebRequestResponseAsync(string adresse, Dictionary<string, string> postArguments)
        {
            using (HttpClient client = new HttpClient())
            {

                FormUrlEncodedContent content = new FormUrlEncodedContent(postArguments);
                HttpResponseMessage response = await client.PostAsync(adresse, content);
                return await response.Content.ReadAsStringAsync();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adresse"></param>
        /// <param name="cookieContainer"></param>
        /// <returns></returns>
        public static async Task<string> GetWebRequestResponseAsync(string adresse, CookieContainer cookieContainer)
        {
            using (HttpClientHandler handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    HttpResponseMessage response = await client.GetAsync(adresse);
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
