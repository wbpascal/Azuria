using System;
using Autofac;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Requests.Builder;
using Azuria.Test.Core;
using Xunit;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    public class RequestBuilderTestBase<T> where T : class, IApiClassRequestBuilder
    {
        private readonly Random _random;

        public RequestBuilderTestBase()
        {
            this._random = new Random();
            this.ProxerClient = Azuria.ProxerClient.Create(new char[32]);
            this.RequestBuilder = this.ProxerClient.Container.Resolve<T>();
        }

        public IProxerClient ProxerClient { get; set; }

        public T RequestBuilder { get; }

        protected void CheckUrl(IRequestBuilderBase builder, string apiClassName, string apiFunctionName)
        {
            Uri lUri = builder.BuildUri();
            Assert.Equal("https", lUri.Scheme);
            Assert.Equal("proxer.me", lUri.Host);
            Assert.Equal(
                $"{TestConstants.ProxerApiV1Path}/{apiClassName}/{apiFunctionName}", lUri.AbsolutePath
            );
        }

        public int GetRandomNumber(int max)
        {
            return this._random.Next(max);
        }

        public virtual void ProxerClientTest()
        {
            Assert.Same(this.ProxerClient, this.RequestBuilder.ProxerClient);
        }
    }
}