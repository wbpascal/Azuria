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
    public class User
    {
        private int userID;
        private bool loggedIn;
        private string username;
        private DispatcherTimer notificationTimer;
        private DispatcherTimer loginCheckTimer;

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
        public User()
        {
            this.AMUpdates = new List<UpdateObject>();
            this.loggedIn = false;
            this.LoginCookies = new CookieContainer();

            this.loginCheckTimer = new DispatcherTimer(DispatcherPriority.Background);
            this.loginCheckTimer.Interval = new TimeSpan(0, 45, 0);
            this.loginCheckTimer.Tick += (s, eArgs) =>
            {
                loginCheckTimer.Interval = new TimeSpan(0, 30, 0);
                checkLogin();
            };

            this.notificationTimer = new DispatcherTimer(DispatcherPriority.Background);
            this.notificationTimer.Interval = new TimeSpan(0, 30, 0);
            this.notificationTimer.Tick += (s, eArgs) => { checkNotifications(); };
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
                    notificationTimer.Stop();
                    this.loggedIn = value;
                }
            }
        }
        /// <summary>
        /// Gibt alle Anime und Manga Benachrichtigungen in einer Liste zurück
        /// </summary>
        public List<UpdateObject> AMUpdates { get; private set; }
        /// <summary>
        /// 
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
                        if (PMNotification_Raised != null) PMNotification_Raised(this, new NotificationEventArgs(new PMNotification(Convert.ToInt32(response[2]))));
                    }
                    if (!response[3].Equals("0"))
                    {
                        if (FriendNotification_Raised != null) FriendNotification_Raised(this, new NotificationEventArgs(new FriendNotification(Convert.ToInt32(response[3]))));
                    }
                    if (!response[4].Equals("0"))
                    {
                        if (NewsNotification_Raised != null) FriendNotification_Raised(this, new NotificationEventArgs(new NewsNotification(Convert.ToInt32(response[4]))));
                    }
                    if (!response[5].Equals("0"))
                    {
                        if (UpdateNotification_Raised != null) UpdateNotification_Raised(this, new NotificationEventArgs(new UpdateNotification(Convert.ToInt32(response[5]), this)));
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
                    }
                    if (!response[3].Equals("0"))
                    {
                        if (FriendNotification_Raised != null) FriendNotification_Raised(this, new NotificationEventArgs(new FriendNotification(Convert.ToInt32(response[3]))));
                    }
                    if (!response[4].Equals("0"))
                    {
                        if (NewsNotification_Raised != null) FriendNotification_Raised(this, new NotificationEventArgs(new NewsNotification(Convert.ToInt32(response[4]))));
                    }
                    if (!response[5].Equals("0"))
                    {
                        if (UpdateNotification_Raised != null) UpdateNotification_Raised(this, new NotificationEventArgs(new UpdateNotification(Convert.ToInt32(response[5]), this)));
                    }
                }
                await getAllUpdateNotifications();

                notificationTimer.Start();

                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Benutzt um ALLE Update Benachrichtigungen in die AMUpdates Eigenschaft einzutragen.
        /// Nur einmal in initNotifications() benutzt.
        /// </summary>
        private async Task<bool> getAllUpdateNotifications()
        {
            if (LoggedIn)
            {
                try
                {
                    List<string> responseSplit = Utility.Utility.GetTagContents(await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/components/com_proxer/misc/notifications_misc.php", LoginCookies), "<a class=\"notificationList\"", "</a>");

                    foreach (string curUpdate in responseSplit)
                    {
                        string lName;
                        int lNumber;

                        int lID = Convert.ToInt32(Utility.Utility.GetTagContents(curUpdate, "id=\"", "\"")[0].Substring(12));
                        string lMessage = Utility.Utility.GetTagContents(curUpdate, "<u>", "</u>")[0];
                        Uri lLink = new Uri(Utility.Utility.GetTagContents(curUpdate, "href=\"", "\"")[0]);

                        if (lMessage.IndexOf('#') != -1)
                        {
                            lName = curUpdate.Split('#')[0];
                            if (!Int32.TryParse(curUpdate.Split('#')[1], out lNumber)) lNumber = -1;
                        }
                        else
                        {
                            lName = "";
                            lNumber = -1;
                        }

                        this.AMUpdates.Add(new UpdateObject(lMessage, lName, lNumber, lLink, lID));
                    }

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
