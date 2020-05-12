using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azuria.Api.Builder;
using Azuria.ErrorHandling;
using Azuria.Middleware;
using Azuria.Requests;
using Azuria.Requests.Builder;
using Azuria.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Azuria.Test.Middleware
{
    public class HttpJsonRequestMiddlewareTest
    {
        private readonly IApiRequestBuilder _apiRequestBuilder;

        public HttpJsonRequestMiddlewareTest()
        {
            IProxerClient client = ProxerClient.Create(new char[32]);
            _apiRequestBuilder = client.CreateRequest();
        }

        [Test]
        public void Constructor_SetsPropertiesTest()
        {
            var httpClient = new TestHttpClient();
            var jsonSerializer = new TestJsonDeserializer();

            var middleware = new HttpJsonRequestMiddleware(httpClient, jsonSerializer);
            Assert.AreSame(httpClient, middleware.HttpClient);
            Assert.AreSame(jsonSerializer, middleware.JsonDeserializer);
        }

        [Test]
        public async Task Invoke_MakesCorrectGetRequestTest()
        {
            var middleware = new HttpJsonRequestMiddleware(new TestHttpClient(), new JsonDeserializer());
            var uri = new Uri("https://proxer.me/api/v1/user/userinfo?uid=42");
            var header = new Dictionary<string, string>() {{"test", "key123"}};
            IRequestBuilder request = _apiRequestBuilder.FromUrl(uri).WithHeader(header);

            IProxerResult result = await middleware.Invoke(request, null);
            Assert.True(result.Success);
            Assert.IsAssignableFrom<ProxerApiResponse>(result);

            var proxerApiResponse = (ProxerApiResponse) result;
            Assert.IsNotEmpty(proxerApiResponse.Message);
            var getData =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    Encoding.UTF8.GetString(Convert.FromBase64String(proxerApiResponse.Message)));
            Assert.AreEqual(uri.ToString(), getData["url"]);
            Assert.AreEqual(JsonConvert.SerializeObject(header), getData["headers"]);
            Assert.False(getData.ContainsKey("postData"));
        }

        [Test]
        public async Task Invoke_MakesCorrectPostRequestTest()
        {
            var middleware = new HttpJsonRequestMiddleware(new TestHttpClient(), new JsonDeserializer());
            var uri = new Uri("https://proxer.me/api/v1/user/userinfo?uid=42");
            var header = new Dictionary<string, string> {{"test", "key123"}};
            var postArgs = new Dictionary<string, string> {{"postArg", "test"}};
            IRequestBuilder request =
                _apiRequestBuilder.FromUrl(uri).WithHeader(header).WithPostParameter(postArgs);

            IProxerResult result = await middleware.Invoke(request, null);
            Assert.True(result.Success);
            Assert.IsAssignableFrom<ProxerApiResponse>(result);

            var proxerApiResponse = (ProxerApiResponse) result;
            Assert.IsNotEmpty(proxerApiResponse.Message);
            var postData =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    Encoding.UTF8.GetString(Convert.FromBase64String(proxerApiResponse.Message)));
            Assert.AreEqual(uri.ToString(), postData["url"]);
            Assert.AreEqual(JsonConvert.SerializeObject(header), postData["headers"]);
            Assert.AreEqual(JsonConvert.SerializeObject(postArgs.ToList()), postData["postArgs"]);
        }

        [Test]
        public void Invoke_UsesCustomJsonSerializerTest()
        {
            // This test is needed because the custom serializer is not needed for the other tests
            // and we want to make sure that it is used
            var middleware = new HttpJsonRequestMiddleware(new TestHttpClient(), new TestJsonDeserializer());
            IRequestBuilder request = _apiRequestBuilder.FromUrl(new Uri("https://proxer.me/api/v1"));
            var exception = Assert.ThrowsAsync<Exception>(async () => await middleware.Invoke(request, null));
            Assert.AreEqual(TestJsonDeserializer.TEST_MESSAGE, exception.Message);
        }

        [Test]
        public void InvokeWithResult_UsesCustomJsonSerializerTest()
        {
            // See Invoke_UsesCustomJsonSerializer
            var middleware = new HttpJsonRequestMiddleware(new TestHttpClient(), new TestJsonDeserializer());
            IRequestBuilderWithResult<object> request = _apiRequestBuilder
                .FromUrl(new Uri("https://proxer.me/api/v1")).WithResult<object>();
            var exception = Assert.ThrowsAsync<Exception>(async () => await middleware.InvokeWithResult(request, null));
            Assert.AreEqual(TestJsonDeserializer.TEST_MESSAGE, exception.Message);
        }
    }
}