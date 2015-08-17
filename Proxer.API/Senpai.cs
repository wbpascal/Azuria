using Newtonsoft.Json;
using Proxer.API.EventArguments;
using Proxer.API.Notifications;
using Proxer.API.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace Proxer.API
{
    /// <summary>
    /// 
    /// </summary>
    public class Senpai
    {
        private int userID;
        private bool loggedIn;
        private string username;
        private Timer notificationCheckTimer;
        private Timer loginCheckTimer;
        private Timer notificationUpdateCheckTimer;

        //für die NotificationObject-Listen Eigenschaften
        private bool checkNewsUpdate;
        private bool checkAnimeMangaUpdate;
        private bool checkPMUpdate;
        private bool checkFriendUpdates;
        private List<NewsObject> newsUpdates;
        private List<AnimeMangaUpdateObject> animeMangaUpdates;
        private List<PMObject> pmUpdates;
        private List<FriendRequestObject> friendUpdates;

        #region Events + Handler
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void NotificationEventHandler(Senpai sender, INotificationEventArgs e);
        /// <summary>
        /// Wird bei allen Benachrichtigungen aufgerufen(30 Minuten Intervall)
        /// </summary>
        public event NotificationEventHandler Notification_Raised;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void FriendNotificiationEventHandler(Senpai sender, FriendNotificationEventArgs e);
        /// <summary>
        /// Wird aufgerufen, wenn eine neue Freundschaftsanfrage aussteht(30 Minuten Intervall)
        /// </summary>
        public event FriendNotificiationEventHandler FriendNotification_Raised;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void NewsNotificationEventHandler(Senpai sender, NewsNotificationEventArgs e);
        /// <summary>
        /// Wird aufgerufen, wenn neue ungelesene News vorhanden sind(30 Minuten Intervall)
        /// </summary>
        public event NewsNotificationEventHandler NewsNotification_Raised;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void PMNotificationEventHandler(Senpai sender, PMNotificationEventArgs e);
        /// <summary>
        /// Wird aufgerufen, wenn ungelesene PMs vorhanden sind(30 Minuten Intervall)
        /// </summary>
        public event PMNotificationEventHandler PMNotification_Raised;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void AMNotificationEventHandler(Senpai sender, AMNotificationEventArgs e);
        /// <summary>
        /// Wird aufgerufen, wenn neue Anime Folgen oder Manga Kapitel vorhanden sind(30 Minuten Intervall)
        /// ACHTUNG: Kann auch aufgerufen werden, wenn z.B. eine Freundschaftsanfrage angenommen wurde(Wird versucht zu fixen)
        /// </summary>
        public event AMNotificationEventHandler AMUpdateNotification_Raised;

        /// <summary>
        /// Wird aufgerufen, wenn die Cookies verfallen sind(30 Minuten Intervall)
        /// </summary>
        public event EventHandler UserLoggedOut_Raised;
        #endregion


        /// <summary>
        /// Initialisiert die Klasse
        /// </summary>
        public Senpai()
        {
            this.ErrHandler = new ErrorHandler();

            this.loggedIn = false;
            this.LoginCookies = new CookieContainer();

            this.loginCheckTimer = new Timer();
            this.loginCheckTimer.AutoReset = true;
            this.loginCheckTimer.Interval = (new TimeSpan(0, 45, 0)).TotalMilliseconds;
            this.loginCheckTimer.Elapsed += (s, eArgs) =>
            {
                this.loginCheckTimer.Interval = (new TimeSpan(0, 30, 0)).TotalMilliseconds;
                this.checkLogin();
            };

            this.notificationCheckTimer = new Timer();
            this.notificationCheckTimer.AutoReset = true;
            this.notificationCheckTimer.Interval = (new TimeSpan(0, 15, 0)).TotalMilliseconds;
            this.notificationCheckTimer.Elapsed += (s, eArgs) => { checkNotifications(); };

            this.notificationUpdateCheckTimer = new Timer(0);
            this.notificationUpdateCheckTimer.AutoReset = true;
            this.notificationUpdateCheckTimer.Elapsed += (s, eArgs) =>
            {
                this.notificationUpdateCheckTimer.Interval = (new TimeSpan(0, 10, 0)).TotalMilliseconds;
                this.checkAnimeMangaUpdate = true;
                this.checkNewsUpdate = true;
                this.checkPMUpdate = true;
            };
        }


        /// <summary>
        /// Gibt an, ob der Benutzter noch eingeloggt ist, wird aber nicht überprüft (nur durch Timer alle 30 Minuten)
        /// </summary>
        public bool LoggedIn
        {
            get
            {
                return this.loggedIn;
            }
            private set
            {
                if (value)
                {
                    loginCheckTimer.Start();
                    this.loggedIn = true;
                }
                else
                {
                    loginCheckTimer.Stop();
                    notificationCheckTimer.Stop();
                    notificationUpdateCheckTimer.Stop();
                    this.loggedIn = false;
                }
            }
        }
        /// <summary>
        /// Gibt alle Anime und Manga Benachrichtigungen in einer Liste zurück
        /// </summary>
        public List<AnimeMangaUpdateObject> AnimeMangaUpdates
        {
            get
            {
                if (checkAnimeMangaUpdate) getAllAnimeMangaUpdates();
                return animeMangaUpdates;
            }
        }
        /// <summary>
        /// Gibt die letzten 15 News in einer Liste zurück
        /// </summary>
        public List<NewsObject> News
        {
            get
            {
                if (checkNewsUpdate) getAllNewsUpdates();
                return newsUpdates;
            }
        }
        /// <summary>
        /// Gibt alle Privat Nachricht Benachrichtigungen in einer Liste zurück
        /// </summary>
        public List<PMObject> PrivateMessages
        {
            get
            {
                if (checkPMUpdate) getAllPMUpdates();
                return pmUpdates;
            }
        }
        /// <summary>
        /// Gibt alle Freundschaftsanfragen in einer Liste zurück
        /// </summary>
        public List<FriendRequestObject> FriendRequests
        {
            get
            {
                if (checkFriendUpdates) getAllFriendUpdates();
                return friendUpdates;
            }
        }
        /// <summary>
        /// Gibt den CookieContainer zurück, der benutzt wird, um Aktionen im eingeloggten Status auszuführen
        /// </summary>
        public CookieContainer LoginCookies { get; private set; }
        /// <summary>
        /// Profil des Senpais
        /// </summary>
        public User Me { get; private set; }

        /// <summary>
        /// Gibt den Error-Handler zurück, der benutzt wird, um Fehler in Serverantworten zu bearbeiten und frühzeitig zu erkennen
        /// </summary>
        internal ErrorHandler ErrHandler { get; private set; }


        /// <summary>
        /// Loggt den Benutzer ein
        /// </summary>
        /// <param name="username">Der Benutzername des zu einloggenden Benutzers</param>
        /// <param name="password">Das Passwort des Benutzers</param>
        /// <returns>Gibt zurück, ob der Benutzer erfolgreich eingeloggt wurde</returns>
        public bool login(string username, string password)
        {
            if (LoggedIn) return false;

            Dictionary<string, string> postArgs = new Dictionary<string, string>
            {
                {"username", username},
                {"password", password}
            };
            string lResponse = HttpUtility.PostWebRequestResponse("https://proxer.me/login?format=json&action=login", LoginCookies, postArgs);

            if (Utilities.Utility.checkForCorrectResponse(lResponse, this.ErrHandler))
            {
                try
                {
                    Dictionary<string, string> responseDes = JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                    if (responseDes["error"].Equals("0"))
                    {
                        this.userID = Convert.ToInt32(responseDes["uid"]);
                        this.username = username;
                        this.Me = new User(username, userID, this);
                        LoggedIn = true;

                        return true;
                    }
                    else
                    {
                        LoggedIn = false;

                        return false;
                    }
                }
                catch (Exception)
                {
                    this.ErrHandler.add(lResponse);
                }
            }

            return false;
        }
        /// <summary>
        /// Checkt per API, ob der Benutzer noch eingeloggt ist
        /// </summary>
        public bool checkLogin()
        {
            if (LoggedIn)
            {
                string lResponse = HttpUtility.GetWebRequestResponse("https://proxer.me/login?format=json&action=login", LoginCookies);

                if (Utilities.Utility.checkForCorrectResponse(lResponse, this.ErrHandler))
                {
                    try
                    {
                        Dictionary<string, string> responseDes = JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                        if (responseDes["error"].Equals("0"))
                        {
                            LoggedIn = true;
                            return true;
                        }
                        else
                        {
                            LoggedIn = false;
                            if (UserLoggedOut_Raised != null) UserLoggedOut_Raised(this, new System.EventArgs());
                            return false;
                        }
                    }
                    catch (Exception)
                    {
                        this.ErrHandler.add(lResponse);
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// Checkt, ob neue Benachrichtigungen vorhanden sind
        /// </summary>
        private void checkNotifications()
        {
            if (LoggedIn)
            {
                string lResponse = HttpUtility.GetWebRequestResponse("https://proxer.me/notifications?format=raw&s=count", LoginCookies);

                if (Utilities.Utility.checkForCorrectResponse(lResponse, this.ErrHandler) && lResponse.StartsWith("0"))
                {
                    try
                    {
                        string[] lResponseSplit = lResponse.Split('#');

                        if (!lResponseSplit[2].Equals("0"))
                        {
                            if (PMNotification_Raised != null) PMNotification_Raised(this, new PMNotificationEventArgs(Convert.ToInt32(lResponseSplit[2]), this));
                            if (Notification_Raised != null) Notification_Raised(this, new PMNotificationEventArgs(Convert.ToInt32(lResponseSplit[2]), this));
                            this.checkPMUpdate = true;
                        }
                        if (!lResponseSplit[3].Equals("0"))
                        {
                            if (FriendNotification_Raised != null) FriendNotification_Raised(this, new FriendNotificationEventArgs(Convert.ToInt32(lResponseSplit[3]), this));
                            if (Notification_Raised != null) Notification_Raised(this, new FriendNotificationEventArgs(Convert.ToInt32(lResponseSplit[3]), this));
                            this.checkFriendUpdates = true;
                        }
                        if (!lResponseSplit[4].Equals("0"))
                        {
                            if (NewsNotification_Raised != null) NewsNotification_Raised(this, new NewsNotificationEventArgs(Convert.ToInt32(lResponseSplit[4]), this));
                            if (Notification_Raised != null) Notification_Raised(this, new NewsNotificationEventArgs(Convert.ToInt32(lResponseSplit[4]), this));
                            this.checkNewsUpdate = true;
                        }
                        if (!lResponseSplit[5].Equals("0"))
                        {
                            if (AMUpdateNotification_Raised != null) AMUpdateNotification_Raised(this, new AMNotificationEventArgs(Convert.ToInt32(lResponseSplit[5]), this));
                            if (Notification_Raised != null) Notification_Raised(this, new AMNotificationEventArgs(Convert.ToInt32(lResponseSplit[5]), this));
                            this.checkAnimeMangaUpdate = true;
                        }
                    }
                    catch (Exception)
                    {
                        this.ErrHandler.add(lResponse);
                    }
                }
            }
        }
        /// <summary>
        /// Initialisiert die Benachrichtigungen
        /// </summary>
        public bool initNotifications()
        {
            if (LoggedIn)
            {
                checkNotifications();

                notificationCheckTimer.Start();
                notificationUpdateCheckTimer.Start();

                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Wenn man unbedingt möchte, dass die Eigenschaften aktualisiert werden.
        /// </summary>
        public void forcePropertyReload()
        {
            this.checkLogin();
            this.checkAnimeMangaUpdate = true;
            this.checkNewsUpdate = true;
            this.checkPMUpdate = true;
        }

        /// <summary>
        /// (Vorläufig, nicht ausführlich getestet)
        /// Benutzt um ALLE Anime und Manga Benachrichtigungen in die vorgesehene Eigenschaft einzutragen.
        /// Wird nur in initNotifications() und alle 30 Minuten, falls die AnimeMangaUpdates-Eigenschaft abgerufen wird, benutzt.
        /// </summary>
        private void getAllAnimeMangaUpdates()
        {
            if (LoggedIn)
            {
                HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
                string lResponse = HttpUtility.GetWebRequestResponse("https://proxer.me/components/com_proxer/misc/notifications_misc.php", LoginCookies);

                if (Utilities.Utility.checkForCorrectResponse(lResponse, this.ErrHandler))
                {
                    try
                    {
                        lDocument.LoadHtml(lResponse);

                        if (lDocument.ParseErrors.Count() == 0)
                        {
                            HtmlAgilityPack.HtmlNodeCollection lNodes = lDocument.DocumentNode.SelectNodes("//a[@class='notificationList']");

                            if (lNodes != null)
                            {
                                this.animeMangaUpdates = new List<AnimeMangaUpdateObject>();
                                foreach (HtmlAgilityPack.HtmlNode curNode in lNodes)
                                {
                                    if (curNode.InnerText.StartsWith("Lesezeichen:"))
                                    {
                                        string lName;
                                        int lNumber;

                                        int lID = Convert.ToInt32(curNode.Id.Substring(12));
                                        string lMessage = curNode.ChildNodes["u"].InnerText;
                                        Uri lLink = new Uri("https://proxer.me" + curNode.Attributes["href"].Value);

                                        if (lMessage.IndexOf('#') != -1)
                                        {
                                            lName = lMessage.Split('#')[0];
                                            if (!Int32.TryParse(lMessage.Split('#')[1], out lNumber)) lNumber = -1;
                                        }
                                        else
                                        {
                                            lName = "";
                                            lNumber = -1;
                                        }

                                        this.AnimeMangaUpdates.Add(new AnimeMangaUpdateObject(lMessage, lName, lNumber, lLink, lID));
                                    }
                                }
                                this.checkAnimeMangaUpdate = false;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        this.ErrHandler.add(lResponse);
                    }
                }
            }
        }
        /// <summary>
        /// Benutzt um die letzten 15 News abzurufen und sie in die vorgesehene Eigenschaft einzutragen.
        /// Nur in initNotifications() und alle 30 Minuten, falls die News-Eigenschaft abgerufen wird, benutzt.
        /// </summary>
        private void getAllNewsUpdates()
        {
            if (LoggedIn)
            {
                string lResponse = HttpUtility.GetWebRequestResponse("https://proxer.me/notifications?format=json&s=news&p=1", LoginCookies);
                if (lResponse.StartsWith("{\"error\":0"))
                {
                    this.newsUpdates = new List<NewsObject>();
                    Dictionary<string, List<NewsObject>> lDeserialized = JsonConvert.DeserializeObject<Dictionary<string, List<NewsObject>>>("{" + lResponse.Substring("{\"error\":0,".Length));
                    newsUpdates = lDeserialized["notifications"];
                    this.checkNewsUpdate = false;
                }
            }
        }
        /// <summary>
        /// (Vorläufig, nicht ausführlich getestet)
        /// Benutzt um ALLE Privat Nachricht Benachrichtigungen abzurufen und sie in die vorgesehene Eigenschaft einzutragen.
        /// Nur in initNotifications() und alle 30 Minuten, falls die PMUpdates-Eigenschaft abgerufen wird, benutzt.
        /// </summary>
        private void getAllPMUpdates()
        {
            if (this.LoggedIn)
            {
                HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
                string lResponse = (HttpUtility.GetWebRequestResponse("https://proxer.me/messages?format=raw&s=notification", this.LoginCookies)).Replace("</link>", "").Replace("\n", "");

                if (Utilities.Utility.checkForCorrectResponse(lResponse, this.ErrHandler))
                {
                    try
                    {
                        lDocument.LoadHtml(lResponse);

                        if (lDocument.ParseErrors.Count() == 0)
                        {
                            HtmlAgilityPack.HtmlNodeCollection lNodes = lDocument.DocumentNode.SelectNodes("//a[@class='conferenceList']");

                            if (lNodes != null)
                            {
                                this.pmUpdates = new List<PMObject>();
                                foreach (HtmlAgilityPack.HtmlNode curNode in lNodes)
                                {
                                    string lTitel;
                                    string[] lDatum;
                                    if (curNode.ChildNodes[1].Name.ToLower().Equals("img"))
                                    {
                                        lTitel = curNode.ChildNodes[0].InnerText;
                                        lDatum = curNode.ChildNodes[1].InnerText.Split('.');

                                        DateTime lTimeStamp = new DateTime(Convert.ToInt32(lDatum[2]), Convert.ToInt32(lDatum[1]), Convert.ToInt32(lDatum[0]));
                                        int lID = Convert.ToInt32(curNode.Attributes["href"].Value.Substring(13, curNode.Attributes["href"].Value.Length - 17));

                                        this.pmUpdates.Add(new PMObject(lID, lTitel, lTimeStamp));
                                    }
                                    else
                                    {
                                        lTitel = curNode.ChildNodes[0].InnerText;
                                        lDatum = curNode.ChildNodes[1].InnerText.Split('.');

                                        DateTime lTimeStamp = new DateTime(Convert.ToInt32(lDatum[2]), Convert.ToInt32(lDatum[1]), Convert.ToInt32(lDatum[0]));
                                        int lID = Convert.ToInt32(curNode.Attributes["href"].Value.Substring(13, curNode.Attributes["href"].Value.Length - 17));

                                        this.pmUpdates.Add(new PMObject(lTitel, lID, lTimeStamp));
                                    }
                                }
                            }

                            this.checkPMUpdate = false;
                        }
                    }
                    catch (Exception)
                    {
                        this.ErrHandler.add(lResponse);
                    }
                }
            }
        }
        /// <summary>
        /// (Vorläufig, nicht ausführlich getestet)
        /// Benutzt um ALLE Freundschaftsanfragen abzurufen und sie in die vorgesehen Eigenschaft einzutragen.
        /// Nur in initNotifications() und alle 30 Minuten, falls die FriendRequests-Eigenschaft abgerufen wird, benutzt.
        /// </summary>
        private void getAllFriendUpdates()
        {
            if (this.LoggedIn)
            {
                HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
                string lResponse = (HttpUtility.GetWebRequestResponse("https://proxer.me/user/my/connections?format=raw", this.LoginCookies)).Replace("</link>", "").Replace("\n", "");

                if (Utilities.Utility.checkForCorrectResponse(lResponse, this.ErrHandler))
                {
                    try
                    {
                        lDocument.LoadHtml(lResponse);

                        if (lDocument.ParseErrors.Count() == 0)
                        {
                            HtmlAgilityPack.HtmlNodeCollection lNodes = lDocument.DocumentNode.SelectNodes("//tr");

                            if (lNodes != null)
                            {
                                this.friendUpdates = new List<FriendRequestObject>();
                                foreach (HtmlAgilityPack.HtmlNode curNode in lNodes)
                                {
                                    if (curNode.Id.StartsWith("entry") && curNode.FirstChild.FirstChild.Attributes["class"].Value.Equals("accept"))
                                    {
                                        int lUserID = Convert.ToInt32(curNode.Id.Replace("entry", ""));
                                        string lUserName = curNode.InnerText.Split("  ".ToCharArray())[0];
                                        string lDescription = curNode.ChildNodes[3].ChildNodes[1].InnerText;
                                        string[] lDatumSplit = curNode.ChildNodes[4].InnerText.Split('-');
                                        DateTime lDatum = new DateTime(Convert.ToInt32(lDatumSplit[0]), Convert.ToInt32(lDatumSplit[1]), Convert.ToInt32(lDatumSplit[2]));
                                        bool lOnline = curNode.ChildNodes[1].ChildNodes[1].FirstChild.Attributes["src"].Value.Equals("/images/misc/onlineicon.png");

                                        this.friendUpdates.Add(new FriendRequestObject(lUserName, lUserID, lDescription, lDatum, lOnline, this));
                                    }
                                }
                                this.checkFriendUpdates = false;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        this.ErrHandler.add(lResponse);
                    }
                }
            }
        }
    }
}
