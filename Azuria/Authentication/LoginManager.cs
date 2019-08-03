using System;
using System.Threading;
using System.Threading.Tasks;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.Input.User;
using Azuria.Api.v1.RequestBuilder;
using Azuria.ErrorHandling;
using Azuria.Requests;
using Azuria.Requests.Builder;

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

        /// <summary>
        /// </summary>
        /// <param name="userRequestBuilder"></param>
        /// <param name="loginToken"></param>
        public LoginManager(UserRequestBuilder userRequestBuilder, char[] loginToken = null)
        {
            this._userRequestBuilder = userRequestBuilder;
            this.LoginToken = loginToken;
        }

        /// <inheritdoc />
        public char[] LoginToken { get; set; }

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
        public void PerformedRequest(bool sendLoginToken = false)
        {
            this._lastRequestPerformed = DateTime.Now;
            this._isLoginQueued = false;
            if (sendLoginToken) this._loginPerformed = DateTime.Now;
        }

        /// <inheritdoc />
        public virtual async Task<IProxerResult> PerformLoginAsync(
            LoginInput input, CancellationToken token = default)
        {
            IRequestBuilderWithResult<LoginDataModel> lRequest = this._userRequestBuilder.Login(input);

            IProxerResult<LoginDataModel> lResult = await lRequest.DoRequestAsync(token);

            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult(lResult.Exceptions);

            this.LoginToken = lResult.Result.Token.ToCharArray();
            this._loginPerformed = DateTime.Now;
            return new ProxerResult();
        }

        /// <inheritdoc />
        public virtual async Task<IProxerResult> PerformLogoutAsync(
            CancellationToken token = default)
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
            this._isLoginQueued = this.LoginToken?.Length == 255;
        }

        /// <inheritdoc />
        public bool SendTokenWithNextRequest()
        {
            return this.LoginToken?.Length == 255 &&
                   (this._isLoginQueued || !this.CheckIsLoginProbablyValid());
        }
    }
}