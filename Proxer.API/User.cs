using Newtonsoft.Json;
using Proxer.API.EventArgs;
using Proxer.API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Proxer.API
{
    public class User
    {
        private int userID;
        private string username;
        private CookieContainer loginCookies;
        private bool loggedIn;

        public delegate void NotificationEventHandler(object sender, NotificationEventArgs e);
        public event NotificationEventHandler FriendNotification_Raised;
        public event NotificationEventHandler NewsNotification_Raised;
        public event NotificationEventHandler PMNotification_Raised;
        public event NotificationEventHandler UpdateNotification_Raised;

        public User() { }

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
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
