using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using Azuria.Community;
using Azuria.EventArguments;
using Azuria.Exceptions;
using Azuria.Notifications;
using Azuria.Utilities;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Net;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Azuria
{
    /// <summary>
    ///     Der Benutzer der Anwendung an sich
    /// </summary>
    public class Senpai
    {
        /// <summary>
        ///     Stellt die Methode da, die ausgelöst wird, wenn neue Anime- oder Manga-Benachrichtigungen verfügbar sind.
        /// </summary>
        /// <param name="sender">Der Benutzer, der die Benachrichtigung empfangen hat.</param>
        /// <param name="e">Die Benachrichtigungen.</param>
        public delegate void AmNotificationEventHandler(Senpai sender, AmNotificationEventArgs e);

        /// <summary>
        ///     Stellt eine Methode da, die ausgelöst, wenn während des Abrufen der Benachrichtigungen eine Ausnahme ausgelöst
        ///     wird.
        /// </summary>
        /// <param name="sender">Der Benutzer, bei dem der Error aufgetreten ist.</param>
        /// <param name="exceptions">Die Ausnahmen, die aufgetreten sind.</param>
        public delegate void ErrorDuringNotificationFetchEventHandler(Senpai sender, IEnumerable<Exception> exceptions);

        /// <summary>
        ///     Stellt die Methode da, die ausgelöst wird, wenn neue Freundschafts-Benachrichtigungen verfügbar sind.
        /// </summary>
        /// <param name="sender">Der Benutzer, der die Benachrichtigung empfangen hat.</param>
        /// <param name="e">Die Benachrichtigungen.</param>
        public delegate void FriendNotificiationEventHandler(Senpai sender, FriendNotificationEventArgs e);

        /// <summary>
        ///     Stellt die Methode da, die ausgelöst wird, wenn neue News verfügbar sind.
        /// </summary>
        /// <param name="sender">Der Benutzer, der die Benachrichtigung empfangen hat.</param>
        /// <param name="e">Die Benachrichtigungen.</param>
        public delegate void NewsNotificationEventHandler(Senpai sender, NewsNotificationEventArgs e);

        /// <summary>
        ///     Stellt die Methode da, die ausgelöst wird, wenn neue Benachrichtigungen aller Art verfügbar sind.
        ///     Wird nur ausgelöst, wenn mindestens eine Benachrichtigung ausgelöst wurde aber wird nur höchstens bei
        ///     jeden Durchgang einmal ausgelöst.
        /// </summary>
        /// <param name="sender">Der Benutzer, der die Benachrichtigung empfangen hat.</param>
        /// <param name="e">Eine Aufzählung aller Benachrichtigungen.</param>
        public delegate void NotificationEventHandler(Senpai sender, IEnumerable<INotificationEventArgs> e);

        /// <summary>
        ///     Stellt die Methode da, die ausgelöst wird, wenn neue Privat-Nachricht-Benachrichtigungen verfügbar sind.
        /// </summary>
        /// <param name="sender">Der Benutzer, der die Benachrichtigung empfangen hat.</param>
        /// <param name="e">Die Benachrichtigungen.</param>
        public delegate void PmNotificationEventHandler(Senpai sender, PmNotificationEventArgs e);

        private readonly Timer _loginCheckTimer;
        private readonly Timer _notificationCheckTimer;
        private readonly Timer _propertyUpdateTimer;
        private readonly List<bool> _updateNotifications;
        private AnimeMangaUpdateCollection _animeMangaUpdates;
        private FriendRequestCollection _friendUpdates;
        private bool _isLoggedIn;
        private NewsCollection _newsUpdates;
        private PmCollection _pmUpdates;
        private int _userId;


        /// <summary>
        ///     Standard-Konstruktor der Klasse.
        /// </summary>
        public Senpai()
        {
            this.ErrHandler = new ErrorHandler();

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
        ///     Gibt ein Objekt zurück, mithilfe dessen alle Anime- und Manga-Benachrichtigungen abgerufen werden könne.
        /// </summary>
        /// <seealso cref="AnimeMangaUpdates" />
        /// <seealso cref="FriendRequests" />
        /// <seealso cref="News" />
        /// <seealso cref="PrivateMessages" />
        public AnimeMangaUpdateCollection AnimeMangaUpdates
        {
            get
            {
                if (!this._updateNotifications[0] && this._animeMangaUpdates != null)
                    return this._animeMangaUpdates;

                this._animeMangaUpdates = new AnimeMangaUpdateCollection(this);
                this._updateNotifications[0] = false;

                return this._animeMangaUpdates;
            }
        }

        /// <summary>
        ///     Gibt den Error-Handler zurück, der benutzt wird, um Fehler in Serverantworten zu bearbeiten und frühzeitig zu
        ///     erkennen.
        /// </summary>
        public ErrorHandler ErrHandler { get; }

        /// <summary>
        ///     Gibt ein Objekt zurück, mithilfe dessen alle Freundschafts-Benachrichtigungen abgerufen werden könne.
        /// </summary>
        /// <seealso cref="AnimeMangaUpdates" />
        /// <seealso cref="FriendRequests" />
        /// <seealso cref="News" />
        /// <seealso cref="PrivateMessages" />
        public FriendRequestCollection FriendRequests
        {
            get
            {
                if (!this._updateNotifications[1] && this._friendUpdates != null) return this._friendUpdates;

                this._friendUpdates = new FriendRequestCollection(this);
                this._updateNotifications[1] = false;

                return this._friendUpdates;
            }
        }


        /// <summary>
        ///     Gibt an, ob der Benutzter noch eingeloggt ist, wird aber nicht überprüft. (nur durch Timer alle 30 Minuten)
        /// </summary>
        public bool IsLoggedIn
        {
            get { return this._isLoggedIn; }
            private set
            {
                if (value)
                {
                    this._loginCheckTimer.Start();
                    this._isLoggedIn = true;
                    this.UserLoggedInRaised?.Invoke(this, EventArgs.Empty);
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
        ///     Gibt den CookieContainer zurück, der benutzt wird, um Aktionen im eingeloggten Status auszuführen.
        /// </summary>
        /// <seealso cref="MobileLoginCookies" />
        public CookieContainer LoginCookies { get; private set; }

        /// <summary>
        ///     Profil des Senpais.
        /// </summary>
        public User Me { get; private set; }

        /// <summary>
        ///     Gibt den CookieContainer zurück, der benutzt wird, um Aktionen im eingeloggten Status auszuführen.
        ///     Jedoch wird hierbei dem CookieContainer noch Cookies hinzugefügt, sodass die mobile Seite angezeigt wird.
        /// </summary>
        /// <seealso cref="LoginCookies" />
        public CookieContainer MobileLoginCookies
        {
            get
            {
                if (this.LoginCookies == null) return null;
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
        ///     Gibt ein Objekt zurück, mithilfe dessen alle News abgerufen werden könne.
        /// </summary>
        /// <seealso cref="AnimeMangaUpdates" />
        /// <seealso cref="FriendRequests" />
        /// <seealso cref="News" />
        /// <seealso cref="PrivateMessages" />
        public NewsCollection News
        {
            get
            {
                if (!this._updateNotifications[2] && this._newsUpdates != null) return this._newsUpdates;

                this._newsUpdates = new NewsCollection(this);
                this._updateNotifications[2] = false;

                return this._newsUpdates;
            }
        }

        /// <summary>
        ///     Gibt ein Objekt zurück, mithilfe dessen alle Privat-Nachricht-Benachrichtigungen abgerufen werden könne.
        /// </summary>
        /// <seealso cref="AnimeMangaUpdates" />
        /// <seealso cref="FriendRequests" />
        /// <seealso cref="News" />
        /// <seealso cref="PrivateMessages" />
        public PmCollection PrivateMessages
        {
            get
            {
                if (!this._updateNotifications[3] && this._pmUpdates != null) return this._pmUpdates;

                this._pmUpdates = new PmCollection(this);
                this._updateNotifications[3] = false;

                return this._pmUpdates;
            }
        }

        #endregion

        #region

        /// <summary>
        ///     Wird ausgelöst, wenn neue Anime Folgen oder Manga Kapitel vorhanden sind. (15 Minuten Intervall)
        /// </summary>
        public event AmNotificationEventHandler AmUpdateNotificationRaised;

        internal async Task<ProxerResult<bool>> CheckLogin()
        {
            ProxerResult<Tuple<string, CookieContainer>> lResult =
                await
                    HttpUtility.GetResponseErrorHandling("https://proxer.me/login?format=json&action=login",
                        this.LoginCookies,
                        this.ErrHandler, this, new Func<string, ProxerResult>[0], false);

            if (!lResult.Success)
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

        private async Task<ProxerResult> CheckNotifications()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling("https://proxer.me/notifications?format=raw&s=count",
                        this.LoginCookies, this.ErrHandler,
                        this);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            if (lResponse.StartsWith("1")) return new ProxerResult();
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
        ///     Wird ausgelöst, wenn während des Abrufens der Benachríchtigungen eine Ausnahme aufgetreten ist.
        /// </summary>
        public event ErrorDuringNotificationFetchEventHandler ErrorDuringNotificationFetch;

        /// <summary>
        ///     Zwingt die Eigenschaften sich beim nächsten Aufruf zu aktualisieren.
        /// </summary>
        /// <exception cref="WrongResponseException">Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</exception>
        public async Task<ProxerResult> ForcePropertyReload()
        {
            ProxerResult<bool> lResult;
            if ((lResult = await this.CheckLogin()).Success == false)
            {
                return new ProxerResult(lResult.Exceptions);
            }

            for (int i = 0; i < 4; i++)
                this._updateNotifications[i] = true;

            return new ProxerResult();
        }

        /// <summary>
        ///     Wird ausgelöst, wenn eine neue Freundschaftsanfrage aussteht. (15 Minuten Intervall)
        /// </summary>
        public event FriendNotificiationEventHandler FriendNotificationRaised;

        /// <summary>
        ///     Gibt alle Konferenzen des Senpais zurück.
        ///     <para>
        ///         ACHTUNG: Bei den Konferenzen muss noch
        ///         <see cref="Conference.Init">InitConference()</see>
        ///         aufgerufen werden!
        ///     </para>
        /// </summary>
        /// <exception cref="WrongResponseException">Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</exception>
        /// <exception cref="NotLoggedInException">Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</exception>
        /// <seealso cref="Login" />
        /// <returns>Alle Konferenzen, in denen der Benutzer Teilnehmer ist.</returns>
        public async Task<ProxerResult<List<Conference>>> GetAllConferences()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling("http://proxer.me/messages", this.LoginCookies, this.ErrHandler,
                        this);

            if (!lResult.Success)
                return new ProxerResult<List<Conference>>(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                HtmlDocument lDocument = new HtmlDocument();
                lDocument.LoadHtml(lResponse);
                List<Conference> lReturn = new List<Conference>();

                IEnumerable<HtmlNode> lNodes = Utility.GetAllHtmlNodes(lDocument.DocumentNode.ChildNodes)
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

                return new ProxerResult<List<Conference>>(lReturn);
            }
            catch
            {
                return
                    new ProxerResult<List<Conference>>(
                        (await ErrorHandler.HandleError(this, lResponse, false)).Exceptions);
            }
        }

        /// <summary>
        ///     Initialisiert die Benachrichtigungen.
        /// </summary>
        /// <exception cref="WrongResponseException">Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</exception>
        /// <exception cref="NotLoggedInException">Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</exception>
        /// <seealso cref="Login" />
        public ProxerResult InitNotifications()
        {
            if (!this.IsLoggedIn) return new ProxerResult(new Exception[] {new NotLoggedInException(this)});

            this._notificationCheckTimer.Start();
            this._propertyUpdateTimer.Start();

            return new ProxerResult();
        }

        /// <summary>
        ///     Loggt den Benutzer ein.
        /// </summary>
        /// <exception cref="WrongResponseException">Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</exception>
        /// <param name="username">Der Benutzername des einzuloggenden Benutzers</param>
        /// <param name="password">Das Passwort des Benutzers</param>
        /// <returns>Gibt zurück, ob der Benutzer erfolgreich eingeloggt wurde.</returns>
        public async Task<ProxerResult<bool>> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return new ProxerResult<bool>(false);

            Dictionary<string, string> postArgs = new Dictionary<string, string>
            {
                {"username", username},
                {"password", password}
            };

            ProxerResult<KeyValuePair<string, CookieContainer>> lResult =
                await
                    HttpUtility.PostResponseErrorHandling("https://proxer.me/login?format=json&action=login",
                        postArgs, this.LoginCookies, this.ErrHandler, this, new Func<string, ProxerResult>[0], false);

            if (!lResult.Success)
                return new ProxerResult<bool>(lResult.Exceptions);

            string lResponse = lResult.Result.Key;
            this.LoginCookies = lResult.Result.Value;

            try
            {
                Dictionary<string, string> responseDes =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                if (responseDes["error"].Equals("0"))
                {
                    this._userId = Convert.ToInt32(responseDes["uid"]);

                    //Avatar einfügen
                    this.Me = new User(username, this._userId, this);
                    this.IsLoggedIn = true;

                    return new ProxerResult<bool>(true);
                }
                this.IsLoggedIn = false;

                return new ProxerResult<bool>(false);
            }
            catch
            {
                return new ProxerResult<bool>(ErrorHandler.HandleError(this, lResponse).Exceptions);
            }
        }

        /// <summary>
        ///     Wird ausgelöst, wenn neue ungelesene News vorhanden sind. (15 Minuten Intervall)
        /// </summary>
        public event NewsNotificationEventHandler NewsNotificationRaised;


        /// <summary>
        ///     Wird bei allen Benachrichtigungen ausgelöst. (15 Minuten Intervall)
        /// </summary>
        public event NotificationEventHandler NotificationRaised;

        /// <summary>
        ///     Wird ausgelöst, wenn ungelesene PMs vorhanden sind. (15 Minuten Intervall)
        /// </summary>
        public event PmNotificationEventHandler PmNotificationRaised;

        /// <summary>
        ///     Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> sich eingeloggt hat.
        /// </summary>
        public event EventHandler UserLoggedInRaised;

        /// <summary>
        ///     Wird ausgelöst, wenn die Login-Cookies verfallen sind. (15 Minuten Intervall)
        /// </summary>
        public event EventHandler UserLoggedOutRaised;

        #endregion
    }
}