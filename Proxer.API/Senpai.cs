using Newtonsoft.Json;
using Proxer.API.EventArgs;
using Proxer.API.Notifications;
using Proxer.API.Notifications.NotificationObjects;
using Proxer.API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
        private DispatcherTimer notificationCheckTimer;
        private DispatcherTimer loginCheckTimer;
        private DispatcherTimer notificationUpdateCheckTimer;

        //für die NotificationObject-Listen Eigenschaften
        internal bool checkNewsUpdate;
        internal bool checkAnimeMangaUpdate;
        internal bool checkPMUpdate;
        internal bool checkFriendUpdates;
        internal List<NewsObject> newsUpdates;
        internal List<AnimeMangaUpdateObject> animeMangaUpdates;
        internal List<PMObject> pmUpdates;
        internal List<FriendRequestObject> friendUpdates;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void NotificationEventHandler(object sender, NotificationEventArgs e);
        /// <summary>
        /// Wird aufgerufen, wenn eine neue Freundschaftsanfrage aussteht(30 Minuten Intervall)
        /// </summary>
        public event NotificationEventHandler FriendNotification_Raised;
        /// <summary>
        /// Wird aufgerufen, wenn neue ungelesene News vorhanden sind(30 Minuten Intervall)
        /// </summary>
        public event NotificationEventHandler NewsNotification_Raised;
        /// <summary>
        /// Wird aufgerufen, wenn ungelesene PMs vorhanden sind(30 Minuten Intervall)
        /// </summary>
        public event NotificationEventHandler PMNotification_Raised;
        /// <summary>
        /// Wird aufgerufen, wenn neue Anime Folgen oder Manga Kapitel vorhanden sind(30 Minuten Intervall)
        /// </summary>
        public event NotificationEventHandler UpdateNotification_Raised;
        /// <summary>
        /// Wird aufgerufen, wenn die Cookies verfallen sind(30 Minuten Intervall)
        /// </summary>
        public event EventHandler UserLoggedOut_Raised;


        /// <summary>
        /// Initialisiert die Klasse
        /// </summary>
        public Senpai()
        {
            this.loggedIn = false;
            this.LoginCookies = new CookieContainer();

            this.loginCheckTimer = new DispatcherTimer(DispatcherPriority.Background);
            this.loginCheckTimer.Interval = new TimeSpan(0, 45, 0);
            this.loginCheckTimer.Tick += (s, eArgs) =>
            {
                loginCheckTimer.Interval = new TimeSpan(0, 30, 0);
                checkLogin();
            };

            this.notificationCheckTimer = new DispatcherTimer(DispatcherPriority.Background);
            this.notificationCheckTimer.Interval = new TimeSpan(0, 30, 0);
            this.notificationCheckTimer.Tick += (s, eArgs) => { checkNotifications(); };

            this.notificationUpdateCheckTimer = new DispatcherTimer(DispatcherPriority.Background);
            this.notificationUpdateCheckTimer.Interval = new TimeSpan(0, 30, 0);
            this.notificationUpdateCheckTimer.Tick += (s, eArgs) =>
            {
                checkAnimeMangaUpdate = true;
                checkNewsUpdate = true;
                checkPMUpdate = true;
            };
        }


        /// <summary>
        /// Gibt an, ob der Benutzter noch eingeloggt ist, wird aber nicht überprüft
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
                    this.loggedIn = value;
                }
                else
                {
                    loginCheckTimer.Stop();
                    notificationCheckTimer.Stop();
                    notificationUpdateCheckTimer.Stop();
                    this.loggedIn = value;
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
                if (checkAnimeMangaUpdate || animeMangaUpdates == null) getAllAnimeMangaUpdates();
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
                if (checkNewsUpdate || newsUpdates == null) getAllNewsUpdates();
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
                if (checkPMUpdate || this.pmUpdates == null) getAllPMUpdates();
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
                if (checkPMUpdate || this.friendUpdates == null) getAllFriendUpdates();
                return friendUpdates;
            }
        }
        /// <summary>
        /// Gibt den CookieContainer zurück, der benutzt wird, um Aktionen im eingeloggten Status auszuführen
        /// </summary>
        public CookieContainer LoginCookies { get; private set; }


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
            string response = await HttpUtility.PostWebRequestResponseAsync("https://proxer.me/login?format=json&action=login", LoginCookies, postArgs);

            Dictionary<string, string> responseDes = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

            if (responseDes["error"].Equals("0"))
            {
                userID = Convert.ToInt32(responseDes["uid"]);
                this.username = username;
                LoggedIn = true;

                return true;
            }
            else
            {
                LoggedIn = false;

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
                string response = await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/login?format=json&action=login", LoginCookies);

                Dictionary<string, string> responseDes = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

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
        /// <summary>
        /// Checkt, ob neue Benachrichtigungen vorhanden sind
        /// </summary>
        private async void checkNotifications()
        {
            if (LoggedIn)
            {
                string[] response = (await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/notifications?format=raw&s=count", LoginCookies)).Split('#');

                if (response[0].Equals("0"))
                {
                    if (!response[2].Equals("0"))
                    {
                        if (PMNotification_Raised != null) PMNotification_Raised(this, new NotificationEventArgs(new PMNotification(Convert.ToInt32(response[2]))));
                        this.checkPMUpdate = true;
                    }
                    if (!response[3].Equals("0"))
                    {
                        if (FriendNotification_Raised != null) FriendNotification_Raised(this, new NotificationEventArgs(new FriendNotification(Convert.ToInt32(response[3]))));
                        //hier check bool einfügen
                    }
                    if (!response[4].Equals("0"))
                    {
                        if (NewsNotification_Raised != null) FriendNotification_Raised(this, new NotificationEventArgs(new NewsNotification(Convert.ToInt32(response[4]))));
                        this.checkNewsUpdate = true;
                    }
                    if (!response[5].Equals("0"))
                    {
                        if (UpdateNotification_Raised != null) UpdateNotification_Raised(this, new NotificationEventArgs(new AnimeMangaNotification(Convert.ToInt32(response[5]))));
                        this.checkAnimeMangaUpdate = true;
                    }
                }
            }
        }
        /// <summary>
        /// Initialisiert die Benachrichtigungen
        /// </summary>
        public async Task<bool> initNotifications()
        {
            if (LoggedIn)
            {
                string[] response = (await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/notifications?format=raw&s=count", LoginCookies)).Split('#');

                if (response[0].Equals("0"))
                {
                    if (!response[2].Equals("0"))
                    {
                        if (PMNotification_Raised != null) PMNotification_Raised(this, new NotificationEventArgs(new PMNotification(Convert.ToInt32(response[2]))));
                        this.checkPMUpdate = true;
                    }
                    if (!response[3].Equals("0"))
                    {
                        if (FriendNotification_Raised != null) FriendNotification_Raised(this, new NotificationEventArgs(new FriendNotification(Convert.ToInt32(response[3]))));
                        //hier check bool einfügen
                    }
                    if (!response[4].Equals("0"))
                    {
                        if (NewsNotification_Raised != null) FriendNotification_Raised(this, new NotificationEventArgs(new NewsNotification(Convert.ToInt32(response[4]))));
                        this.checkNewsUpdate = true;
                    }
                    if (!response[5].Equals("0"))
                    {
                        if (UpdateNotification_Raised != null) UpdateNotification_Raised(this, new NotificationEventArgs(new AnimeMangaNotification(Convert.ToInt32(response[5]))));
                        this.checkAnimeMangaUpdate = true;
                    }
                }

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
        private async void getAllAnimeMangaUpdates()
        {
            if (LoggedIn)
            {
                this.animeMangaUpdates = new List<AnimeMangaUpdateObject>();

                HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
                string lResponse = await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/components/com_proxer/misc/notifications_misc.php", LoginCookies);

                lDocument.LoadHtml(lResponse);

                if (lDocument.ParseErrors.Count() == 0)
                {
                    HtmlAgilityPack.HtmlNodeCollection lNodes = lDocument.DocumentNode.SelectNodes("//a[@class='notificationList']");

                    if (lNodes != null)
                    {
                        foreach (HtmlAgilityPack.HtmlNode curNode in lNodes)
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
                        this.checkAnimeMangaUpdate = false;
                    }
                }
            }
        }
        /// <summary>
        /// Benutzt um die letzten 15 News abzurufen und sie in die vorgesehene Eigenschaft einzutragen.
        /// Nur in initNotifications() und alle 30 Minuten, falls die News-Eigenschaft abgerufen wird, benutzt.
        /// </summary>
        private async void getAllNewsUpdates()
        {
            if (LoggedIn)
            {
                this.newsUpdates = new List<NewsObject>();

                string lResponse = await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/notifications?format=json&s=news&p=1", LoginCookies);
                if (lResponse.StartsWith("{\"error\":0"))
                {
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
        private async void getAllPMUpdates()
        {
            this.pmUpdates = new List<PMObject>();

            if (this.LoggedIn)
            {
                HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
                string lResponse = (await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/messages?format=raw&s=notification", this.LoginCookies)).Replace("</link>", "");

                lDocument.LoadHtml(lResponse);

                if (lDocument.ParseErrors.Count() == 0)
                {
                    HtmlAgilityPack.HtmlNodeCollection lNodes = lDocument.DocumentNode.SelectNodes("//a[@class='conferenceList']");

                    if (lNodes != null)
                    {
                        foreach (HtmlAgilityPack.HtmlNode curNode in lNodes)
                        {
                            string lTitel;
                            string[] lDatum;
                            if (curNode.ChildNodes[1].Name.ToLower().Equals("img"))
                            {
                                lTitel = curNode.ChildNodes[1].InnerText;
                                lDatum = curNode.ChildNodes[2].InnerText.Split('.');

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
        /// <summary>
        /// (Vorläufig, nicht ausfürhlich getestet)
        /// Benutzt um ALLE Freundschaftsanfragen abzurufen und sie in die vorgesehen Eigenschaft einzutragen.
        /// Nur in initNotifications() und alle 30 Minuten, falls die FriendRequests-Eigenschaft abgerufen wird, benutzt.
        /// </summary>
        public async void getAllFriendUpdates()
        {
            if (this.LoggedIn)
            {
                this.friendUpdates = new List<FriendRequestObject>();
                HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
                string lResponse = (await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/user/my/connections?format=raw", this.LoginCookies)).Replace("</link>", "").Replace("\n", "");

                lDocument.LoadHtml(lResponse);

                if (lDocument.ParseErrors.Count() == 0)
                {
                    HtmlAgilityPack.HtmlNodeCollection lNodes = lDocument.DocumentNode.SelectNodes("//tr");

                    if (lNodes != null)
                    {
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

                                this.friendUpdates.Add(new FriendRequestObject(lUserName, lUserID, lDescription, lDatum, lOnline));
                            }
                        }
                    }

                    this.checkFriendUpdates = false;
                }
            }
        }
    }
}
