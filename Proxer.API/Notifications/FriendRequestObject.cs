using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proxer.API.Exceptions;
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

        #region Properties

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
        public string Message { get; private set; }

        /// <summary>
        /// </summary>
        public bool Online { get; private set; }

        /// <summary>
        /// </summary>
        public NotificationObjectType Type { get; private set; }

        /// <summary>
        /// </summary>
        public string UserName { get; private set; }

        #endregion

        #region

        /// <summary>
        /// </summary>
        /// <exception cref="NotLoggedInException"></exception>
        /// <returns>Ob die Aktion erfolgreich war</returns>
        public async Task<bool> AcceptRequest()
        {
            if(!this._senpai.LoggedIn) throw new NotLoggedInException();
            if (this._accepted || this._denied) return false;
            Dictionary<string, string> lPostArgs = new Dictionary<string, string> {{"type", "accept"}};
            string lResponse =
                await HttpUtility.PostWebRequestResponse("https://proxer.me/user/my?format=json&cid=" + this.Id,
                    this._senpai.LoginCookies, lPostArgs);

            if (!lResponse.StartsWith("{\"error\":0")) return false;
            this._accepted = true;
            return true;
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotLoggedInException"></exception>
        /// <returns>Ob die Aktion erfolgreich war</returns>
        public async Task<bool> DenyRequest()
        {
            if(!this._senpai.LoggedIn) throw new NotLoggedInException();
            if (this._accepted || this._denied) return false;
            Dictionary<string, string> lPostArgs = new Dictionary<string, string> {{"type", "deny"}};
            string lResponse =
                await HttpUtility.PostWebRequestResponse("https://proxer.me/user/my?format=json&cid=" + this.Id,
                    this._senpai.LoginCookies, lPostArgs);

            if (!lResponse.StartsWith("{\"error\":0")) return false;
            this._denied = true;
            return true;
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotLoggedInException"></exception>
        /// <returns>Ob die Aktion erfolgreich war</returns>
        public async Task<bool> EditDescription(string pNewDescription)
        {
            if (!this._senpai.LoggedIn) throw new NotLoggedInException();
            Dictionary<string, string> lPostArgs = new Dictionary<string, string> {{"type", "desc"}};
            string lResponse =
                await HttpUtility.PostWebRequestResponse(
                    "https://proxer.me/user/my?format=json&desc=" +
                    System.Web.HttpUtility.JavaScriptStringEncode(pNewDescription) + "&cid=" + this.Id,
                    this._senpai.LoginCookies, lPostArgs);

            if (!lResponse.StartsWith("{\"error\":0")) return false;
            this.Description = pNewDescription;
            return true;
        }

        #endregion
    }
}