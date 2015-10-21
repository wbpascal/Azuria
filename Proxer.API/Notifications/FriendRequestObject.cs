using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Proxer.API.Exceptions;
using Proxer.API.Utilities;
using Proxer.API.Utilities.Net;
using RestSharp;

namespace Proxer.API.Notifications
{
    /// <summary>
    ///     Eine Klasse, die eine Freundschaftsanfrage aus den Benachrichtigungen darstellt.
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
        ///     Gibt die Nachricht der Benachrichtigung als Text zurück.
        ///     <para>(Vererbt von <see cref="INotificationObject" />)</para>
        /// </summary>
        public string Message { get; }

        /// <summary>
        ///     Gibt den Typ der Benachrichtigung zurück.
        ///     <para>(Vererbt von <see cref="INotificationObject" />)</para>
        /// </summary>
        public NotificationObjectType Type { get; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gibt das Datum der Freundschaftsanfrage zurück.
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        ///     Gibt die ID des <see cref="User">Benutzers</see> zurück, der die Freundschaftsanfrage gestellt hat.
        /// </summary>
        public int UserId { get; }

        /// <summary>
        ///     Gibt den Namen des <see cref="User">Benutzers</see> zurück, der die Freundschaftsanfrage gestellt hat.
        /// </summary>
        public string UserName { get; private set; }

        #endregion

        #region

        /// <summary>
        ///     Akzeptiert die Freundschaftsanfrage.
        /// </summary>
        /// <seealso cref="Senpai.Login" />
        /// <returns>Die Aktion war erfolgreich. True oder False</returns>
        public async Task<ProxerResult<bool>> AcceptRequest()
        {
            if (!this._senpai.LoggedIn)
                return new ProxerResult<bool>(new Exception[] {new NotLoggedInException(this._senpai)});
            if (this._accepted || this._denied) return new ProxerResult<bool>(false);

            Dictionary<string, string> lPostArgs = new Dictionary<string, string> {{"type", "accept"}};

            string lResponse;

            IRestResponse lResponseObject =
                await
                    HttpUtility.PostWebRequestResponse(
                        "https://proxer.me/user/my?format=json&cid=" + this.UserId,
                        this._senpai.LoginCookies, lPostArgs);
            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseObject.Content))
                lResponse = System.Web.HttpUtility.HtmlDecode(lResponseObject.Content).Replace("\n", "");
            else return new ProxerResult<bool>(new[] {new WrongResponseException(), lResponseObject.ErrorException});

            if (string.IsNullOrEmpty(lResponse) ||
                !Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler))
                return new ProxerResult<bool>(new Exception[] {new WrongResponseException()});

            if (!lResponse.StartsWith("{\"error\":0")) return new ProxerResult<bool>(false);

            this._accepted = true;
            return new ProxerResult<bool>(true);
        }

        /// <summary>
        ///     Lehnt die Freundschaftsanfrage ab.
        /// </summary>
        /// <seealso cref="Senpai.Login" />
        /// <returns>Die Aktion war erfolgreich. True oder False</returns>
        public async Task<ProxerResult<bool>> DenyRequest()
        {
            if (!this._senpai.LoggedIn)
                return new ProxerResult<bool>(new Exception[] {new NotLoggedInException(this._senpai)});
            if (this._accepted || this._denied) return new ProxerResult<bool>(false);

            Dictionary<string, string> lPostArgs = new Dictionary<string, string> {{"type", "deny"}};

            string lResponse;

            IRestResponse lResponseObject =
                await
                    HttpUtility.PostWebRequestResponse(
                        "https://proxer.me/user/my?format=json&cid=" + this.UserId,
                        this._senpai.LoginCookies, lPostArgs);
            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseObject.Content))
                lResponse = System.Web.HttpUtility.HtmlDecode(lResponseObject.Content).Replace("\n", "");
            else return new ProxerResult<bool>(new[] {new WrongResponseException(), lResponseObject.ErrorException});

            if (string.IsNullOrEmpty(lResponse) ||
                !Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler))
                return new ProxerResult<bool>(new Exception[] {new WrongResponseException()});

            if (!lResponse.StartsWith("{\"error\":0")) return new ProxerResult<bool>(false);

            this._denied = true;
            return new ProxerResult<bool>(true);
        }

        #endregion
    }
}