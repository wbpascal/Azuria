using System;
using System.Threading.Tasks;
using Azuria.Api;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.RequestBuilder;
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
        /// </summary>
        public ISecureContainer<char[]> LoginToken { get; protected set; }

        /// <summary>
        /// Gets the profile of the user.
        /// </summary>
        public User Me { get; protected set; }

        #endregion

        #region Methods

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this.LoginToken.Dispose();
            this.HttpClient.Dispose();
        }

        /// <summary>
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public static async Task<IProxerResult<Senpai>> FromCredentials(IProxerCredentials credentials)
        {
            Senpai lSenpai = new Senpai();
            IProxerResult lResult = await lSenpai.LoginWithCredentials(credentials).ConfigureAwait(false);
            if (lResult.Success) return new ProxerResult<Senpai>(lSenpai);

            lSenpai.Dispose();
            return new ProxerResult<Senpai>(lResult.Exceptions);
        }

        /// <summary>
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<IProxerResult<Senpai>> FromToken(char[] token)
        {
            Senpai lSenpai = new Senpai();
            IProxerResult lResult = await lSenpai.LoginWithToken(token).ConfigureAwait(false);
            if (lResult.Success) return new ProxerResult<Senpai>(lSenpai);

            lSenpai.Dispose();
            return new ProxerResult<Senpai>(lResult.Exceptions);
        }

        private void InvalidateCookies()
        {
            this._cookiesCreated = DateTime.MinValue;
            this._cookiesLastUsed = DateTime.MinValue;
        }

        private async Task<IProxerResult> LoginWithCredentials(IProxerCredentials credentials)
        {
            if (string.IsNullOrEmpty(credentials?.Username) || (credentials.Password.Length == 0))
                return new ProxerResult(new[] {new ArgumentException(nameof(credentials))});
            if (this.IsProbablyLoggedIn) return new ProxerResult<bool>(new AlreadyLoggedInException());

            ProxerApiResponse<LoginDataModel> lResult = await RequestHandler.ApiRequest(
                    UserRequestBuilder.Login(credentials.Username, new string(credentials.Password), this))
                .ConfigureAwait(false);
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult(lResult.Exceptions);

            this.Me = new User(credentials.Username, lResult.Result.UserId,
                new Uri("https://cdn.proxer.me/avatar/" + lResult.Result.Avatar));
            this._cookiesCreated = DateTime.Now;
            this.LoginToken.SetValue(lResult.Result.Token.ToCharArray());

            return new ProxerResult();
        }

        private async Task<IProxerResult> LoginWithToken(char[] token)
        {
            if ((token == null) || (token.Length != 255))
                return new ProxerResult(new[] {new ArgumentException(nameof(token))});

            this.LoginToken.SetValue(token);
            ProxerApiResponse<UserInfoDataModel> lResult = await RequestHandler.ApiRequest(
                UserRequestBuilder.GetInfo(this), true).ConfigureAwait(false);
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult(lResult.Exceptions);
            UserInfoDataModel lDataModel = lResult.Result;
            this.Me = new User(lDataModel);
            this._cookiesCreated = DateTime.Now;

            return new ProxerResult();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<IProxerResult> Logout()
        {
            if (this._cookiesCreated == DateTime.MinValue) return new ProxerResult(new NotLoggedInException(this));

            ProxerApiResponse lResult = await RequestHandler.ApiRequest(UserRequestBuilder.Logout(this))
                .ConfigureAwait(false);
            if (!lResult.Success) return new ProxerResult(lResult.Exceptions);
            this.InvalidateCookies();
            return new ProxerResult();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<IProxerResult> TryRelogin()
        {
            char[] lLoginToken = this.LoginToken.ReadValue();
            if (lLoginToken.Length != 255) return new ProxerResult {Success = false};
            return await this.LoginWithToken(lLoginToken).ConfigureAwait(false);
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