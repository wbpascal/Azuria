using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Proxer.API.Utilities.Net
{
    internal class HttpUtility
    {
        #region

        internal static async Task<IRestResponse> GetWebRequestResponse(string url, CookieContainer cookies)
        {
            RestClient lClient = new RestClient(url)
            {
                CookieContainer = cookies,
                Encoding = Encoding.UTF8
            };
            RestRequest lRequest = new RestRequest(Method.GET);
            return await lClient.ExecuteTaskAsync(lRequest);
        }

        internal static async Task<IRestResponse> PostWebRequestResponse(string url, CookieContainer cookies,
            Dictionary<string, string> postArgs)
        {
            RestClient lClient = new RestClient(url)
            {
                CookieContainer = cookies,
                Encoding = Encoding.UTF8
            };
            RestRequest lRequest = new RestRequest(Method.POST);
            postArgs.ToList().ForEach(x => lRequest.AddParameter(x.Key, x.Value));
            return await lClient.ExecuteTaskAsync(lRequest);
        }

        #endregion
    }
}