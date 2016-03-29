using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Net;

namespace Azuria.Notifications
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
        /// <exception cref="NotLoggedInException">Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</exception>
        /// <exception cref="WrongResponseException">Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</exception>
        /// <seealso cref="Senpai.Login" />
        /// <returns>Die Aktion war erfolgreich. True oder False</returns>
        public async Task<ProxerResult<bool>> AcceptRequest()
        {
            if (!this._senpai.IsLoggedIn)
                return new ProxerResult<bool>(new Exception[] {new NotLoggedInException(this._senpai)});
            if (this._accepted || this._denied) return new ProxerResult<bool>(false);

            Dictionary<string, string> lPostArgs = new Dictionary<string, string> {{"type", "accept"}};

            Func<string, ProxerResult> lCheckFunc =
                s => !s.StartsWith("{\"error\":0") ? new ProxerResult(new Exception[0]) : new ProxerResult();

            ProxerResult<string> lResult = await
                HttpUtility.PostResponseErrorHandling("https://proxer.me/user/my?format=json&cid=" + this.UserId,
                    lPostArgs, this._senpai.LoginCookies, this._senpai.ErrHandler, this._senpai, new[] {lCheckFunc});

            if (!lResult.Success) return new ProxerResult<bool>(lResult.Exceptions);

            this._accepted = true;
            return new ProxerResult<bool>(true);
        }

        /// <summary>
        ///     Lehnt die Freundschaftsanfrage ab.
        /// </summary>
        /// <exception cref="NotLoggedInException">Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</exception>
        /// <exception cref="WrongResponseException">Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</exception>
        /// <seealso cref="Senpai.Login" />
        /// <returns>Die Aktion war erfolgreich. True oder False</returns>
        public async Task<ProxerResult<bool>> DenyRequest()
        {
            if (!this._senpai.IsLoggedIn)
                return new ProxerResult<bool>(new Exception[] {new NotLoggedInException(this._senpai)});
            if (this._accepted || this._denied) return new ProxerResult<bool>(false);

            Dictionary<string, string> lPostArgs = new Dictionary<string, string> {{"type", "deny"}};

            Func<string, ProxerResult> lCheckFunc =
                s => !s.StartsWith("{\"error\":0") ? new ProxerResult(new Exception[0]) : new ProxerResult();

            ProxerResult<string> lResult = await
                HttpUtility.PostResponseErrorHandling("https://proxer.me/user/my?format=json&cid=" + this.UserId,
                    lPostArgs, this._senpai.LoginCookies, this._senpai.ErrHandler, this._senpai, new[] {lCheckFunc});

            if (!lResult.Success) return new ProxerResult<bool>(lResult.Exceptions);

            this._denied = true;
            return new ProxerResult<bool>(true);
        }

        #endregion
    }
}