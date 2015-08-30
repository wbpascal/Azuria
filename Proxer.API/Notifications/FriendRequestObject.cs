using System;
using System.Collections.Generic;
using Proxer.API.Utilities;

namespace Proxer.API.Notifications
{
    /// <summary>
    /// </summary>
    public class FriendRequestObject : INotificationObject
    {
        private readonly Senpai _senpai;
        private bool _accepted;
        private bool _denied;

        /// <summary>
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <param name="senpai"></param>
        internal FriendRequestObject(string userName, int userId, Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationObjectType.FriendRequest;
            this.Message = userName;
            this.UserName = userName;
            this.Id = userId;
        }

        /// <summary>
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <param name="userDescription"></param>
        /// <param name="requestDate"></param>
        /// <param name="userOnline"></param>
        /// <param name="senpai"></param>
        internal FriendRequestObject(string userName, int userId, string userDescription, DateTime requestDate,
            bool userOnline, Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationObjectType.FriendRequest;
            this.Message = userName;
            this.UserName = userName;
            this.Id = userId;
            this.Description = userDescription;
            this.Date = requestDate;
            this.Online = userOnline;
        }

        /// <summary>
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// </summary>
        public bool Online { get; private set; }

        /// <summary>
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// </summary>
        public NotificationObjectType Type { get; private set; }

        /// <summary>
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// </summary>
        /// <returns>Ob die Aktion erfolgreich war</returns>
        public bool AcceptRequest()
        {
            if (this._senpai.LoggedIn && !this._accepted && !this._denied)
            {
                Dictionary<string, string> lPostArgs = new Dictionary<string, string> {{"type", "accept"}};
                string lResponse = HttpUtility.PostWebRequestResponse("https://proxer.me/user/my?format=json&cid=" + this.Id, this._senpai.LoginCookies, lPostArgs);

                if (lResponse.StartsWith("{\"error\":0"))
                {
                    this._accepted = true;
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// </summary>
        /// <returns>Ob die Aktion erfolgreich war</returns>
        public bool DenyRequest()
        {
            if (this._senpai.LoggedIn && !this._accepted && !this._denied)
            {
                Dictionary<string, string> lPostArgs = new Dictionary<string, string> {{"type", "deny"}};
                string lResponse = HttpUtility.PostWebRequestResponse("https://proxer.me/user/my?format=json&cid=" + this.Id, this._senpai.LoginCookies, lPostArgs);

                if (lResponse.StartsWith("{\"error\":0"))
                {
                    this._denied = true;
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// </summary>
        /// <returns>Ob die Aktion erfolgreich war</returns>
        public bool EditDescription(string pNewDescription)
        {
            if (this._senpai.LoggedIn)
            {
                Dictionary<string, string> lPostArgs = new Dictionary<string, string> {{"type", "desc"}};
                string lResponse =
                    HttpUtility.PostWebRequestResponse(
                        "https://proxer.me/user/my?format=json&desc=" +
                        System.Web.HttpUtility.JavaScriptStringEncode(pNewDescription) + "&cid=" + this.Id, this._senpai.LoginCookies, lPostArgs);

                if (lResponse.StartsWith("{\"error\":0"))
                {
                    this.Description = pNewDescription;
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}