using Newtonsoft.Json;
using Proxer.API.EventArgs;
using Proxer.API.Notifications;
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
    public class User
    {
        private int userID;
        private bool loggedIn;
        private string username;
        private CookieContainer loginCookies;
        private DispatcherTimer notificationTimer;
        private DispatcherTimer loginCheckTimer;

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

        public User()
        {
            this.loggedIn = false;
            this.loginCookies = new CookieContainer();

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
                    notificationTimer.Start();
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
            string response = await HttpUtility.PostWebRequestResponseAsync("https://proxer.me/login?format=json&action=login", loginCookies, postArgs);

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
        private async void checkLogin()
        {
            if (LoggedIn)
            {
                string response = await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/login?format=json&action=login", loginCookies);

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
                string[] response = (await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/notifications?format=raw&s=count", loginCookies)).Split('#');

                if (response[0].Equals("0"))
                {
                    if (!response[2].Equals("0"))
                    {
                        if (PMNotification_Raised != null) PMNotification_Raised(this, new NotificationEventArgs(new PMNotification()));
                    }
                    if (!response[3].Equals("0"))
                    {
                        if (FriendNotification_Raised != null) FriendNotification_Raised(this, new NotificationEventArgs(new FriendNotification()));
                    }
                    if (!response[4].Equals("0"))
                    {
                        if (NewsNotification_Raised != null) FriendNotification_Raised(this, new NotificationEventArgs(new FriendNotification()));
                    }
                    if (!response[5].Equals("0"))
                    {
                        if (UpdateNotification_Raised != null) UpdateNotification_Raised(this, new NotificationEventArgs(new UpdateNotification()));
                    }
                }
            }
        }
    }
}
