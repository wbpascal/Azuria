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
        private DispatcherTimer newsCheckTimer;
        private DispatcherTimer animeMangaCheckTimer;

        //für die NotificationObject-Listen Eigenschaften
        internal bool checkNews;
        internal bool checkAnimeMangaUpdate;
        internal List<NewsObject> newsObjects;
        internal List<AnimeMangaUpdateObject> animeMangaUpdates;

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
        /// 
        /// </summary>
        public Senpai()
        {
            //Initialisiere Eigenschaften
            //this.AnimeMangaUpdates = new List<AnimeMangaUpdateObject>();
            newsObjects = new List<NewsObject>();
            animeMangaUpdates = new List<AnimeMangaUpdateObject>();

            //Initialisiert die check-Variablen(wichtig für die Update-Eigenschaften)
            checkNews = false;
            checkAnimeMangaUpdate = false;

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

            this.newsCheckTimer = new DispatcherTimer(DispatcherPriority.Background);
            this.newsCheckTimer.Interval = new TimeSpan(0, 30, 0);
            this.newsCheckTimer.Tick += (s, eArgs) => { checkNews = true; };

            this.animeMangaCheckTimer = new DispatcherTimer(DispatcherPriority.Background);
            this.animeMangaCheckTimer.Interval = new TimeSpan(0, 30, 0);
            this.animeMangaCheckTimer.Tick += (s, eArgs) => { checkAnimeMangaUpdate = true; };
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
                    newsCheckTimer.Stop();
                    animeMangaCheckTimer.Stop();
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
                if (checkNews || newsObjects == null) getAllNewsUpdates();
                return newsObjects;
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
                        if (PMNotification_Raised != null) PMNotification_Raised(this, new NotificationEventArgs(new PMNotification(Convert.ToInt32(response[2]), this)));
                    }
                    if (!response[3].Equals("0"))
                    {
                        if (FriendNotification_Raised != null) FriendNotification_Raised(this, new NotificationEventArgs(new FriendNotification(Convert.ToInt32(response[3]))));
                    }
                    if (!response[4].Equals("0"))
                    {
                        if (NewsNotification_Raised != null) FriendNotification_Raised(this, new NotificationEventArgs(new NewsNotification(Convert.ToInt32(response[4]), this)));
                    }
                    if (!response[5].Equals("0"))
                    {
                        if (UpdateNotification_Raised != null) UpdateNotification_Raised(this, new NotificationEventArgs(new AnimeMangaNotification(Convert.ToInt32(response[5]), this)));
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
                        if (PMNotification_Raised != null) PMNotification_Raised(this, new NotificationEventArgs(new PMNotification(Convert.ToInt32(response[2]), this)));
                    }
                    if (!response[3].Equals("0"))
                    {
                        if (FriendNotification_Raised != null) FriendNotification_Raised(this, new NotificationEventArgs(new FriendNotification(Convert.ToInt32(response[3]))));
                    }
                    if (!response[4].Equals("0"))
                    {
                        if (NewsNotification_Raised != null) FriendNotification_Raised(this, new NotificationEventArgs(new NewsNotification(Convert.ToInt32(response[4]), this)));
                    }
                    if (!response[5].Equals("0"))
                    {
                        if (UpdateNotification_Raised != null) UpdateNotification_Raised(this, new NotificationEventArgs(new AnimeMangaNotification(Convert.ToInt32(response[5]), this)));
                    }
                }

                getAllAnimeMangaUpdates();
                getAllNewsUpdates();

                notificationCheckTimer.Start();
                newsCheckTimer.Start();
                animeMangaCheckTimer.Start();

                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// (Vorläufig, nicht ausführlich getestet)
        /// Benutzt um ALLE Update Benachrichtigungen in die AnimeMangaUpdates Eigenschaft einzutragen.
        /// Nur in initNotifications() und alle 30 Minuten, falls die AnimeMangaUpdates-Eigenschaft abgerufen wird, benutzt.
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

                    foreach (HtmlAgilityPack.HtmlNode curNode in lNodes)
                    {
                        string lName;
                        int lNumber;

                        int lID = Convert.ToInt32(curNode.Id.Substring(12));
                        string lMessage = curNode.ChildNodes["u"].InnerText;
                        Uri lLink = new Uri("http://proxer.me" + curNode.Attributes["href"].Value);

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
        /// <summary>
        /// Benutzt um die letzten 15 News abzurufen.
        /// Nur in initNotifications() und alle 30 Minuten, falls die AnimeMangaUpdates-Eigenschaft abgerufen wird, benutzt.
        /// </summary>
        private async void getAllNewsUpdates()
        {
            if (LoggedIn)
            {
                this.newsObjects = new List<NewsObject>();

                string lResponse = await HttpUtility.GetWebRequestResponseAsync("http://proxer.me/notifications?format=json&s=news&p=1", LoginCookies);
                if (lResponse.StartsWith("{\"error\":0"))
                {
                    Dictionary<string, List<NewsObject>> lDeserialized = JsonConvert.DeserializeObject<Dictionary<string, List<NewsObject>>>("{" + lResponse.Substring("{\"error\":0,".Length));
                    newsObjects = lDeserialized["notifications"];
                    this.checkNews = false;
                }
            }
        }
    }
}
