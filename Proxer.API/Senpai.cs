using Newtonsoft.Json;
using Nito.AsyncEx;
using Proxer.API.EventArguments;
using Proxer.API.Notifications;
using Proxer.API.Notifications.NotificationObjects;
using Proxer.API.Utility;
using System;
using System.Collections.Generic;
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
        public delegate void NotificationEventHandler(object sender, NotificationEventArgs e);
        /// <summary>
        /// Wird bei allen Benachrichtigungen aufgerufen(30 Minuten Intervall)
        /// </summary>
        public event NotificationEventHandler Notification_Raised;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void FriendNotificiationEventHandler(object sender, FriendNotificationEventArgs e);
        /// <summary>
        /// Wird aufgerufen, wenn eine neue Freundschaftsanfrage aussteht(30 Minuten Intervall)
        /// </summary>
        public event FriendNotificiationEventHandler FriendNotification_Raised;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void NewsNotificationEventHandler(object sender, NewsNotificationEventArgs e);
        /// <summary>
        /// Wird aufgerufen, wenn neue ungelesene News vorhanden sind(30 Minuten Intervall)
        /// </summary>
        public event NewsNotificationEventHandler NewsNotification_Raised;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void PMNotificationEventHandler(object sender, PMNotificationEventArgs e);
        /// <summary>
        /// Wird aufgerufen, wenn ungelesene PMs vorhanden sind(30 Minuten Intervall)
        /// </summary>
        public event PMNotificationEventHandler PMNotification_Raised;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void AMNotificationEventHandler(object sender, AMNotificationEventArgs e);
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
            this.animeMangaUpdates = new List<AnimeMangaUpdateObject>() { new AnimeMangaUpdateObject(new Object()) };
            this.friendUpdates = new List<FriendRequestObject>() { new FriendRequestObject(new Object()) };
            this.newsUpdates = new List<NewsObject>() { new NewsObject(new Object()) };
            this.pmUpdates = new List<PMObject>() { new PMObject(new Object()) };

            this.loggedIn = false;
            this.LoginCookies = new CookieContainer();

            this.loginCheckTimer = new Timer();
            this.loginCheckTimer.AutoReset = true;
            this.loginCheckTimer.Interval = (new TimeSpan(0, 45, 0)).TotalMilliseconds;
            this.loginCheckTimer.Elapsed += (s, eArgs) =>
            {
                loginCheckTimer.Interval = (new TimeSpan(0, 30, 0)).TotalMilliseconds;
                checkLogin();
            };

            this.notificationCheckTimer = new Timer();
            this.notificationCheckTimer.AutoReset = true;
            this.notificationCheckTimer.Interval = (new TimeSpan(0, 30, 0)).TotalMilliseconds;
            this.notificationCheckTimer.Elapsed += (s, eArgs) => { checkNotifications(); };

            this.notificationUpdateCheckTimer = new Timer();
            this.notificationUpdateCheckTimer.AutoReset = true;
            this.notificationUpdateCheckTimer.Interval = (new TimeSpan(0, 30, 0)).TotalMilliseconds;
            this.notificationUpdateCheckTimer.Elapsed += (s, eArgs) =>
            {
                checkAnimeMangaUpdate = true;
                checkNewsUpdate = true;
                checkPMUpdate = true;
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
                if (checkAnimeMangaUpdate || (this.animeMangaUpdates.Count == 1 && this.animeMangaUpdates[0].Typ == NotificationObjectType.Dummy)) AsyncContext.Run(() => getAllAnimeMangaUpdates());
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
                if (checkNewsUpdate || (this.newsUpdates.Count == 1 && this.newsUpdates[0].Typ == NotificationObjectType.Dummy)) AsyncContext.Run(() => getAllNewsUpdates());
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
                if (checkPMUpdate || (this.pmUpdates.Count == 1 && this.pmUpdates[0].Typ == NotificationObjectType.Dummy)) AsyncContext.Run(() => getAllPMUpdates());
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
                if (checkFriendUpdates || (this.friendUpdates.Count == 1 && this.friendUpdates[0].Typ == NotificationObjectType.Dummy)) AsyncContext.Run(() => getAllFriendUpdates());
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
        /// Loggt den Benutzer ein und speichert die Cookies in ein Objekt, sodass
        /// mit diesem Objekt weitergearbeitet werden kann
        /// </summary>
        /// <param name="username">Der Benutzername des zu einloggenden Benutzers</param>
        /// <param name="password">Das Passwort des Benutzers</param>
        /// <returns>Gibt zurück, ob der Benutzer erfolgreich eingeloggt wurde</returns>
        public async Task<bool> login(string username, string password)
        {
            if (LoggedIn) return false;

            Dictionary<string, string> postArgs = new Dictionary<string, string>
            {
                {"username", username},
                {"password", password}
            };
            string lResponse = await HttpUtility.PostWebRequestResponseAsync("https://proxer.me/login?format=json&action=login", LoginCookies, postArgs);

            if (Utility.Utility.checkForCorrectJson(lResponse))
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
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Checkt per API, ob der Benutzer noch eingeloggt ist
        /// </summary>
        public async void checkLogin()
        {
            if (LoggedIn)
            {
                string lResponse = await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/login?format=json&action=login", LoginCookies);

                if (Utility.Utility.checkForCorrectJson(lResponse))
                {
                    Dictionary<string, string> responseDes = JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                    if (responseDes["error"].Equals("0"))
                    {
                        LoggedIn = true;
                    }
                    else
                    {
                        LoggedIn = false;
                        if (UserLoggedOut_Raised != null) UserLoggedOut_Raised(this, new System.EventArgs());
                    }
                }
            }
        }
        /// <summary>
        /// Checkt, ob neue Benachrichtigungen vorhanden sind
        /// </summary>
        private async void checkNotifications()
        {
            if (LoggedIn)
            {
                string lResponse = await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/notifications?format=raw&s=count", LoginCookies);

                if (Utility.Utility.checkForCorrectHTML(lResponse) && lResponse.StartsWith("0"))
                {
                    string[] lResponseSplit = lResponse.Split('#');

                    if (!lResponseSplit[2].Equals("0"))
                    {
                        if (PMNotification_Raised != null) PMNotification_Raised(this, new PMNotificationEventArgs(new PMNotification(Convert.ToInt32(lResponseSplit[2]))));
                        if (Notification_Raised != null) Notification_Raised(this, new NotificationEventArgs(new PMNotification(Convert.ToInt32(lResponseSplit[2]))));
                        this.checkPMUpdate = true;
                    }
                    if (!lResponseSplit[3].Equals("0"))
                    {
                        if (FriendNotification_Raised != null) FriendNotification_Raised(this, new FriendNotificationEventArgs(new FriendNotification(Convert.ToInt32(lResponseSplit[3]))));
                        if (Notification_Raised != null) Notification_Raised(this, new NotificationEventArgs(new FriendNotification(Convert.ToInt32(lResponseSplit[3]))));
                        this.checkFriendUpdates = true;
                    }
                    if (!lResponseSplit[4].Equals("0"))
                    {
                        if (NewsNotification_Raised != null) NewsNotification_Raised(this, new NewsNotificationEventArgs(new NewsNotification(Convert.ToInt32(lResponseSplit[4]))));
                        if (Notification_Raised != null) Notification_Raised(this, new NotificationEventArgs(new NewsNotification(Convert.ToInt32(lResponseSplit[4]))));
                        this.checkNewsUpdate = true;
                    }
                    if (!lResponseSplit[5].Equals("0"))
                    {
                        if (AMUpdateNotification_Raised != null) AMUpdateNotification_Raised(this, new AMNotificationEventArgs(new AnimeMangaNotification(Convert.ToInt32(lResponseSplit[5]))));
                        if (Notification_Raised != null) Notification_Raised(this, new NotificationEventArgs(new AnimeMangaNotification(Convert.ToInt32(lResponseSplit[5]))));
                        this.checkAnimeMangaUpdate = true;
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
        /// (Vorläufig, nicht ausführlich getestet)
        /// Benutzt um ALLE Anime und Manga Benachrichtigungen in die vorgesehene einzutragen.
        /// Wird nur in initNotifications() und alle 30 Minuten, falls die AnimeMangaUpdates-Eigenschaft abgerufen wird, benutzt.
        /// </summary>
        private async Task getAllAnimeMangaUpdates()
        {
            if (LoggedIn)
            {
                HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
                string lResponse = await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/components/com_proxer/misc/notifications_misc.php", LoginCookies);

                if (Utility.Utility.checkForCorrectHTML(lResponse))
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
            }
        }
        /// <summary>
        /// Benutzt um die letzten 15 News abzurufen und sie in die vorgesehene Eigenschaft einzutragen.
        /// Nur in initNotifications() und alle 30 Minuten, falls die News-Eigenschaft abgerufen wird, benutzt.
        /// </summary>
        private async Task getAllNewsUpdates()
        {
            if (LoggedIn)
            {
                string lResponse = await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/notifications?format=json&s=news&p=1", LoginCookies);
                if (Utility.Utility.checkForCorrectJson(lResponse) && lResponse.StartsWith("{\"error\":0"))
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
        private async Task getAllPMUpdates()
        {
            if (this.LoggedIn)
            {
                HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
                string lResponse = (await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/messages?format=raw&s=notification", this.LoginCookies)).Replace("</link>", "").Replace("\n", "");

                if (Utility.Utility.checkForCorrectHTML(lResponse))
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
            }
        }
        /// <summary>
        /// (Vorläufig, nicht ausführlich getestet)
        /// Benutzt um ALLE Freundschaftsanfragen abzurufen und sie in die vorgesehen Eigenschaft einzutragen.
        /// Nur in initNotifications() und alle 30 Minuten, falls die FriendRequests-Eigenschaft abgerufen wird, benutzt.
        /// </summary>
        public async Task getAllFriendUpdates()
        {
            if (this.LoggedIn)
            {
                HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
                string lResponse = (await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/user/my/connections?format=raw", this.LoginCookies)).Replace("</link>", "").Replace("\n", "");

                if (Utility.Utility.checkForCorrectHTML(lResponse))
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
            }
        }
    }
}
