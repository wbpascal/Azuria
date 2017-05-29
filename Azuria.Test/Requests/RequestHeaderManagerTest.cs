using System.Collections.Generic;
using Autofac;
using Azuria.Authentication;
using Azuria.Requests;
using Azuria.Test.Core;
using Azuria.Test.Core.Helpers;
using Moq;
using Xunit;

namespace Azuria.Test.Requests
{
    public class RequestHeaderManagerTest
    {
        private readonly char[] _apiKey = ArrayHelpers.GetRandomChars(32);
        private readonly IRequestHeaderManager _headerManager;
        
        public RequestHeaderManagerTest()
        {
            IProxerClient lClient = ProxerClient.Create(this._apiKey);
            this._headerManager = lClient.Container.Resolve<IRequestHeaderManager>();
        }

        [Fact]
        public void GetHeaderTest()
        {
            Dictionary<string, string> lHeaders = this._headerManager.GetHeader();
            Assert.True(lHeaders.ContainsKey(TestConstants.ApiKeyHeaderName));
            Assert.Equal(this._apiKey, lHeaders[TestConstants.ApiKeyHeaderName]);
            Assert.False(lHeaders.ContainsKey(TestConstants.LoginTokenHeaderName));
        }
        
        [Fact]
        public void GetHeaderAuthenticatedTest()
        {
            char[] lLoginToken = ArrayHelpers.GetRandomChars(255);
            
            Mock<ILoginManager> lLoginManager = new Mock<ILoginManager>();
            lLoginManager.Setup(manager => manager.SendTokenWithNextRequest()).Returns(true);
            lLoginManager.SetupGet(manager => manager.LoginToken).Returns(lLoginToken);
            IProxerClient lClient = ProxerClient.Create(
                this._apiKey, options => options.WithCustomLoginManager(context => lLoginManager.Object)
            );

            IRequestHeaderManager lHeaderManager = lClient.Container.Resolve<IRequestHeaderManager>();
            Dictionary<string, string> lHeaders = lHeaderManager.GetHeader();
            Assert.True(lHeaders.ContainsKey(TestConstants.ApiKeyHeaderName));
            Assert.Equal(this._apiKey, lHeaders[TestConstants.ApiKeyHeaderName]);
            Assert.True(lHeaders.ContainsKey(TestConstants.LoginTokenHeaderName));
            Assert.Equal(lLoginToken, lHeaders[TestConstants.LoginTokenHeaderName]);
        }

        [Fact]
        public void ContainsAuthenticationHeadersTest()
        {
            Dictionary<string, string> lHeadersToTest = new Dictionary<string, string>();
            Assert.False(this._headerManager.ContainsAuthenticationHeaders(lHeadersToTest));
            
            lHeadersToTest.Add(TestConstants.LoginTokenHeaderName, new string((char) 0, 255));
            Assert.True(this._headerManager.ContainsAuthenticationHeaders(lHeadersToTest));
        }
    }
}