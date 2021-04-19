using System;
using Azuria.Authentication;
using Azuria.ErrorHandling;
using Azuria.Requests.Builder;

namespace Azuria.Test.Middleware
{
    internal class TestLoginManager : ILoginManager
    {
        public const string LOGIN_HEADER_KEY = "proxer-test-login-token";
        public const string LOGIN_HEADER_VALUE = "login-token-value";

        private readonly Action<IRequestBuilderBase, IProxerResultBase> _onUpdate;
        private bool _loginInvalidated;

        public TestLoginManager(Action<IRequestBuilderBase, IProxerResultBase> onUpdate = null)
        {
            this._onUpdate = onUpdate;
        }

        public bool AddAuthenticationInformation(IRequestBuilderBase request)
        {
            if (request.BuildUri().Query.Contains("addLogin=1") || this._loginInvalidated)
            {
                request.Headers[LOGIN_HEADER_KEY] = LOGIN_HEADER_VALUE;
                return true;
            }

            return false;
        }

        public bool ContainsAuthenticationInformation(IRequestBuilderBase request)
        {
            return request.Headers.ContainsKey(LOGIN_HEADER_KEY);
        }

        public void InvalidateLogin()
        {
            this._loginInvalidated = true;
        }

        public void SetLogin(char[] loginToken)
        {
            throw new NotImplementedException();
        }

        public bool IsLoginProbablyValid()
        {
            return !this._loginInvalidated;
        }

        public void Update(IRequestBuilderBase request, IProxerResultBase result)
        {
            this._onUpdate?.Invoke(request, result);
        }
    }
}