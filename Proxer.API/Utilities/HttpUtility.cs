using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using RestSharp;

namespace Proxer.API.Utilities
{
    /// <summary>
    /// </summary>
    internal class HttpUtility
    {
        #region

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookies"></param>
        /// <returns></returns>
        internal static string GetWebRequestResponse(string url, CookieContainer cookies)
        {
            RestClient lClient = new RestClient(url)
            {
                CookieContainer = cookies,
                Encoding = Encoding.UTF8
            };
            RestRequest lRequest = new RestRequest(Method.GET);
            return System.Web.HttpUtility.HtmlDecode(lClient.Execute(lRequest).Content);
        }

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookies"></param>
        /// <param name="postArgs"></param>
        /// <returns></returns>
        internal static string PostWebRequestResponse(string url, CookieContainer cookies,
            Dictionary<string, string> postArgs)
        {
            RestClient lClient = new RestClient(url)
            {
                CookieContainer = cookies,
                Encoding = Encoding.UTF8
            };
            RestRequest lRequest = new RestRequest(Method.POST);
            postArgs.ToList().ForEach(x => lRequest.AddParameter(x.Key, x.Value));
            return System.Web.HttpUtility.HtmlDecode(lClient.Execute(lRequest).Content);
        }

        #endregion
    }
}