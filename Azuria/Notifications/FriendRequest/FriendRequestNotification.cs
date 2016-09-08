using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azuria.Api;
using Azuria.ErrorHandling;
using Azuria.UserInfo;

namespace Azuria.Notifications.FriendRequest
{
    /// <summary>
    ///     Represents a friend request notification.
    /// </summary>
    public class FriendRequestNotification : INotification
    {
        private readonly Senpai _senpai;
        private bool _handled;

        internal FriendRequestNotification(User user, DateTime requestTimeStamp, Senpai senpai)
        {
            this.TimeStamp = requestTimeStamp;
            this.User = user;
            this._senpai = senpai;
            this.NotificationId = user.Id.ToString() + requestTimeStamp.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        #region Properties

        /// <summary>
        ///     Gets the id of the notification.
        /// </summary>
        public string NotificationId { get; }

        /// <summary>
        ///     Gets the date of the friend request.
        /// </summary>
        public DateTime TimeStamp { get; }

        /// <summary>
        ///     Gets the type of the notification.
        /// </summary>
        public NotificationType Type => NotificationType.FriendRequest;

        /// <summary>
        ///     Gets the user that send the friend request.
        /// </summary>
        public User User { get; }

        #endregion

        #region Methods

        /// <summary>
        ///     Accepts the friend request.
        /// </summary>
        /// <returns>If the action was successful.</returns>
        public async Task<ProxerResult> AcceptRequest()
        {
            if (this._handled) return new ProxerResult {Success = false};

            Dictionary<string, string> lPostArgs = new Dictionary<string, string> {{"type", "accept"}};

            Func<string, ProxerResult> lCheckFunc =
                s => !s.StartsWith("{\"error\":0") ? new ProxerResult(new Exception[0]) : new ProxerResult();

            ProxerResult<string> lResult = await
                ApiInfo.HttpClient.PostRequest(
                    new Uri("https://proxer.me/user/my?format=json&cid=" + this.User.Id),
                    lPostArgs, new[] {lCheckFunc}, this._senpai);

            if (!lResult.Success) return new ProxerResult(lResult.Exceptions);

            this._handled = true;
            return new ProxerResult();
        }

        /// <summary>
        ///     Denies the friend request.
        /// </summary>
        /// <returns>If the action was successful.</returns>
        public async Task<ProxerResult> DenyRequest()
        {
            if (this._handled) return new ProxerResult {Success = false};

            Dictionary<string, string> lPostArgs = new Dictionary<string, string> {{"type", "deny"}};

            Func<string, ProxerResult> lCheckFunc =
                s => !s.StartsWith("{\"error\":0") ? new ProxerResult(new Exception[0]) : new ProxerResult();

            ProxerResult<string> lResult = await
                ApiInfo.HttpClient.PostRequest(
                    new Uri("https://proxer.me/user/my?format=json&cid=" + this.User.Id),
                    lPostArgs, new[] {lCheckFunc}, this._senpai);

            if (!lResult.Success) return new ProxerResult(lResult.Exceptions);

            this._handled = true;
            return new ProxerResult();
        }

        #endregion
    }
}