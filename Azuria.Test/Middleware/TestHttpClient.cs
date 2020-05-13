using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Requests.Http;
using Newtonsoft.Json;

namespace Azuria.Test.Middleware
{
    public class TestHttpClient : IHttpClient
    {
        public Task<IProxerResult<string>> GetRequestAsync(Uri url, IDictionary<string, string> headers = null, CancellationToken token = default)
        {
            if (url.Query.Contains("empty=1"))
                return Task.FromResult((IProxerResult<string>) new ProxerResult<string>(""));
            if (url.Query.Contains("fail=1"))
                return Task.FromResult((IProxerResult<string>) new ProxerResult<string>(new Exception("GET")));
            if (url.Query.Contains("malformed=1"))
                return Task.FromResult((IProxerResult<string>)new ProxerResult<string>("{'}"));

            var requestData = new Dictionary<string, string>()
            {
                {"method", "GET"},
                {"url", url.ToString()},
                {"headers", JsonConvert.SerializeObject(headers)}
            };

            string requestDataString =
                Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestData)));
            string dataString = url.AbsolutePath.EndsWith("withdata") ? ", 'data': {}" : "";
            string proxerApiResponse = $"{{'error': 0, 'message': '{requestDataString}'{dataString}}}";

            return Task.FromResult((IProxerResult<string>) new ProxerResult<string>(proxerApiResponse));
        }

        public Task<IProxerResult<string>> PostRequestAsync(Uri url, IEnumerable<KeyValuePair<string, string>> postArgs, IDictionary<string, string> headers = null, CancellationToken token = default)
        {
            var requestData = new Dictionary<string, string>()
            {
                {"method", "POST"},
                {"url", url.ToString()},
                {"postArgs", JsonConvert.SerializeObject(postArgs)},
                {"headers", JsonConvert.SerializeObject(headers)}
            };

            string requestDataString =
                Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestData)));
            string dataString = url.AbsolutePath.EndsWith("withdata") ? ", 'data': {}" : "";
            string proxerApiResponse = $"{{'error': 0, 'message': '{requestDataString}'{dataString}}}";

            return Task.FromResult((IProxerResult<string>) new ProxerResult<string>(proxerApiResponse));
        }
    }
}
