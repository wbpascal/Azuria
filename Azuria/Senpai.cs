using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.User;
using Azuria.Community.Conference;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Web;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria
{
    /// <summary>
    ///     Represents a user that makes requests to the proxer servers.
    /// </summary>
    public class Senpai
    {
        private DateTime _cookiesCreated;
        private DateTime _cookiesLastUsed = DateTime.MinValue;
        private string _username;

        /// <summary>
        /// </summary>
        /// <param name="username"></param>
        public Senpai([NotNull] string username)
        {
            if (string.IsNullOrEmpty(username.Trim())) throw new ArgumentException(nameof(username));
            this._username = username;
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        public Senpai([NotNull] Senpai senpai)
        {
            this._username = senpai._username;
            this._cookiesCreated = senpai._cookiesCreated;
            this._cookiesLastUsed = senpai._cookiesLastUsed;
            this.LoginCookies = senpai.LoginCookies;
            this.Me = senpai.Me;
        }

        internal Senpai()
        {
        }

        #region Properties

        /// <summary>
        ///     Gets if the user is probably currently logged in.
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
        ///     Gets the cookies that are used to make requests to the server with this user.
        /// </summary>
        [NotNull]
        public CookieContainer LoginCookies { get; } = new CookieContainer();

        /// <summary>
        /// </summary>
        [CanBeNull]
        public char[] LoginToken { get; protected set; }

        /// <summary>
        ///     Gets the profile of the user.
        /// </summary>
        [CanBeNull]
        public User.User Me { get; protected set; }

        /// <summary>
        ///     Gets the cookies that are used to make requests to the server with this user. Unlike <see cref="LoginCookies" />
        ///     contains this cookie container some additional cookies to request a response that is intended for mobile usage.
        /// </summary>
        [NotNull]
        public CookieContainer MobileLoginCookies
        {
            get
            {
                CookieContainer lMobileCookies = new CookieContainer();
                foreach (Cookie loginCookie in this.LoginCookies.GetCookies(new Uri("https://proxer.me/")))
                {
                    lMobileCookies.Add(new Uri("https://proxer.me/"), loginCookie);
                }
                lMobileCookies.Add(new Uri("https://proxer.me/"), new Cookie("device", "mobile", "/", "proxer.me"));
                return lMobileCookies;
            }
        }

        #endregion

        #region

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

        /// <summary>
        ///     Fetches all messaging conferences the user is part of.
        /// </summary>
        /// <returns>If the action was successful and if it was, an enumeration of the conferences.</returns>
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<Conference>>> GetAllConferences()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(new Uri("http://proxer.me/messages"), this);

            if (!lResult.Success)
                return new ProxerResult<IEnumerable<Conference>>(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                HtmlDocument lDocument = new HtmlDocument();
                lDocument.LoadHtml(lResponse);
                List<Conference> lReturn = new List<Conference>();

                IEnumerable<HtmlNode> lNodes = lDocument.DocumentNode.DescendantsAndSelf()
                    .Where(
                        x =>
                            x.Attributes.Contains("class") &&
                            x.Attributes["class"].Value == "conferenceGrid ");

                lReturn.AddRange(from curNode in lNodes
                    let lId =
                        Convert.ToInt32(
                            curNode.Attributes["href"].Value.GetTagContents("/messages?id=",
                                "#top")[0])
                    let lTitle = curNode.FirstChild.InnerText
                    select new Conference(lTitle, lId, this));

                return new ProxerResult<IEnumerable<Conference>>(lReturn);
            }
            catch
            {
                return
                    new ProxerResult<IEnumerable<Conference>>(
                        ErrorHandler.HandleError(this, lResponse, false).Exceptions);
            }
        }

        /// <summary>
        ///     Logs the user in.
        /// </summary>
        /// <param name="password">The password of the user.</param>
        /// <returns>If the action was successful and if it was, whether the user was successfully logged in.</returns>
        [ItemNotNull]
        public async Task<ProxerResult<bool>> Login([NotNull] string password)
        {
            if (string.IsNullOrEmpty(password.Trim()))
                return new ProxerResult<bool>(new[] {new ArgumentException(nameof(password))});
            if (this.IsProbablyLoggedIn) return new ProxerResult<bool>(new[] {new UserAlreadyLoggedInException()});

            ProxerResult<ProxerApiResponse<LoginDataModel>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.BuildForLogin(this._username, password, this));
            if (!lResult.Success || lResult.Result == null)
            {
                return new ProxerResult<bool>(lResult.Exceptions);
            }

            this.Me = new User.User(this._username, lResult.Result.Data.UserId,
                new Uri("https://cdn.proxer.me/avatar/" + lResult.Result.Data.Avatar), this);
            this._cookiesCreated = DateTime.Now;
            this.LoginToken = lResult.Result.Data.Token.ToCharArray();

            return new ProxerResult<bool>(true);
        }

        internal async Task<ProxerResult> LoginWithToken(char[] token = null)
        {
            token = token ?? this.LoginToken;
            string lLoginToken = new string(token);
            if (string.IsNullOrEmpty(lLoginToken.Trim()))
                return new ProxerResult(new[] {new ArgumentException(nameof(token))});

            ProxerResult<ProxerApiResponse<UserInfoDataModel>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.BuildForGetUserInfo(null, this), lLoginToken);
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);
            UserInfoDataModel lDataModel = lResult.Result.Data;
            this.LoginToken = token;
            this.Me = new User.User(lDataModel, this);
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
                await RequestHandler.ApiRequest(ApiRequestBuilder.BuildForLogout(this));
            if (!lResult.Success || lResult.Result == null || lResult.Result.Error)
                return new ProxerResult(lResult.Exceptions);
            this._cookiesCreated = DateTime.MinValue;
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