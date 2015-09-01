using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Timers;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Proxer.API.Community;
using Proxer.API.EventArguments;
using Proxer.API.Notifications;
using Proxer.API.Utilities;

namespace Proxer.API
{
    /// <summary>
    ///     Der Benutzer der Anwendung an sich
    /// </summary>
    public class Senpai
    {
        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void AmNotificationEventHandler(Senpai sender, AmNotificationEventArgs e);

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void FriendNotificiationEventHandler(Senpai sender, FriendNotificationEventArgs e);

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void NewsNotificationEventHandler(Senpai sender, NewsNotificationEventArgs e);

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void NotificationEventHandler(Senpai sender, INotificationEventArgs e);

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void PmNotificationEventHandler(Senpai sender, PmNotificationEventArgs e);

        private readonly Timer _loginCheckTimer;
        private readonly Timer _notificationCheckTimer;
        private readonly Timer _notificationUpdateCheckTimer;
        private List<AnimeMangaUpdateObject> _animeMangaUpdates;
        private bool _checkAnimeMangaUpdate;
        private bool _checkFriendUpdates;

        //für die NotificationObject-Listen Eigenschaften
        private bool _checkNewsUpdate;
        private bool _checkPmUpdate;
        private List<FriendRequestObject> _friendUpdates;
        private bool _loggedIn;
        private List<NewsObject> _newsUpdates;
        private List<PmObject> _pmUpdates;
        private int _userId;


        /// <summary>
        ///     Initialisiert die Klasse
        /// </summary>
        public Senpai()
        {
            this.ErrHandler = new ErrorHandler();

            this._loggedIn = false;
            this.LoginCookies = new CookieContainer();

            this._loginCheckTimer = new Timer
            {
                AutoReset = true,
                Interval = (new TimeSpan(0, 45, 0)).TotalMilliseconds
            };
            this._loginCheckTimer.Elapsed += (s, eArgs) =>
            {
                this._loginCheckTimer.Interval = (new TimeSpan(0, 30, 0)).TotalMilliseconds;
                this.CheckLogin();
            };

            this._notificationCheckTimer = new Timer
            {
                AutoReset = true,
                Interval = (new TimeSpan(0, 15, 0)).TotalMilliseconds
            };
            this._notificationCheckTimer.Elapsed += (s, eArgs) => { this.CheckNotifications(); };

            this._notificationUpdateCheckTimer = new Timer(1) {AutoReset = true};
            this._notificationUpdateCheckTimer.Elapsed += (s, eArgs) =>
            {
                this._notificationUpdateCheckTimer.Interval = (new TimeSpan(0, 10, 0)).TotalMilliseconds;
                this._checkAnimeMangaUpdate = true;
                this._checkNewsUpdate = true;
                this._checkPmUpdate = true;
            };
        }

        #region Properties

        /// <summary>
        ///     Gibt alle Anime und Manga Benachrichtigungen in einer Liste zurück
        /// </summary>
        public List<AnimeMangaUpdateObject> AnimeMangaUpdates
        {
            get
            {
                if (this._checkAnimeMangaUpdate) this.GetAllAnimeMangaUpdates();
                return this._animeMangaUpdates;
            }
        }

        /// <summary>
        ///     Gibt den Error-Handler zurück, der benutzt wird, um Fehler in Serverantworten zu bearbeiten und frühzeitig zu
        ///     erkennen
        /// </summary>
        public ErrorHandler ErrHandler { get; private set; }

        /// <summary>
        ///     Gibt alle Freundschaftsanfragen in einer Liste zurück
        /// </summary>
        public List<FriendRequestObject> FriendRequests
        {
            get
            {
                if (this._checkFriendUpdates) this.GetAllFriendUpdates();
                return this._friendUpdates;
            }
        }


        /// <summary>
        ///     Gibt an, ob der Benutzter noch eingeloggt ist, wird aber nicht überprüft (nur durch Timer alle 30 Minuten)
        /// </summary>
        public bool LoggedIn
        {
            get { return this._loggedIn; }
            private set
            {
                if (value)
                {
                    this._loginCheckTimer.Start();
                    this._loggedIn = true;
                }
                else
                {
                    this._loginCheckTimer.Stop();
                    this._notificationCheckTimer.Stop();
                    this._notificationUpdateCheckTimer.Stop();
                    this._loggedIn = false;
                }
            }
        }

        /// <summary>
        ///     Gibt den CookieContainer zurück, der benutzt wird, um Aktionen im eingeloggten Status auszuführen
        /// </summary>
        public CookieContainer LoginCookies { get; private set; }

        /// <summary>
        ///     Profil des Senpais
        /// </summary>
        public User Me { get; private set; }

        /// <summary>
        ///     Gibt die letzten 15 News in einer Liste zurück
        /// </summary>
        public List<NewsObject> News
        {
            get
            {
                if (this._checkNewsUpdate) this.GetAllNewsUpdates();
                return this._newsUpdates;
            }
        }

        /// <summary>
        ///     Gibt alle Privat Nachricht Benachrichtigungen in einer Liste zurück
        /// </summary>
        public List<PmObject> PrivateMessages
        {
            get
            {
                if (this._checkPmUpdate) this.GetAllPmUpdates();
                return this._pmUpdates;
            }
        }

        #endregion

        #region

        /// <summary>
        ///     Loggt den Benutzer ein
        /// </summary>
        /// <param name="username">Der Benutzername des zu einloggenden Benutzers</param>
        /// <param name="password">Das Passwort des Benutzers</param>
        /// <returns>Gibt zurück, ob der Benutzer erfolgreich eingeloggt wurde</returns>
        public bool Login(string username, string password)
        {
            if (this.LoggedIn) return false;

            Dictionary<string, string> postArgs = new Dictionary<string, string>
            {
                {"username", username},
                {"password", password}
            };
            string lResponse = HttpUtility.PostWebRequestResponse("https://proxer.me/login?format=json&action=login",
                this.LoginCookies, postArgs);

            if (Utility.CheckForCorrectResponse(lResponse, this.ErrHandler))
            {
                try
                {
                    Dictionary<string, string> responseDes =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                    if (responseDes["error"].Equals("0"))
                    {
                        this._userId = Convert.ToInt32(responseDes["uid"]);
                        this.Me = new User(username, this._userId, this);
                        this.LoggedIn = true;

                        return true;
                    }
                    this.LoggedIn = false;

                    return false;
                }
                catch (Exception)
                {
                    this.ErrHandler.Add(lResponse);
                }
            }

            return false;
        }

        /// <summary>
        ///     Checkt per API, ob der Benutzer noch eingeloggt ist
        /// </summary>
        /// <returns></returns>
        public bool CheckLogin()
        {
            if (this.LoggedIn)
            {
                string lResponse = HttpUtility.GetWebRequestResponse(
                    "https://proxer.me/login?format=json&action=login", this.LoginCookies);

                if (Utility.CheckForCorrectResponse(lResponse, this.ErrHandler))
                {
                    try
                    {
                        Dictionary<string, string> responseDes =
                            JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                        if (responseDes["error"].Equals("0"))
                        {
                            this.LoggedIn = true;
                            return true;
                        }
                        this.LoggedIn = false;
                        if (this.UserLoggedOutRaised != null) this.UserLoggedOutRaised(this, new EventArgs());
                        return false;
                    }
                    catch (Exception)
                    {
                        this.ErrHandler.Add(lResponse);
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     Initialisiert die Benachrichtigungen
        /// </summary>
        /// <returns></returns>
        public bool InitNotifications()
        {
            if (this.LoggedIn)
            {
                this.CheckNotifications();

                this._notificationCheckTimer.Start();
                this._notificationUpdateCheckTimer.Start();

                return true;
            }
            return false;
        }

        /// <summary>
        ///     Nach dem Aufruf, wenn eine Eigenschaft aufgerufen wird, wird dessen Wert neu berechnet.
        /// </summary>
        public void ForcePropertyReload()
        {
            this.CheckLogin();
            this._checkAnimeMangaUpdate = true;
            this._checkNewsUpdate = true;
            this._checkPmUpdate = true;
        }

        /// <summary>
        ///     Gibt alle Konferenzen des Senpais zurück. ACHTUNG: Bei den Konferenzen muss noch initConference() aufgerufen
        ///     werden!
        /// </summary>
        /// <returns></returns>
        public List<Conference> GetAllConferences()
        {
            if (this.LoggedIn)
            {
                string lResponse =
                    HttpUtility.GetWebRequestResponse("http://proxer.me/messages", this.LoginCookies)
                        .Replace("</link>", "")
                        .Replace("\n", "");

                if (Utility.CheckForCorrectResponse(lResponse, this.ErrHandler))
                {
                    try
                    {
                        HtmlDocument lDocument = new HtmlDocument();
                        lDocument.LoadHtml(lResponse);

                        if (!lDocument.ParseErrors.Any())
                        {
                            List<Conference> lReturn = new List<Conference>();

                            HtmlNodeCollection lNodes =
                                lDocument.DocumentNode.SelectNodes("//a[@class='conferenceGrid ']");
                            if (lNodes != null)
                            {
                                lReturn.AddRange(from curNode in lNodes
                                    let lId =
                                        Convert.ToInt32(
                                            Utility.GetTagContents(curNode.Attributes["href"].Value, "/messages?id=",
                                                "#top")[0])
                                    let lTitle = curNode.FirstChild.InnerText
                                    select new Conference(lTitle, lId, this));
                            }

                            return lReturn;
                        }
                    }
                    catch (Exception)
                    {
                        this.ErrHandler.Add(lResponse);
                    }
                }
            }

            return null;
        }


        private void CheckNotifications()
        {
            if (this.LoggedIn)
            {
                string lResponse =
                    HttpUtility.GetWebRequestResponse("https://proxer.me/notifications?format=raw&s=count",
                        this.LoginCookies);

                if (lResponse.StartsWith("0"))
                {
                    string[] lResponseSplit = lResponse.Split('#');

                    if (!lResponseSplit[2].Equals("0"))
                    {
                        if (this.PmNotificationRaised != null)
                            this.PmNotificationRaised(this,
                                new PmNotificationEventArgs(Convert.ToInt32(lResponseSplit[2]), this));
                        if (this.NotificationRaised != null)
                            this.NotificationRaised(this,
                                new PmNotificationEventArgs(Convert.ToInt32(lResponseSplit[2]), this));
                        this._checkPmUpdate = true;
                    }
                    if (!lResponseSplit[3].Equals("0"))
                    {
                        if (this.FriendNotificationRaised != null)
                            this.FriendNotificationRaised(this,
                                new FriendNotificationEventArgs(Convert.ToInt32(lResponseSplit[3]), this));
                        if (this.NotificationRaised != null)
                            this.NotificationRaised(this,
                                new FriendNotificationEventArgs(Convert.ToInt32(lResponseSplit[3]), this));
                        this._checkFriendUpdates = true;
                    }
                    if (!lResponseSplit[4].Equals("0"))
                    {
                        if (this.NewsNotificationRaised != null)
                            this.NewsNotificationRaised(this,
                                new NewsNotificationEventArgs(Convert.ToInt32(lResponseSplit[4]), this));
                        if (this.NotificationRaised != null)
                            this.NotificationRaised(this,
                                new NewsNotificationEventArgs(Convert.ToInt32(lResponseSplit[4]), this));
                        this._checkNewsUpdate = true;
                    }
                    if (!lResponseSplit[5].Equals("0"))
                    {
                        if (this.AmUpdateNotificationRaised != null)
                            this.AmUpdateNotificationRaised(this,
                                new AmNotificationEventArgs(Convert.ToInt32(lResponseSplit[5]), this));
                        if (this.NotificationRaised != null)
                            this.NotificationRaised(this,
                                new AmNotificationEventArgs(Convert.ToInt32(lResponseSplit[5]), this));
                        this._checkAnimeMangaUpdate = true;
                    }
                }
            }
        }

        private void GetAllAnimeMangaUpdates()
        {
            if (!this.LoggedIn) return;
            HtmlDocument lDocument = new HtmlDocument();
            string lResponse =
                HttpUtility.GetWebRequestResponse(
                    "https://proxer.me/components/com_proxer/misc/notifications_misc.php", this.LoginCookies);

            if (!Utility.CheckForCorrectResponse(lResponse, this.ErrHandler)) return;
            try
            {
                lDocument.LoadHtml(lResponse);

                if (lDocument.ParseErrors.Any()) return;
                HtmlNodeCollection lNodes =
                    lDocument.DocumentNode.SelectNodes("//a[@class='notificationList']");

                if (lNodes == null) return;
                this._animeMangaUpdates = new List<AnimeMangaUpdateObject>();
                foreach (HtmlNode curNode in lNodes.Where(curNode => curNode.InnerText.StartsWith("Lesezeichen:")))
                {
                    string lName;
                    int lNumber;

                    int lId = Convert.ToInt32(curNode.Id.Substring(12));
                    string lMessage = curNode.ChildNodes["u"].InnerText;
                    Uri lLink = new Uri("https://proxer.me" + curNode.Attributes["href"].Value);

                    if (lMessage.IndexOf('#') != -1)
                    {
                        lName = lMessage.Split('#')[0];
                        if (!int.TryParse(lMessage.Split('#')[1], out lNumber)) lNumber = -1;
                    }
                    else
                    {
                        lName = "";
                        lNumber = -1;
                    }

                    this.AnimeMangaUpdates.Add(new AnimeMangaUpdateObject(lMessage, lName, lNumber,
                        lLink,
                        lId));
                }
                this._checkAnimeMangaUpdate = false;
            }
            catch (NullReferenceException)
            {
                this.ErrHandler.Add(lResponse);
            }
        }

        private void GetAllNewsUpdates()
        {
            if (!this.LoggedIn) return;
            string lResponse =
                HttpUtility.GetWebRequestResponse("https://proxer.me/notifications?format=json&s=news&p=1",
                    this.LoginCookies);
            if (!lResponse.StartsWith("{\"error\":0")) return;
            this._newsUpdates = new List<NewsObject>();
            Dictionary<string, List<NewsObject>> lDeserialized =
                JsonConvert.DeserializeObject<Dictionary<string, List<NewsObject>>>("{" +
                                                                                    lResponse.Substring(
                                                                                        "{\"error\":0,".Length));
            this._newsUpdates = lDeserialized["notifications"];
            this._checkNewsUpdate = false;
        }

        private void GetAllPmUpdates()
        {
            if (!this.LoggedIn) return;
            HtmlDocument lDocument = new HtmlDocument();
            string lResponse =
                (HttpUtility.GetWebRequestResponse("https://proxer.me/messages?format=raw&s=notification",
                    this.LoginCookies)).Replace("</link>", "").Replace("\n", "");

            if (!Utility.CheckForCorrectResponse(lResponse, this.ErrHandler)) return;
            try
            {
                lDocument.LoadHtml(lResponse);

                if (lDocument.ParseErrors.Any()) return;
                HtmlNodeCollection lNodes =
                    lDocument.DocumentNode.SelectNodes("//a[@class='conferenceList']");

                if (lNodes != null)
                {
                    this._pmUpdates = new List<PmObject>();
                    foreach (HtmlNode curNode in lNodes)
                    {
                        string lTitel;
                        string[] lDatum;
                        if (curNode.ChildNodes[1].Name.ToLower().Equals("img"))
                        {
                            lTitel = curNode.ChildNodes[0].InnerText;
                            lDatum = curNode.ChildNodes[1].InnerText.Split('.');

                            DateTime lTimeStamp = new DateTime(Convert.ToInt32(lDatum[2]),
                                Convert.ToInt32(lDatum[1]), Convert.ToInt32(lDatum[0]));
                            int lId =
                                Convert.ToInt32(curNode.Attributes["href"].Value.Substring(13,
                                    curNode.Attributes["href"].Value.Length - 17));

                            this._pmUpdates.Add(new PmObject(lId, lTitel, lTimeStamp));
                        }
                        else
                        {
                            lTitel = curNode.ChildNodes[0].InnerText;
                            lDatum = curNode.ChildNodes[1].InnerText.Split('.');

                            DateTime lTimeStamp = new DateTime(Convert.ToInt32(lDatum[2]),
                                Convert.ToInt32(lDatum[1]), Convert.ToInt32(lDatum[0]));
                            int lId =
                                Convert.ToInt32(curNode.Attributes["href"].Value.Substring(13,
                                    curNode.Attributes["href"].Value.Length - 17));

                            this._pmUpdates.Add(new PmObject(lTitel, lId, lTimeStamp));
                        }
                    }
                }

                this._checkPmUpdate = false;
            }
            catch (NullReferenceException)
            {
                this.ErrHandler.Add(lResponse);
            }
        }

        private void GetAllFriendUpdates()
        {
            if (this.LoggedIn)
            {
                HtmlDocument lDocument = new HtmlDocument();
                string lResponse =
                    (HttpUtility.GetWebRequestResponse("https://proxer.me/user/my/connections?format=raw",
                        this.LoginCookies))
                        .Replace("</link>", "").Replace("\n", "");

                if (Utility.CheckForCorrectResponse(lResponse, this.ErrHandler))
                {
                    try
                    {
                        lDocument.LoadHtml(lResponse);

                        if (lDocument.ParseErrors.Any()) return;
                        HtmlNodeCollection lNodes = lDocument.DocumentNode.SelectNodes("//tr");

                        if (lNodes == null) return;
                        this._friendUpdates = new List<FriendRequestObject>();
                        foreach (HtmlNode curNode in lNodes)
                        {
                            if (!curNode.Id.StartsWith("entry") ||
                                !curNode.FirstChild.FirstChild.Attributes["class"].Value.Equals("accept")) continue;
                            int lUserId = Convert.ToInt32(curNode.Id.Replace("entry", ""));
                            string lUserName = curNode.InnerText.Split("  ".ToCharArray())[0];
                            string lDescription = curNode.ChildNodes[3].ChildNodes[1].InnerText;
                            string[] lDatumSplit = curNode.ChildNodes[4].InnerText.Split('-');
                            DateTime lDatum = new DateTime(Convert.ToInt32(lDatumSplit[0]),
                                Convert.ToInt32(lDatumSplit[1]), Convert.ToInt32(lDatumSplit[2]));
                            bool lOnline =
                                curNode.ChildNodes[1].ChildNodes[1].FirstChild.Attributes["src"].Value
                                    .Equals("/images/misc/onlineicon.png");

                            this._friendUpdates.Add(new FriendRequestObject(lUserName, lUserId, lDescription,
                                lDatum, lOnline, this));
                        }
                        this._checkFriendUpdates = false;
                    }
                    catch (NullReferenceException)
                    {
                        this.ErrHandler.Add(lResponse);
                    }
                }
            }
        }

        /// <summary>
        ///     Wird bei allen Benachrichtigungen aufgerufen(30 Minuten Intervall)
        /// </summary>
        public event NotificationEventHandler NotificationRaised;

        /// <summary>
        ///     Wird aufgerufen, wenn eine neue Freundschaftsanfrage aussteht(30 Minuten Intervall)
        /// </summary>
        public event FriendNotificiationEventHandler FriendNotificationRaised;

        /// <summary>
        ///     Wird aufgerufen, wenn neue ungelesene News vorhanden sind(30 Minuten Intervall)
        /// </summary>
        public event NewsNotificationEventHandler NewsNotificationRaised;

        /// <summary>
        ///     Wird aufgerufen, wenn ungelesene PMs vorhanden sind(30 Minuten Intervall)
        /// </summary>
        public event PmNotificationEventHandler PmNotificationRaised;

        /// <summary>
        ///     Wird aufgerufen, wenn neue Anime Folgen oder Manga Kapitel vorhanden sind(30 Minuten Intervall)
        ///     ACHTUNG: Kann auch aufgerufen werden, wenn z.B. eine Freundschaftsanfrage angenommen wurde(Wird versucht zu fixen)
        /// </summary>
        public event AmNotificationEventHandler AmUpdateNotificationRaised;

        /// <summary>
        ///     Wird aufgerufen, wenn die Cookies verfallen sind(30 Minuten Intervall)
        /// </summary>
        public event EventHandler UserLoggedOutRaised;

        #endregion
    }
}