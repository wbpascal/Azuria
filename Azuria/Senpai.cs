using System;
using System.Net;
using System.Threading.Tasks;
using Azuria.Api;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.User;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Security;
using Azuria.UserInfo;

namespace Azuria
{
    /// <summary>
    /// Represents a user that makes requests to the proxer servers.
    /// </summary>
    public class Senpai : IDisposable
    {
        private DateTime _cookiesCreated;
        private DateTime _cookiesLastUsed = DateTime.MinValue;
        private string _username;

        /// <summary>
        /// </summary>
        /// <param name="username"></param>
        public Senpai(string username) : this()
        {
            if (string.IsNullOrEmpty(username.Trim())) throw new ArgumentException(nameof(username));
            this._username = username;
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        public Senpai(Senpai senpai) : this()
        {
            this._username = senpai._username;
            this._cookiesCreated = senpai._cookiesCreated;
            this._cookiesLastUsed = senpai._cookiesLastUsed;
            this.LoginCookies = senpai.LoginCookies;
            this.LoginToken = senpai.LoginToken;
            this.Me = senpai.Me;
        }

        private Senpai()
        {
            this.LoginToken = ApiInfo.SecureContainerFactory.Invoke();
        }

        #region Properties

        /// <summary>
        /// Gets if the user is probably currently logged in.
        /// </summary>
        public bool IsProbablyLoggedIn
        {
            get
            {
                if (this._cookiesLastUsed == DateTime.MinValue)
                    return DateTime.Now.Subtract(this._cookiesCreated).TotalHours < 24;
                return DateTime.Now.Subtract(this._cookiesLastUsed).TotalHours < 1;
            }
        }

        /// <summary>
        /// Gets the cookies that are used to make requests to the server with this user.
        /// </summary>
        public CookieContainer LoginCookies { get; private set; } = new CookieContainer();

        /// <summary>
        /// </summary>
        public ISecureContainer<char[]> LoginToken { get; protected set; }

        /// <summary>
        /// Gets the profile of the user.
        /// </summary>
        public User Me { get; protected set; }

        /// <summary>
        /// Gets the cookies that are used to make requests to the server with this user. Unlike <see cref="LoginCookies" />
        /// contains this cookie container some additional cookies to request a response that is intended for mobile usage.
        /// </summary>
        public CookieContainer MobileLoginCookies
        {
            get
            {
                CookieContainer lMobileCookies = new CookieContainer();
                foreach (Cookie loginCookie in this.LoginCookies.GetCookies(new Uri("https://proxer.me/")))
                    lMobileCookies.Add(new Uri("https://proxer.me/"), loginCookie);
                lMobileCookies.Add(new Uri("https://proxer.me/"), new Cookie("device", "mobile", "/", "proxer.me"));
                return lMobileCookies;
            }
        }

        #endregion

        #region Methods

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this.LoginToken.Dispose();
        }

        /// <summary>
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<ProxerResult<Senpai>> FromToken(char[] token)
        {
            Senpai lSenpai = new Senpai();
            ProxerResult lResult = await lSenpai.LoginWithToken(token);
            return !lResult.Success ? new ProxerResult<Senpai>(lResult.Exceptions) : new ProxerResult<Senpai>(lSenpai);
        }

        internal void InvalidateCookies()
        {
            this._cookiesCreated = DateTime.MinValue;
            this._cookiesLastUsed = DateTime.MinValue;
            this.LoginCookies = new CookieContainer();
        }

        /// <summary>
        /// Logs the user in.
        /// </summary>
        /// <param name="password">The password of the user.</param>
        /// <returns>If the action was successful and if it was, whether the user was successfully logged in.</returns>
        public async Task<ProxerResult<bool>> Login(string password)
        {
            if (string.IsNullOrEmpty(password.Trim()))
                return new ProxerResult<bool>(new[] {new ArgumentException(nameof(password))});
            if (this.IsProbablyLoggedIn) return new ProxerResult<bool>(new[] {new UserAlreadyLoggedInException()});

            ProxerResult<ProxerApiResponse<LoginDataModel>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.UserLogin(this._username, password, this));
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<bool>(lResult.Exceptions);

            this.Me = new User(this._username, lResult.Result.Data.UserId,
                new Uri("https://cdn.proxer.me/avatar/" + lResult.Result.Data.Avatar));
            this._cookiesCreated = DateTime.Now;
            this.LoginToken.SetValue(lResult.Result.Data.Token.ToCharArray());

            return new ProxerResult<bool>(true);
        }

        internal async Task<ProxerResult> LoginWithToken(char[] token = null)
        {
            token = token ?? this.LoginToken.ReadValue();
            if (token.Length == 0)
                return new ProxerResult(new[] {new ArgumentException(nameof(token))});

            ProxerResult<ProxerApiResponse<UserInfoDataModel>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.UserGetInfo(this), token);
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult(lResult.Exceptions);
            UserInfoDataModel lDataModel = lResult.Result.Data;
            this.LoginToken.SetValue(token);
            this.Me = new User(lDataModel);
            this._username = lDataModel.Username;
            this._cookiesCreated = DateTime.Now;

            return new ProxerResult();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<ProxerResult> Logout()
        {
            ProxerResult<ProxerApiResponse> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.UserLogout(this));
            if (!lResult.Success || (lResult.Result == null) || lResult.Result.Error)
                return new ProxerResult(lResult.Exceptions);
            this.InvalidateCookies();
            return new ProxerResult();
        }

        /// <summary>
        /// </summary>
        public void UsedCookies()
        {
            this._cookiesLastUsed = DateTime.Now;
        }

        #endregion
    }
}