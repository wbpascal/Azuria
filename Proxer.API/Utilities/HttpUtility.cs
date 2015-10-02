using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Proxer.API.Utilities
{
    internal class HttpUtility
    {
        #region

        internal static async Task<string> GetWebRequestResponse(string url, CookieContainer cookies)
        {
            RestClient lClient = new RestClient(url)
            {
                CookieContainer = cookies,
                Encoding = Encoding.UTF8
            };
            RestRequest lRequest = new RestRequest(Method.GET);
            return System.Web.HttpUtility.HtmlDecode((await lClient.ExecuteTaskAsync(lRequest)).Content);
        }

        internal static async Task<string> PostWebRequestResponse(string url, CookieContainer cookies,
            Dictionary<string, string> postArgs)
        {
            RestClient lClient = new RestClient(url)
            {
                CookieContainer = cookies,
                Encoding = Encoding.UTF8
            };
            RestRequest lRequest = new RestRequest(Method.POST);
            postArgs.ToList().ForEach(x => lRequest.AddParameter(x.Key, x.Value));
            return System.Web.HttpUtility.HtmlDecode((await lClient.ExecuteTaskAsync(lRequest)).Content);
        }

        #endregion
    }
}