using System;
using System.Threading;
using System.Threading.Tasks;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.RequestBuilder;
using Azuria.ErrorHandling;
using Azuria.Requests;

namespace Azuria.Authentication
{
    /// <summary>
    /// Represents a class that is used to authenticate a <see cref="IProxerClient">client</see> and keep it
    /// authenticated.
    /// </summary>
    public class LoginManager : ILoginManager
    {
        private readonly UserRequestBuilder _userRequestBuilder;
        private bool _isLoginQueued;
        private DateTime _lastRequestPerformed = DateTime.MinValue;
        private DateTime _loginPerformed = DateTime.MinValue;

        public LoginManager(UserRequestBuilder userRequestBuilder, char[] loginToken = null)
        {
            this._userRequestBuilder = userRequestBuilder;
            this.LoginToken = loginToken;
        }

        #region Properties

        /// <inheritdoc />
        public char[] LoginToken { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public bool CheckIsLoginProbablyValid()
        {
            if (this._loginPerformed == DateTime.MinValue)
                return false;
            if (this._lastRequestPerformed == DateTime.MinValue)
                return DateTime.Now.Subtract(this._loginPerformed).TotalHours < 24;
            return DateTime.Now.Subtract(this._lastRequestPerformed).TotalHours < 1;
        }

        /// <inheritdoc />
        public virtual async Task<IProxerResult> PerformLogin(
            string username, string password, string secretKey = null,
            CancellationToken token = new CancellationToken())
        {
            IProxerResult<LoginDataModel> lResult =
                await (secretKey == null
                           ? this._userRequestBuilder.Login(username, password)
                           : this._userRequestBuilder.Login(username, password, secretKey))
                    .DoRequestAsync(token);

            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult(lResult.Exceptions);

            this.LoginToken = lResult.Result.Token.ToCharArray();
            this._loginPerformed = DateTime.Now;
            return new ProxerResult();
        }

        /// <inheritdoc />
        public virtual async Task<IProxerResult> PerformLogout(CancellationToken token = new CancellationToken())
        {
            IProxerResult lResult = await this._userRequestBuilder.Logout().DoRequestAsync(token);
            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            this.LoginToken = null;
            this._loginPerformed = DateTime.MinValue;
            return new ProxerResult();
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