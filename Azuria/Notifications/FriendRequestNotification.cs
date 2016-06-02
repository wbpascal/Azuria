using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Net;
using JetBrains.Annotations;

namespace Azuria.Notifications
{
    /// <summary>
    ///     Represents a friend request notification.
    /// </summary>
    public class FriendRequestNotification : INotification
    {
        private readonly Senpai _senpai;
        private bool _accepted;
        private bool _denied;

        internal FriendRequestNotification([NotNull] string userName, int userUserId, [NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationType.FriendRequest;
            this.Message = userName;
            this.UserName = userName;
            this.UserId = userUserId;
        }

        internal FriendRequestNotification([NotNull] string userName, int userUserId, DateTime requestDate,
            [NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationType.FriendRequest;
            this.Message = userName;
            this.UserName = userName;
            this.UserId = userUserId;
            this.Date = requestDate;
        }

        #region Geerbt

        /// <summary>
        ///     Gets the message of the notification.
        /// </summary>
        public string Message { get; }

        /// <summary>
        ///     Gets the type of the notification.
        /// </summary>
        public NotificationType Type { get; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the date of the friend request.
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        ///     Gets the id of the user who send the friend request.
        /// </summary>
        public int UserId { get; }

        /// <summary>
        ///     Gets the user name of the user who send the friend request.
        /// </summary>
        [NotNull]
        public string UserName { get; private set; }

        #endregion

        #region

        /// <summary>
        ///     Accepts the friend request.
        /// </summary>
        /// <returns>If the action was successful.</returns>
        [ItemNotNull]
        public async Task<ProxerResult> AcceptRequest()
        {
            if (!this._senpai.IsLoggedIn)
                return new ProxerResult(new Exception[] {new NotLoggedInException(this._senpai)});
            if (this._accepted || this._denied) return new ProxerResult {Success = false};

            Dictionary<string, string> lPostArgs = new Dictionary<string, string> {{"type", "accept"}};

            Func<string, ProxerResult> lCheckFunc =
                s => !s.StartsWith("{\"error\":0") ? new ProxerResult(new Exception[0]) : new ProxerResult();

            ProxerResult<string> lResult = await
                HttpUtility.PostResponseErrorHandling(
                    new Uri("https://proxer.me/user/my?format=json&cid=" + this.UserId),
                    lPostArgs, this._senpai.LoginCookies, this._senpai, new[] {lCheckFunc});

            if (!lResult.Success) return new ProxerResult(lResult.Exceptions);

            this._accepted = true;
            return new ProxerResult();
        }

        /// <summary>
        ///     Denies the friend request.
        /// </summary>
        /// <returns>If the action was successful.</returns>
        [ItemNotNull]
        public async Task<ProxerResult> DenyRequest()
        {
            if (!this._senpai.IsLoggedIn)
                return new ProxerResult(new Exception[] {new NotLoggedInException(this._senpai)});
            if (this._accepted || this._denied) return new ProxerResult {Success = false};

            Dictionary<string, string> lPostArgs = new Dictionary<string, string> {{"type", "deny"}};

            Func<string, ProxerResult> lCheckFunc =
                s => !s.StartsWith("{\"error\":0") ? new ProxerResult(new Exception[0]) : new ProxerResult();

            ProxerResult<string> lResult = await
                HttpUtility.PostResponseErrorHandling(
                    new Uri("https://proxer.me/user/my?format=json&cid=" + this.UserId),
                    lPostArgs, this._senpai.LoginCookies, this._senpai, new[] {lCheckFunc});

            if (!lResult.Success) return new ProxerResult(lResult.Exceptions);

            this._denied = true;
            return new ProxerResult();
        }

        #endregion
    }
}