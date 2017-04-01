using System;
using System.Threading.Tasks;
using Azuria.ErrorHandling;

namespace Azuria.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginManager : ILoginManager
    {
        private bool _isLoginQueued;
        private DateTime _lastRequestPerformed = DateTime.MinValue;
        private DateTime _loginPerformed = DateTime.MinValue;

        internal LoginManager(char[] loginToken = null)
        {
            this.LoginToken = loginToken;
        }

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public char[] LoginToken { get; set; }

        #endregion

        #region Methods

        private bool IsLoginProbablyValid()
        {
            if (this._lastRequestPerformed == DateTime.MinValue)
                return DateTime.Now.Subtract(this._loginPerformed).TotalHours < 24;
            return DateTime.Now.Subtract(this._lastRequestPerformed).TotalHours < 1;
        }

        /// <inheritdoc />
        public Task<IProxerResult> PerformLogin(string username, string password, string secretKey = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void QueueLoginForNextRequest()
        {
            this._isLoginQueued = true;
        }

        /// <inheritdoc />
        public bool SendTokenWithNextRequest()
        {
            if (this._isLoginQueued || this.LoginToken?.Length == 255 && !this.IsLoginProbablyValid())
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