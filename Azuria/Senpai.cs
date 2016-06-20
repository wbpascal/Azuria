using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using Azuria.Community.Conference;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Net;
using HtmlAgilityPack;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Azuria
{
    /// <summary>
    ///     Represents a user that makes requests to the proxer servers.
    /// </summary>
    public class Senpai
    {
        private readonly Timer _loginCheckTimer;
        private bool _isLoggedIn;
        private int _userId;


        /// <summary>
        ///     Initialises a new instance of the class.
        /// </summary>
        public Senpai()
        {
            this._isLoggedIn = false;
            this.LoginCookies = new CookieContainer();

            this._loginCheckTimer = new Timer
            {
                AutoReset = true,
                Interval = new TimeSpan(0, 45, 0).TotalMilliseconds
            };
            this._loginCheckTimer.Elapsed += async (s, eArgs) =>
            {
                this._loginCheckTimer.Interval = new TimeSpan(0, 30, 0).TotalMilliseconds;
                await this.ForceCheckLogin();
            };
        }

        #region Properties

        /// <summary>
        ///     Gets if the user is currently logged in. Is checked every 30 minutes through a timer.
        /// </summary>
        public bool IsLoggedIn
        {
            get { return this._isLoggedIn; }
            protected set
            {
                if (value)
                {
                    this._loginCheckTimer.Start();
                    this._isLoggedIn = true;
                }
                else
                {
                    this._loginCheckTimer.Stop();
                    if (this._isLoggedIn == false) return;
                    this._isLoggedIn = false;
                    this.UserLoggedOutRaised?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        ///     Gets the cookies that are used to make requests to the server with this user.
        /// </summary>
        [NotNull]
        public CookieContainer LoginCookies { get; protected set; }

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
        /// <returns></returns>
        [ItemNotNull]
        public async Task<ProxerResult<bool>> ForceCheckLogin()
        {
            ProxerResult<Tuple<string, CookieContainer>> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(new Uri("https://proxer.me/login?format=json&action=login"),
                        this.LoginCookies, this, new Func<string, ProxerResult>[0], false);

            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult<bool>(lResult.Exceptions);

            string lResponse = lResult.Result.Item1;

            try
            {
                Dictionary<string, string> responseDes =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                if (responseDes["error"].Equals("0"))
                {
                    this._loginCheckTimer.Reset();
                    if (!this.IsLoggedIn) this.IsLoggedIn = true;
                    return new ProxerResult<bool>(true);
                }
                if (this.IsLoggedIn) this.IsLoggedIn = false;
                return new ProxerResult<bool>(false);
            }
            catch
            {
                return new ProxerResult<bool>((await ErrorHandler.HandleError(this, lResponse, true)).Exceptions);
            }
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
                    HttpUtility.GetResponseErrorHandling(new Uri("http://proxer.me/messages"), this.LoginCookies, this);

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
                        (await ErrorHandler.HandleError(this, lResponse, false)).Exceptions);
            }
        }

        /// <summary>
        ///     Logs the user in.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>If the action was successful and if it was, whether the user was successfully logged in.</returns>
        [ItemNotNull]
        public async Task<ProxerResult<bool>> Login([NotNull] string username, [NotNull] string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return new ProxerResult<bool>(false);

            Dictionary<string, string> postArgs = new Dictionary<string, string>
            {
                {"username", username},
                {"password", password}
            };

            ProxerResult<Tuple<string, CookieContainer>> lResult =
                await
                    HttpUtility.PostResponseErrorHandling(new Uri("https://proxer.me/login?format=json&action=login"),
                        postArgs, this.LoginCookies, this, new Func<string, ProxerResult>[0], false);

            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult<bool>(lResult.Exceptions);

            string lResponse = lResult.Result.Item1;
            this.LoginCookies = lResult.Result.Item2;

            try
            {
                Dictionary<string, string> lDeserialisedResponse =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                if (lDeserialisedResponse["error"].Equals("0"))
                {
                    this._userId = Convert.ToInt32(lDeserialisedResponse["uid"]);

                    this.Me = new User.User(username, this._userId,
                        lDeserialisedResponse.ContainsKey("avatar")
                            ? new Uri("https://cdn.proxer.me/avatar/" + lDeserialisedResponse["avatar"])
                            : null, this);
                    this.IsLoggedIn = true;

                    return new ProxerResult<bool>(true);
                }
                if (lDeserialisedResponse["message"].Equals("Too many connections. Try again later."))
                    return new ProxerResult<bool>(new[] {new FirewallException()});

                this.IsLoggedIn = false;

                return new ProxerResult<bool>(false);
            }
            catch
            {
                return new ProxerResult<bool>(ErrorHandler.HandleError(this, lResponse).Exceptions);
            }
        }

        /// <summary>
        ///     Logs the user in.
        /// </summary>
        /// <param name="cookieContainer">The cookies that contain the logged in state of the user.</param>
        /// <returns>If the action was successful and if it was, whether the user was successfully logged in.</returns>
        [ItemNotNull]
        public async Task<ProxerResult<bool>> Login([NotNull] CookieContainer cookieContainer)
        {
            ProxerResult<Tuple<string, CookieContainer>> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(new Uri("https://proxer.me/login?format=json&action=login"),
                        cookieContainer, this, new Func<string, ProxerResult>[0], false);

            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult<bool>(lResult.Exceptions);

            string lResponse = lResult.Result.Item1;

            try
            {
                Dictionary<string, string> lDeserialisedResponse =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                if (lDeserialisedResponse["error"].Equals("0"))
                {
                    this._userId = Convert.ToInt32(lDeserialisedResponse["uid"]);
                    this.LoginCookies = lResult.Result.Item2;
                    this.Me = new User.User(this._userId,
                        lDeserialisedResponse.ContainsKey("avatar")
                            ? new Uri("https://cdn.proxer.me/avatar/" + lDeserialisedResponse["avatar"])
                            : null, this);
                    this.IsLoggedIn = true;

                    return new ProxerResult<bool>(true);
                }
                if (lDeserialisedResponse["message"].Equals("Too many connections. Try again later."))
                    return new ProxerResult<bool>(new[] {new FirewallException()});

                this.IsLoggedIn = false;

                return new ProxerResult<bool>(false);
            }
            catch
            {
                return new ProxerResult<bool>(ErrorHandler.HandleError(this, lResponse).Exceptions);
            }
        }

        /// <summary>
        ///     Occurs when the login cookies of the user are invalid and therefore the user is not logged in anymore.
        /// </summary>
        public event EventHandler UserLoggedOutRaised;

        #endregion
    }
}