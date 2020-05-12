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
            var requestData = new Dictionary<string, string>()
            {
                {"url", url.ToString()},
                {"headers", JsonConvert.SerializeObject(headers)}
            };

            string requestDataString =
                Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestData)));
            string proxerApiResponse = $"{{'error': 0, 'message': '{requestDataString}'}}";

            return Task.FromResult((IProxerResult<string>) new ProxerResult<string>(proxerApiResponse));
        }

        public Task<IProxerResult<string>> PostRequestAsync(Uri url, IEnumerable<KeyValuePair<string, string>> postArgs, IDictionary<string, string> headers = null, CancellationToken token = default)
        {
            var requestData = new Dictionary<string, string>()
            {
                {"url", url.ToString()},
                {"postArgs", JsonConvert.SerializeObject(postArgs)},
                {"headers", JsonConvert.SerializeObject(headers)}
            };

            string requestDataString =
                Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestData)));
            string proxerApiResponse = $"{{'error': 0, 'message': '{requestDataString}'}}";

            return Task.FromResult((IProxerResult<string>) new ProxerResult<string>(proxerApiResponse));
        }
    }
}
