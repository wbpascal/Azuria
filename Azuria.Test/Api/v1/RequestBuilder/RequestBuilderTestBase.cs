using System;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Requests.Builder;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    [TestFixture]
    public abstract class RequestBuilderTestBase<T> where T : class, IApiClassRequestBuilder
    {
        private readonly Random _random;

        protected RequestBuilderTestBase(Func<IProxerClient, T> requestBuilderFactory)
        {
            this._random = new Random();
            this.ProxerClient = Azuria.ProxerClient.Create(new char[32]);
            this.RequestBuilder = requestBuilderFactory.Invoke(this.ProxerClient);
        }

        public IProxerClient ProxerClient { get; set; }

        public T RequestBuilder { get; }

        protected void CheckUrl(IRequestBuilderBase builder, string apiClassName, string apiFunctionName)
        {
            Uri lUri = builder.BuildUri();
            Assert.AreEqual("https", lUri.Scheme);
            Assert.AreEqual("proxer.me", lUri.Host);
            Assert.AreEqual(
                $"{TestConstants.ProxerApiV1Path}/{apiClassName}/{apiFunctionName}", lUri.AbsolutePath
            );
        }

        public int GetRandomNumber(int max)
        {
            return this._random.Next(max);
        }

        public virtual void ProxerClientTest()
        {
            Assert.AreSame(this.ProxerClient, this.RequestBuilder.ProxerClient);
        }
    }
}