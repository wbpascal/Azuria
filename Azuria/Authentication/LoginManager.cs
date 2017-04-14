using System;
using System.Threading;
using System.Threading.Tasks;
using Azuria.Api;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.RequestBuilder;
using Azuria.ErrorHandling;

namespace Azuria.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginManager : ILoginManager
    {
        private readonly IProxerClient _client;
        private bool _isLoginQueued;
        private DateTime _lastRequestPerformed = DateTime.MinValue;
        private DateTime _loginPerformed = DateTime.MinValue;

        internal LoginManager(IProxerClient client, char[] loginToken = null)
        {
            this._client = client;
            this.LoginToken = loginToken;
        }

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public char[] LoginToken { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public bool CheckIsLoginProbablyValid()
        {
            if (this._lastRequestPerformed == DateTime.MinValue)
                return DateTime.Now.Subtract(this._loginPerformed).TotalHours < 24;
            return DateTime.Now.Subtract(this._lastRequestPerformed).TotalHours < 1;
        }

        /// <inheritdoc />
        public virtual async Task<IProxerResult> PerformLogin(
            string username, string password, CancellationToken token, string secretKey = null)
        {
            UserRequestBuilder lRequestBuilder = this._client.CreateRequest().FromUserClass();
            IProxerResult<LoginDataModel> lResult = await (secretKey == null
                                                               ? lRequestBuilder.Login(username, password)
                                                               : lRequestBuilder.Login(username, password, secretKey))
                                                        .DoRequestAsync(token);
            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult(lResult.Exceptions);

            this.LoginToken = lResult.Result.Token.ToCharArray();
            this._loginPerformed = DateTime.Now;
            return new ProxerResult();
        }

        /// <inheritdoc />
        public Task<IProxerResult> PerformLogin(string username, string password, string secretKey = null)
        {
            return this.PerformLogin(username, password, new CancellationToken(), secretKey);
        }

        /// <inheritdoc />
        public virtual async Task<IProxerResult> PerformLogout(CancellationToken token)
        {
            IProxerResult lResult = await this._client.CreateRequest()
                                        .FromUserClass()
                                        .Logout()
                                        .DoRequestAsync(token);
            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            this.LoginToken = null;
            this._loginPerformed = DateTime.MinValue;
            return new ProxerResult();
        }

        /// <inheritdoc />
        public Task<IProxerResult> PerformLogout()
        {
            return this.PerformLogout(new CancellationToken());
        }

        /// <inheritdoc />
        public void QueueLoginForNextRequest()
        {
            this._isLoginQueued = true;
        }

        /// <inheritdoc />
        public bool SendTokenWithNextRequest()
        {
            if (this.LoginToken?.Length == 255 && (this._isLoginQueued || !this.CheckIsLoginProbablyValid()))
            {
                this._loginPerformed = DateTime.Now;
                this._lastRequestPerformed = DateTime.Now;
                return true;
            }

            this._lastRequestPerformed = DateTime.Now;
            return false;
        }

        #endregion
    }
}