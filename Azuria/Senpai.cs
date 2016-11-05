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
using Azuria.Web;

namespace Azuria
{
    /// <summary>
    /// Represents a user that makes requests to the proxer servers.
    /// </summary>
    public class Senpai : IDisposable
    {
        private DateTime _cookiesCreated = DateTime.MinValue;
        private DateTime _cookiesLastUsed = DateTime.MinValue;

        /// <summary>
        /// </summary>
        /// <param name="username"></param>
        public Senpai(string username) : this()
        {
            if (string.IsNullOrEmpty(username.Trim())) throw new ArgumentException(nameof(username));
            this.Username = username;
        }

        private Senpai()
        {
            this.HttpClient = ApiInfo.HttpClientFactory.Invoke(this);
            this.LoginToken = ApiInfo.SecureContainerFactory.Invoke();
        }

        #region Properties

        internal IHttpClient HttpClient { get; }

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
        /// </summary>
        public string Username { get; private set; }

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
        public static async Task<IProxerResult<Senpai>> FromToken(char[] token)
        {
            if ((token == null) || (token.Length != 255))
                return new ProxerResult<Senpai>(new ArgumentException(nameof(token)));

            Senpai lSenpai = new Senpai();
            IProxerResult lResult = await lSenpai.LoginWithToken(token);
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
        public async Task<IProxerResult> Login(string password)
        {
            if (string.IsNullOrEmpty(password))
                return new ProxerResult(new[] {new ArgumentException(nameof(password))});
            if (this.IsProbablyLoggedIn) return new ProxerResult<bool>(new AlreadyLoggedInException());

            ProxerApiResponse<LoginDataModel> lResult = await RequestHandler.ApiRequest(
                ApiRequestBuilder.UserLogin(this.Username, password, this));
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult(lResult.Exceptions);

            this.Me = new User(this.Username, lResult.Result.UserId,
                new Uri("https://cdn.proxer.me/avatar/" + lResult.Result.Avatar));
            this._cookiesCreated = DateTime.Now;
            this.LoginToken.SetValue(lResult.Result.Token.ToCharArray());

            return new ProxerResult();
        }

        internal async Task<IProxerResult> LoginWithToken(char[] token = null)
        {
            if ((token == null) || (token.Length != 255))
                return new ProxerResult(new[] {new ArgumentException(nameof(token))});
            this.LoginToken.SetValue(token);

            ProxerApiResponse<UserInfoDataModel> lResult = await RequestHandler.ApiRequest(
                ApiRequestBuilder.UserGetInfo(this), true);
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult(lResult.Exceptions);
            UserInfoDataModel lDataModel = lResult.Result;
            this.LoginToken.SetValue(token);
            this.Me = new User(lDataModel);
            this.Username = lDataModel.Username;
            this._cookiesCreated = DateTime.Now;

            return new ProxerResult();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<IProxerResult> Logout()
        {
            if (this._cookiesCreated == DateTime.MinValue) return new ProxerResult(new NotLoggedInException(this));

            ProxerApiResponse lResult = await RequestHandler.ApiRequest(ApiRequestBuilder.UserLogout(this));
            if (!lResult.Success) return new ProxerResult(lResult.Exceptions);
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