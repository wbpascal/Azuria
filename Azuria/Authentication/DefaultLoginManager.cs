using System;
using System.Collections.Generic;
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
        private readonly IProxerClient _client;

        //TODO: Maybe make this protected so that the class can be derived and these values can be saved?
        private DateTime _lastRequestPerformed = DateTime.MinValue;
        private DateTime _loginPerformed = DateTime.MinValue;

        /// <summary>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="loginToken"></param>
        public DefaultLoginManager(IProxerClient client, char[] loginToken = null)
        {
            _client = client;
            this.LoginToken = loginToken;
        }

        /// <summary>
        /// Gets or sets the login token that will be used by this login manager to authenticate the user
        /// </summary>
        public char[] LoginToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns>If any information was added to the request</returns>
        public bool AddAuthenticationInformation(IRequestBuilderBase request)
        {
            if (!request.CheckLogin) return false;
            if (this.ContainsAuthenticationInformation(request)) return false;
            if (this.LoginToken == null || this.LoginToken.Length != 255 || this.IsLoginProbablyValid())
                return false;

            request.Headers[LoginTokenHeaderName] = this.LoginToken.ToString();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool ContainsAuthenticationInformation(IRequestBuilderBase request)
        {
            return request.Headers.ContainsKey(LoginTokenHeaderName);
        }

        /// <summary>
        /// 
        /// </summary>
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual async Task<IProxerResult<LoginDataModel>> PerformLoginAsync(LoginInput input, CancellationToken token = default)
        {
            IRequestBuilderWithResult<LoginDataModel> loginRequest = this._client.CreateRequest().FromUserClass().Login(input);
            IProxerResult<LoginDataModel> result = await loginRequest.DoRequestAsync(token).ConfigureAwait(false);
            if (!result.Success || result.Result == null)
                return new ProxerResult<LoginDataModel>(result.Exceptions);

            this.LoginToken = result.Result.Token.ToCharArray();
            this._loginPerformed = DateTime.Now;
            return new ProxerResult<LoginDataModel>(result.Result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual async Task<IProxerResult> PerformLogoutAsync(CancellationToken token = default)
        {
            IRequestBuilder logoutRequest = this._client.CreateRequest().FromUserClass().Logout();
            IProxerResult lResult = await logoutRequest.DoRequestAsync(token).ConfigureAwait(false);
            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            this.LoginToken = null;
            this._loginPerformed = DateTime.MinValue;
            return new ProxerResult();
        }

        /// <inheritdoc />
        public void Update(IRequestBuilderBase request, IProxerResultBase result)
        {
            this._lastRequestPerformed = DateTime.Now;
            if (result.Success && this.ContainsAuthenticationInformation(request))
                this._loginPerformed = DateTime.Now;
        }
    }
}