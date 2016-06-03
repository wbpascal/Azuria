using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using Azuria.Community;
using Azuria.EventArguments;
using Azuria.Exceptions;
using Azuria.Main;
using Azuria.Notifications;
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
        /// <summary>
        ///     Represents a method that is executed when new anime or manga notifications are available.
        /// </summary>
        /// <param name="sender">The user that recieved the notifications.</param>
        /// <param name="e">The notifications.</param>
        public delegate void AmNotificationEventHandler(Senpai sender, AmNotificationEventArgs e);

        /// <summary>
        ///     Represents a method that is executed when exceptions were thrown during the fetching of the notifications.
        /// </summary>
        /// <param name="sender">The user that should have recieved the notifications.</param>
        /// <param name="exceptions">The exceptions that were thrown.</param>
        public delegate void ErrorDuringNotificationFetchEventHandler(Senpai sender, IEnumerable<Exception> exceptions);

        /// <summary>
        ///     Represents a method that is executed when new friend request notifications are available.
        /// </summary>
        /// <param name="sender">The user that recieved the notifications.</param>
        /// <param name="e">The notifications.</param>
        public delegate void FriendNotificiationEventHandler(Senpai sender, FriendNotificationEventArgs e);

        /// <summary>
        ///     Represents a method that is executed when new news notifications are available.
        /// </summary>
        /// <param name="sender">The user that recieved the notifications.</param>
        /// <param name="e">The notifications.</param>
        public delegate void NewsNotificationEventHandler(Senpai sender, NewsNotificationEventArgs e);

        /// <summary>
        ///     Represents a method that is executed when at least one new notification is available.
        /// </summary>
        /// <param name="sender">The user that recieved the notifications.</param>
        /// <param name="e">The notifications.</param>
        public delegate void NotificationEventHandler(Senpai sender, IEnumerable<INotificationEventArgs> e);

        /// <summary>
        ///     Represents a method that is executed when new private message notifications are available.
        /// </summary>
        /// <param name="sender">The user that recieved the notifications.</param>
        /// <param name="e">The notifications.</param>
        public delegate void PmNotificationEventHandler(Senpai sender, PmNotificationEventArgs e);

        private readonly Timer _loginCheckTimer;
        private readonly Timer _notificationCheckTimer;
        private readonly Timer _propertyUpdateTimer;
        private readonly List<bool> _updateNotifications;
        private AnimeMangaNotificationCollection _animeMangaNotifications;
        private FriendRequestNotificationCollection _friendUpdates;
        private bool _isLoggedIn;
        private NewsNotificationCollection _newsNotificationUpdates;
        private PrivateMessageNotificationCollection _privateMessageNotificationUpdates;
        private int _userId;


        /// <summary>
        ///     Initialises a new instance of the class.
        /// </summary>
        public Senpai()
        {
            this._updateNotifications = new List<bool>(new[] {true, false, true, true});
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
                await this.CheckLogin();
            };

            this._notificationCheckTimer = new Timer
            {
                AutoReset = true,
                Interval = new TimeSpan(0, 15, 0).TotalMilliseconds
            };
            this._notificationCheckTimer.Elapsed += async (s, eArgs) =>
            {
                ProxerResult lResult = await this.CheckNotifications();
                try
                {
                    if (!lResult.Success)
                        this.ErrorDuringNotificationFetch?.Invoke(this, lResult.Exceptions);
                }
                catch
                {
                    //ignored
                }
            };

            this._propertyUpdateTimer = new Timer(new TimeSpan(0, 20, 0).TotalMilliseconds) {AutoReset = true};
            this._propertyUpdateTimer.Elapsed += (s, eArgs) =>
            {
                for (int i = 0; i < 4; i++)
                    this._updateNotifications[i] = true;
            };
        }

        #region Properties

        /// <summary>
        ///     Gets the current <see cref="Anime" /> and <see cref="Manga" /> notifications.
        /// </summary>
        [NotNull]
        public AnimeMangaNotificationCollection AnimeMangaNotifications
        {
            get
            {
                if (!this._updateNotifications[0] && this._animeMangaNotifications != null)
                    return this._animeMangaNotifications;

                this._animeMangaNotifications = new AnimeMangaNotificationCollection(this);
                this._updateNotifications[0] = false;

                return this._animeMangaNotifications;
            }
        }

        /// <summary>
        ///     Gets the current friend request notifications.
        /// </summary>
        [NotNull]
        public FriendRequestNotificationCollection FriendRequestsNotification
        {
            get
            {
                if (!this._updateNotifications[1] && this._friendUpdates != null) return this._friendUpdates;

                this._friendUpdates = new FriendRequestNotificationCollection(this);
                this._updateNotifications[1] = false;

                return this._friendUpdates;
            }
        }


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
                    this._notificationCheckTimer.Stop();
                    this._propertyUpdateTimer.Stop();
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
        public User Me { get; protected set; }

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

        /// <summary>
        ///     Gets the current news notifications.
        /// </summary>
        [NotNull]
        public NewsNotificationCollection NewsNotification
        {
            get
            {
                if (!this._updateNotifications[2] && this._newsNotificationUpdates != null)
                    return this._newsNotificationUpdates;

                this._newsNotificationUpdates = new NewsNotificationCollection(this);
                this._updateNotifications[2] = false;

                return this._newsNotificationUpdates;
            }
        }

        /// <summary>
        ///     Gets the current private message notifications.
        /// </summary>
        [NotNull]
        public PrivateMessageNotificationCollection PrivateMessages
        {
            get
            {
                if (!this._updateNotifications[3] && this._privateMessageNotificationUpdates != null)
                    return this._privateMessageNotificationUpdates;

                this._privateMessageNotificationUpdates = new PrivateMessageNotificationCollection(this);
                this._updateNotifications[3] = false;

                return this._privateMessageNotificationUpdates;
            }
        }

        #endregion

        #region

        /// <summary>
        ///     Occurs when new <see cref="Anime" /> or <see cref="Manga" /> notifications are available. Notifications are checked
        ///     in a 15 minute interval.
        /// </summary>
        public event AmNotificationEventHandler AmUpdateNotificationRaised;

        [ItemNotNull]
        internal async Task<ProxerResult<bool>> CheckLogin()
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

        [ItemNotNull]
        private async Task<ProxerResult> CheckNotifications()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(new Uri("https://proxer.me/notifications?format=raw&s=count"),
                        this.LoginCookies, this);

            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            if (lResponse.StartsWith("1")) return new ProxerResult {Success = false};
            try
            {
                List<INotificationEventArgs> lNotificationEventArgs = new List<INotificationEventArgs>();
                string[] lResponseSplit = lResponse.Split('#');
                if (lResponseSplit.Length < 6)
                    return new ProxerResult(new Exception[] {new WrongResponseException {Response = lResponse}});

                for (int i = 0; i < 4; i++)
                    this._updateNotifications[i] = true;

                if (!lResponseSplit[2].Equals("0"))
                {
                    PmNotificationEventArgs lEventArgs = new PmNotificationEventArgs(
                        Convert.ToInt32(lResponseSplit[2]), this);
                    this.PmNotificationRaised?.Invoke(this, lEventArgs);

                    lNotificationEventArgs.Add(lEventArgs);
                }

                if (!lResponseSplit[3].Equals("0"))
                {
                    FriendNotificationEventArgs lEventArgs =
                        new FriendNotificationEventArgs(Convert.ToInt32(lResponseSplit[3]), this);
                    this.FriendNotificationRaised?.Invoke(this, lEventArgs);

                    lNotificationEventArgs.Add(lEventArgs);
                }

                if (!lResponseSplit[4].Equals("0"))
                {
                    NewsNotificationEventArgs lEventArgs =
                        new NewsNotificationEventArgs(Convert.ToInt32(lResponseSplit[4]), this);
                    this.NewsNotificationRaised?.Invoke(this, lEventArgs);

                    lNotificationEventArgs.Add(lEventArgs);
                }

                if (!lResponseSplit[5].Equals("0"))
                {
                    AmNotificationEventArgs lEventArgs = new AmNotificationEventArgs(
                        Convert.ToInt32(lResponseSplit[5]), this);
                    this.AmUpdateNotificationRaised?.Invoke(this, lEventArgs);

                    lNotificationEventArgs.Add(lEventArgs);
                }

                if (lNotificationEventArgs.Any())
                    this.NotificationRaised?.Invoke(this, lNotificationEventArgs);
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this, lResponse, false)).Exceptions);
            }

            return new ProxerResult();
        }

        /// <summary>
        ///     Occurs when exceptions were thrown during the fetching of the notifications.
        /// </summary>
        public event ErrorDuringNotificationFetchEventHandler ErrorDuringNotificationFetch;

        /// <summary>
        ///     Forces the properties to refresh themselves on the next call.
        /// </summary>
        [ItemNotNull]
        public async Task<ProxerResult> ForcePropertyReload()
        {
            ProxerResult<bool> lResult;
            if (!(lResult = await this.CheckLogin()).Success)
            {
                return new ProxerResult(lResult.Exceptions);
            }

            for (int i = 0; i < 4; i++)
                this._updateNotifications[i] = true;

            return new ProxerResult();
        }

        /// <summary>
        ///     Occurs when new friend request notifications are available. Notifications are checked in a 15 minute interval.
        /// </summary>
        public event FriendNotificiationEventHandler FriendNotificationRaised;

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
        ///     Initialises the notifications. Is needed in order for the notifications to be checked in a set interval.
        /// </summary>
        [NotNull]
        public ProxerResult InitNotifications()
        {
            if (!this.IsLoggedIn) return new ProxerResult(new Exception[] {new NotLoggedInException(this)});

            this._notificationCheckTimer.Start();
            this._propertyUpdateTimer.Start();

            return new ProxerResult();
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

                    this.Me = new User(username, this._userId,
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
                    this.Me = new User(this._userId,
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
        ///     Occurs when new news notifications are available. Notifications are checked in a 15 minute interval.
        /// </summary>
        public event NewsNotificationEventHandler NewsNotificationRaised;


        /// <summary>
        ///     Occurs when new notifications are available. Notifications are checked in a 15 minute interval.
        /// </summary>
        public event NotificationEventHandler NotificationRaised;

        /// <summary>
        ///     Occurs when new private message notifications are available. Notifications are checked in a 15 minute interval.
        /// </summary>
        public event PmNotificationEventHandler PmNotificationRaised;

        /// <summary>
        ///     Occurs when the login cookies of the user are invalid and therefore the user is not logged in anymore.
        /// </summary>
        public event EventHandler UserLoggedOutRaised;

        #endregion
    }
}