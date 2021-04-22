using System;
using System.Threading;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.Input.User;
using Azuria.ErrorHandling;
using Azuria.Requests;
using Azuria.Requests.Builder;

namespace Azuria.Authentication
{
    /// <summary>
    /// Represents a class that is used to authenticate a <see cref="IProxerClient">client</see> and keep it
    /// authenticated.
    /// </summary>
    public class DefaultLoginManager : ILoginManager
    {
        private const string LoginTokenHeaderName = "proxer-api-token";

        //TODO: Maybe make this protected so that the class can be derived and these values can be saved?
        private DateTime _lastRequestPerformed = DateTime.MinValue;
        private DateTime _loginPerformed = DateTime.MinValue;

        /// <summary>
        /// </summary>
        /// <param name="loginToken"></param>
        public DefaultLoginManager(char[] loginToken = null)
        {
            this.LoginToken = loginToken;
        }

        /// <summary>
        /// Gets or sets the login token that will be used by this login manager to authenticate the user
        /// </summary>
        public char[] LoginToken { get; set; }

        /// <inheritdoc />
        public bool AddAuthenticationInformation(IRequestBuilderBase request)
        {
            if (!request.CheckLogin) return false;
            if (this.ContainsAuthenticationInformation(request)) return false;
            if (this.LoginToken == null || this.LoginToken.Length != 255 || this.IsLoginProbablyValid())
                return false;

            request.Headers[LoginTokenHeaderName] = this.LoginToken.ToString();
            return true;
        }

        /// <inheritdoc />
        public bool ContainsAuthenticationInformation(IRequestBuilderBase request)
        {
            return request.Headers.ContainsKey(LoginTokenHeaderName);
        }

        /// <inheritdoc />
        public void InvalidateLogin()
        {
            this._loginPerformed = DateTime.MinValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsLoginProbablyValid()
        {
            if (this._loginPerformed == DateTime.MinValue)
                return false;
            if (this._lastRequestPerformed == DateTime.MinValue)
                return DateTime.Now.Subtract(this._loginPerformed).TotalHours < 24;
            return DateTime.Now.Subtract(this._lastRequestPerformed).TotalHours < 1;
        }

        /// <inheritdoc />
        public void SetLogin(char[] loginToken)
        {
            this.LoginToken = loginToken;
            this._loginPerformed = DateTime.Now;
        }

        private void TryParseLogin(IRequestBuilderBase request, IProxerResultBase result)
        {
            if (request.BuildUri().AbsolutePath != "/api/v1/user/login" ||
                !(result is IProxerResult<LoginDataModel> loginResult) || !result.Success) return;

            this.SetLogin(loginResult.Result.Token.ToCharArray());
        }

        private void TryParseLogout(IRequestBuilderBase request, IProxerResultBase result)
        {
            if (request.BuildUri().AbsolutePath != "/api/v1/user/logout" || !result.Success) return;

            this.LoginToken = null;
            this.InvalidateLogin();
        }

        /// <inheritdoc />
        public void Update(IRequestBuilderBase request, IProxerResultBase result)
        {
            this.TryParseLogin(request, result);
            this.TryParseLogout(request, result);

            this._lastRequestPerformed = DateTime.Now;
            if (result.Success && this.ContainsAuthenticationInformation(request))
                this._loginPerformed = DateTime.Now;
        }
    }
}