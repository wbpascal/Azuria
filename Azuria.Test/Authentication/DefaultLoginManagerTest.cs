using System;
using System.Threading;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.Input.User;
using Azuria.Authentication;
using Azuria.ErrorHandling;
using Azuria.Middleware;
using Azuria.Requests;
using Azuria.Requests.Builder;
using Azuria.Test.Core.Helpers;
using Moq;
using NUnit.Framework;

namespace Azuria.Test.Authentication
{
    [TestFixture]
    public class DefaultLoginManagerTest
    {
        private const string LoginTokenHeaderName = "proxer-api-token";

        [Test]
        public void AddAuthenticationInformation_AddsCorrectInfosTest()
        {
            IProxerClient client = ProxerClient.Create(new char[32]);

            char[] loginToken = RandomHelper.GetRandomString(255).ToCharArray();
            var defaultLoginManager = new DefaultLoginManager(loginToken);

            IRequestBuilder request = new RequestBuilder(new Uri("http://proxer.me/api"), client).WithLoginCheck();
            bool added = defaultLoginManager.AddAuthenticationInformation(request);
            Assert.True(added);
            Assert.True(request.Headers.ContainsKey(LoginTokenHeaderName));
            Assert.AreEqual(loginToken.ToString(), request.Headers[LoginTokenHeaderName]);
        }

        [Test]
        public void AddAuthenticationInformation_SkipsIfLoginNotNeededTest()
        {
            IProxerClient client = ProxerClient.Create(new char[32]);

            char[] loginToken = RandomHelper.GetRandomString(255).ToCharArray();
            var defaultLoginManager = new DefaultLoginManager(loginToken);

            IRequestBuilder request = new RequestBuilder(new Uri("http://proxer.me/api"), client).WithLoginCheck(false);
            bool added = defaultLoginManager.AddAuthenticationInformation(request);
            Assert.False(added);
            Assert.False(request.Headers.ContainsKey(LoginTokenHeaderName));
        }

        [Test]
        public void AddAuthenticationInformation_SkipsIfRequestContainsAuthInfosTest()
        {
            IProxerClient client = ProxerClient.Create(new char[32]);

            char[] loginToken = RandomHelper.GetRandomString(255).ToCharArray();
            var defaultLoginManager = new DefaultLoginManager(loginToken);

            IRequestBuilder request =
                new RequestBuilder(new Uri("http://proxer.me/api"), client)
                    .WithLoginCheck(false)
                    .WithHeader(LoginTokenHeaderName, "token");
            bool added = defaultLoginManager.AddAuthenticationInformation(request);
            Assert.False(added);
            Assert.True(request.Headers.ContainsKey(LoginTokenHeaderName));
            Assert.AreEqual("token", request.Headers[LoginTokenHeaderName]);
        }

        [Test]
        public void AddAuthenticationInformation_SkipsIfTokenInvalidTest()
        {
            IProxerClient client = ProxerClient.Create(new char[32]);

            char[] loginToken = RandomHelper.GetRandomString(42).ToCharArray();
            var defaultLoginManager = new DefaultLoginManager(loginToken);

            IRequestBuilder request = new RequestBuilder(new Uri("http://proxer.me/api"), client).WithLoginCheck();
            bool added = defaultLoginManager.AddAuthenticationInformation(request);
            Assert.False(added);
            Assert.False(request.Headers.ContainsKey(LoginTokenHeaderName));

            defaultLoginManager = new DefaultLoginManager();

            added = defaultLoginManager.AddAuthenticationInformation(request);
            Assert.False(added);
            Assert.False(request.Headers.ContainsKey(LoginTokenHeaderName));
        }

        [Test]
        public void ContainsAuthenticationInformationTest()
        {
            IProxerClient client = ProxerClient.Create(new char[32]);
            var loginManager = new DefaultLoginManager(RandomHelper.GetRandomString(255).ToCharArray());
            var request = new RequestBuilder(new Uri("https://proxer.me/api"), client);

            Assert.False(loginManager.ContainsAuthenticationInformation(request));
            request.WithHeader("proxer-api-token", loginManager.LoginToken.ToString());
            Assert.True(loginManager.ContainsAuthenticationInformation(request));
        }

        [Test]
        public void InvalidateLoginTest()
        {
            IProxerClient client = ProxerClient.Create(new char[32]);
            DefaultLoginManager loginManager = CreateLoggedInManager(new char[255], client);

            IRequestBuilder request = new RequestBuilder(new Uri("https://proxer.me/api"), client).WithLoginCheck();
            Assert.False(loginManager.AddAuthenticationInformation(request));
            Assert.True(loginManager.IsLoginProbablyValid());
            loginManager.InvalidateLogin();
            Assert.True(loginManager.AddAuthenticationInformation(request));
            Assert.False(loginManager.IsLoginProbablyValid());
        }

        [Test]
        public void IsLoginProbablyValid_InvalidIfLoginNeverPerformedTest()
        {
            var loginManager = new DefaultLoginManager(new char[255]);

            Assert.False(loginManager.IsLoginProbablyValid());
        }

        [Test]
        public void IsLoginProbablyValid_ValidIfJustLoggedInTest()
        {
            //TODO: The other cases are hard to test. Maybe search for a package on github that helps with DateTime manipulation?
            IProxerClient client = ProxerClient.Create(new char[32]);
            DefaultLoginManager loginManager = CreateLoggedInManager(new char[255], client);

            Assert.True(loginManager.IsLoginProbablyValid());
        }

        [Test]
        public void UpdateTest()
        {
            IProxerClient client = ProxerClient.Create(new char[32]);
            var loginManager = new DefaultLoginManager(new char[255]);
            Assert.False(loginManager.IsLoginProbablyValid());

            var request = new RequestBuilder(new Uri("http://proxer.me/api"), client);

            // request does not contain any login information and the login manager was not logged in
            // => the login manager is still not logged in
            loginManager.Update(request, new ProxerResult());
            Assert.False(loginManager.IsLoginProbablyValid());

            bool added = loginManager.AddAuthenticationInformation(request.WithLoginCheck());
            Assert.True(added);

            loginManager.Update(request, new ProxerResult());
            Assert.True(loginManager.IsLoginProbablyValid());
        }

        [Test]
        public void UpdateTest_ListensForLogin()
        {
            const string loginToken = "token_test";

            IProxerClient client = ProxerClient.Create(new char[32]);
            var loginManager = new DefaultLoginManager();
            Assert.False(loginManager.IsLoginProbablyValid());
            Assert.IsNull(loginManager.LoginToken);

            IRequestBuilderWithResult<LoginDataModel> request =
                client.CreateRequest().FromUserClass().Login(new LoginInput {Password = "pass", Username = "user"});

            var response = new ProxerApiResponse<LoginDataModel>
                {Success = true, Result = new LoginDataModel {Token = loginToken, UserId = 5}};

            // request does not contain any login information and the login manager was not logged in
            // => the login manager is still not logged in
            loginManager.Update(request, response);
            Assert.True(loginManager.IsLoginProbablyValid());
            Assert.AreEqual(loginToken, loginManager.LoginToken);
        }

        [Test]
        public void UpdateTest_ListensForLogout()
        {
            IProxerClient client = ProxerClient.Create(new char[32]);
            DefaultLoginManager loginManager = CreateLoggedInManager(new char[255], client);
            Assert.True(loginManager.IsLoginProbablyValid());

            IRequestBuilder request = client.CreateRequest().FromUserClass().Logout();

            var response = new ProxerApiResponse { Success = true };

            // request does not contain any login information and the login manager was not logged in
            // => the login manager is still not logged in
            loginManager.Update(request, response);
            Assert.False(loginManager.IsLoginProbablyValid());
            Assert.IsNull(loginManager.LoginToken);
        }

        private static DefaultLoginManager CreateLoggedInManager(char[] loginToken, IProxerClient client)
        {
            var loginManager = new DefaultLoginManager(loginToken);
            IRequestBuilder request = new RequestBuilder(new Uri("http://proxer.me/api"), client).WithLoginCheck();
            loginManager.AddAuthenticationInformation(request);
            loginManager.Update(request, new ProxerResult());

            return loginManager;
        }
    }
}