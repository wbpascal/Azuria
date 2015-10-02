using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proxer.API.Exceptions;
using Proxer.API.Utilities;

namespace Proxer.API.Notifications
{
    /// <summary>
    /// Eine Klasse, die eine Freundschaftsanfrage aus den Benachrichtigungen darstellt.
    /// </summary>
    public class FriendRequestObject : INotificationObject
    {
        private readonly Senpai _senpai;
        private bool _accepted;
        private bool _denied;

        internal FriendRequestObject(string userName, int userUserId, Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationObjectType.FriendRequest;
            this.Message = userName;
            this.UserName = userName;
            this.UserId = userUserId;
        }

        internal FriendRequestObject(string userName, int userUserId, DateTime requestDate, Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationObjectType.FriendRequest;
            this.Message = userName;
            this.UserName = userName;
            this.UserId = userUserId;
            this.Date = requestDate;
        }

        #region Geerbt

        /// <summary>
        /// Gibt die Nachricht der Benachrichtigung als Text zurück.
        /// <para>(Vererbt von <see cref="INotificationObject"/>)</para>
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gibt den Typ der Benachrichtigung zurück.
        /// <para>(Vererbt von <see cref="INotificationObject"/>)</para>
        /// </summary>
        public NotificationObjectType Type { get; private set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gibt das Datum der Freundschaftsanfrage zurück.
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Gibt die ID des <see cref="User">Benutzers</see> zurück, der die Freundschaftsanfrage gestellt hat.
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        /// Gibt den Namen des <see cref="User">Benutzers</see> zurück, der die Freundschaftsanfrage gestellt hat.
        /// </summary>
        public string UserName { get; private set; }

        #endregion

        #region

        /// <summary>
        /// Akzeptiert die Freundschaftsanfrage.
        /// </summary>
        /// <exception cref="NotLoggedInException">Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</exception>
        /// <seealso cref="Senpai.Login"/>
        /// <returns>Die Aktion war erfolgreich. True oder False</returns>
        public async Task<bool> AcceptRequest()
        {
            if (!this._senpai.LoggedIn) throw new NotLoggedInException();
            if (this._accepted || this._denied) return false;
            Dictionary<string, string> lPostArgs = new Dictionary<string, string> {{"type", "accept"}};
            string lResponse =
                await HttpUtility.PostWebRequestResponse("https://proxer.me/user/my?format=json&cid=" + this.UserId,
                    this._senpai.LoginCookies, lPostArgs);

            if (!lResponse.StartsWith("{\"error\":0")) return false;
            this._accepted = true;
            return true;
        }

        /// <summary>
        /// Lehnt die Freundschaftsanfrage ab.
        /// </summary>
        /// <exception cref="NotLoggedInException">Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</exception>
        /// <seealso cref="Senpai.Login"/>
        /// <returns>Die Aktion war erfolgreich. True oder False</returns>
        public async Task<bool> DenyRequest()
        {
            if (!this._senpai.LoggedIn) throw new NotLoggedInException();
            if (this._accepted || this._denied) return false;
            Dictionary<string, string> lPostArgs = new Dictionary<string, string> {{"type", "deny"}};
            string lResponse =
                await HttpUtility.PostWebRequestResponse("https://proxer.me/user/my?format=json&cid=" + this.UserId,
                    this._senpai.LoginCookies, lPostArgs);

            if (!lResponse.StartsWith("{\"error\":0")) return false;
            this._denied = true;
            return true;
        }

        #endregion
    }
}