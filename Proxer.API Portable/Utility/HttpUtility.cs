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
    public class HttpUtility
    {
        public class CookieResponse
        {
            public CookieResponse(string response, CookieContainer cookieContainer)
            {
                this.Response = response;
                this.Cookies = cookieContainer;
            }

            public string Response { get; private set; }
            public CookieContainer Cookies { get; private set; }
        }

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
        public static async Task<string> PostWebRequestResponseAsync(string adresse, Dictionary<string, string> postArguments)
        {
            using (HttpClient client = new HttpClient())
            {

                FormUrlEncodedContent content = new FormUrlEncodedContent(postArguments);
                HttpResponseMessage response = await client.PostAsync(adresse, content);
                return await response.Content.ReadAsStringAsync();
            }
        }
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
