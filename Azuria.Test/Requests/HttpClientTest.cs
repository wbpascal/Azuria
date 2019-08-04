using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Requests.Http;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Azuria.Test.Requests
{
    [TestFixture]
    public class HttpClientTest
    {
        private readonly IHttpClient _httpClient;

        public HttpClientTest()
        {
            this._httpClient = new HttpClient();
        }

        [Test]
        public async Task GetRequestAsyncCancelTokenTest()
        {
            //Start request and cancel after 1 ms
            IProxerResult<string> lResult =
                await this._httpClient.GetRequestAsync(
                    new Uri("https://httpbin.org/get"), token: new CancellationTokenSource(1).Token
                );
            Assert.False(lResult.Success);
            Assert.True(lResult.Exceptions.Any(exception => exception.GetType() == typeof(TaskCanceledException)));
        }

        [Test]
        public async Task GetRequestAsyncTest()
        {
            Dictionary<string, string> lHeaders = new Dictionary<string, string> {{"Header-Key", "headerValue"}};
            IProxerResult<string> lResult =
                await this._httpClient.GetRequestAsync(new Uri("https://httpbin.org/get?test=value"), lHeaders);
            Assert.True(lResult.Success, lResult.Result);
            Assert.IsEmpty(lResult.Exceptions);
            Assert.IsNotEmpty(lResult.Result);

            JObject lJsonObject = JObject.Parse(lResult.Result);
            Assert.AreEqual("value", lJsonObject["args"]["test"].Value<string>());
            Assert.AreEqual("headerValue", lJsonObject["headers"]["Header-Key"].Value<string>());
        }

        [Test]
        public async Task PostRequestAsyncCancelTokenTest()
        {
            //Start request and cancel after 1 ms
            IProxerResult<string> lResult =
                await this._httpClient.PostRequestAsync(
                    new Uri("https://httpbin.org/post"), new KeyValuePair<string, string>[0],
                    token: new CancellationTokenSource(1).Token
                );
            Assert.False(lResult.Success);
            Assert.True(lResult.Exceptions.Any(exception => exception.GetType() == typeof(TaskCanceledException)));
        }

        [Test]
        public async Task PostRequestAsyncTest()
        {
            Dictionary<string, string> lHeaders = new Dictionary<string, string> {{"Header-Key", "headerValue"}};
            Dictionary<string, string> lPostArgs = new Dictionary<string, string> {{"postKey", "postValue"}};
            IProxerResult<string> lResult =
                await this._httpClient.PostRequestAsync(
                    new Uri("https://httpbin.org/post?test=value"), lPostArgs, lHeaders
                );
            Assert.True(lResult.Success);
            Assert.IsEmpty(lResult.Exceptions);
            Assert.IsNotEmpty(lResult.Result);

            JObject lJsonObject = JObject.Parse(lResult.Result);
            Assert.AreEqual("value", lJsonObject["args"]["test"].Value<string>());
            Assert.AreEqual("headerValue", lJsonObject["headers"]["Header-Key"].Value<string>());
            Assert.AreEqual("postValue", lJsonObject["form"]["postKey"].Value<string>());
        }
    }
}