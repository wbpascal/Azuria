using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Net;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria.Notifications
{
    /// <summary>
    ///     Represents a collection of friend request notifications.
    /// </summary>
    public class FriendRequestNotificationCollection : INotificationCollection
    {
        private readonly Senpai _senpai;
        private FriendRequestNotification[] _friendRequestNotifications;
        private INotification[] _notification;


        internal FriendRequestNotificationCollection([NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationType.FriendRequest;
        }

        #region Geerbt

        /// <summary>
        ///     Gets the type of the notifications.
        /// </summary>
        public NotificationType Type { get; }

        /// <summary>
        ///     Gets all notifications of the current <see cref="INotificationCollection.Type" />.
        /// </summary>
        /// <returns>An enumeration of notifications.</returns>
        public async Task<ProxerResult<IEnumerable<INotification>>> GetAllNotifications()
        {
            if (this._notification != null)
                return new ProxerResult<IEnumerable<INotification>>(this._notification);

            ProxerResult lResult;
            return !(lResult = await this.GetInfos()).Success
                ? new ProxerResult<IEnumerable<INotification>>(lResult.Exceptions)
                : new ProxerResult<IEnumerable<INotification>>(this._notification);
        }

        /// <summary>
        ///     Gets a specified <paramref name="count" /> of notifications from the current ones.
        /// </summary>
        /// <param name="count">The notification count.</param>
        /// <returns>
        ///     An enumeration of notifications with a maximum length of <paramref name="count" />.
        /// </returns>
        public async Task<ProxerResult<IEnumerable<INotification>>> GetNotifications(int count)
        {
            if (this._notification != null)
                return this._notification.Length >= count
                    ? new ProxerResult<IEnumerable<INotification>>(this._notification)
                    : new ProxerResult<IEnumerable<INotification>>(this._notification.Take(count).ToArray());
            ProxerResult lResult;
            if (!(lResult = await this.GetInfos()).Success)
                return new ProxerResult<IEnumerable<INotification>>(lResult.Exceptions);

            return this._notification.Length >= count
                ? new ProxerResult<IEnumerable<INotification>>(this._notification)
                : new ProxerResult<IEnumerable<INotification>>(this._notification.Take(count).ToArray());
        }

        #endregion

        #region

        /// <summary>
        ///     Gets all notifications of the current <see cref="INotificationCollection.Type" />.
        /// </summary>
        /// <returns>An enumeration of notifications.</returns>
        /// [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<FriendRequestNotification>>> GetAllFriendRequestNotfications()
        {
            if (this._notification != null)
                return new ProxerResult<IEnumerable<FriendRequestNotification>>(this._friendRequestNotifications);

            ProxerResult lResult;
            return !(lResult = await this.GetInfos()).Success
                ? new ProxerResult<IEnumerable<FriendRequestNotification>>(lResult.Exceptions)
                : new ProxerResult<IEnumerable<FriendRequestNotification>>(this._friendRequestNotifications);
        }

        /// <summary>
        ///     Gets a specified <paramref name="count" /> of notifications from the current ones.
        /// </summary>
        /// <param name="count">The notification count.</param>
        /// <returns>
        ///     An enumeration of notifications with a maximum length of <paramref name="count" />.
        /// </returns>
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<FriendRequestNotification>>> GetFriendRequestNotifications(int count)
        {
            if (this._notification != null)
                return this._notification.Length >= count
                    ? new ProxerResult<IEnumerable<FriendRequestNotification>>(this._friendRequestNotifications)
                    : new ProxerResult<IEnumerable<FriendRequestNotification>>(
                        this._friendRequestNotifications.Take(count).ToArray());
            ProxerResult lResult;
            if (!(lResult = await this.GetInfos()).Success)
                return new ProxerResult<IEnumerable<FriendRequestNotification>>(lResult.Exceptions);

            return this._notification.Length >= count
                ? new ProxerResult<IEnumerable<FriendRequestNotification>>(this._friendRequestNotifications)
                : new ProxerResult<IEnumerable<FriendRequestNotification>>(
                    this._friendRequestNotifications.Take(count).ToArray());
        }


        [ItemNotNull]
        private async Task<ProxerResult> GetInfos()
        {
            HtmlDocument lDocument = new HtmlDocument();
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/user/my/connections?format=raw"),
                        this._senpai.LoginCookies,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                IEnumerable<HtmlNode> lNodes = lDocument.DocumentNode.DescendantsAndSelf().Where(x => x.Name == "tr");

                List<FriendRequestNotification> lFriendRequests = (from curNode in lNodes
                    where
                        curNode.Id.StartsWith("entry") &&
                        curNode.FirstChild.FirstChild.Attributes["class"].Value
                            .Equals
                            ("accept")
                    let lUserId =
                        Convert.ToInt32(curNode.Id.Replace("entry", ""))
                    let lUserName =
                        curNode.InnerText.Split("  ".ToCharArray())[0]
                    let lDatumSplit =
                        curNode.ChildNodes[4].InnerText.Split('-')
                    let lDatum =
                        new DateTime(Convert.ToInt32(lDatumSplit[0]),
                            Convert.ToInt32(lDatumSplit[1]),
                            Convert.ToInt32(lDatumSplit[2]))
                    select
                        new FriendRequestNotification(lUserName, lUserId, lDatum,
                            this._senpai))
                    .ToList();

                this._friendRequestNotifications = lFriendRequests.ToArray();
                this._notification = lFriendRequests.Cast<INotification>().ToArray();

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        #endregion
    }
}