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
        private const string LoginTokenHeaderName = "proxer-api-token";

        private DateTime _lastRequestPerformed = DateTime.MinValue;
        private DateTime _loginPerformed = DateTime.MinValue;
        private char[] _loginToken;

        /// <summary>
        /// </summary>
        /// <param name="loginToken"></param>
        public LoginManager(char[] loginToken = null)
        {
            this._loginToken = loginToken;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns>If any information was added to the request</returns>
        public bool AddAuthenticationInformation(IRequestBuilderBase request)
        {
            if (this.ContainsAuthenticationInformation(request)) return false;
            if (this._loginToken == null || this._loginToken.Length != 255 || this.IsLoginProbablyValid())
                return false;

            request.Headers[LoginTokenHeaderName] = this._loginToken.ToString();
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
        /// <param name="result"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Update(IProxerResultBase result)
        {
            this._lastRequestPerformed = DateTime.Now;

            // TODO: Update token after login + timestamp
        }
    }
}