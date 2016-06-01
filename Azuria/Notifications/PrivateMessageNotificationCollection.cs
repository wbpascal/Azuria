using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Net;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria.Notifications
{
    /// <summary>
    ///     Represents a collection of private message notifications.
    /// </summary>
    public class PrivateMessageNotificationCollection : INotificationCollection
    {
        private readonly Senpai _senpai;
        private INotification[] _notification;
        private PrivateMessageNotification[] _privateMessageNotifications;

        internal PrivateMessageNotificationCollection([NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationType.PrivateMessage;
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
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<PrivateMessageNotification>>> GetAllPrivateMessageNotifications()
        {
            if (this._notification != null)
                return new ProxerResult<IEnumerable<PrivateMessageNotification>>(this._privateMessageNotifications);

            ProxerResult lResult;
            return !(lResult = await this.GetInfos()).Success
                ? new ProxerResult<IEnumerable<PrivateMessageNotification>>(lResult.Exceptions)
                : new ProxerResult<IEnumerable<PrivateMessageNotification>>(this._privateMessageNotifications);
        }


        [ItemNotNull]
        private async Task<ProxerResult> GetInfos()
        {
            HtmlDocument lDocument = new HtmlDocument();
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/messages?format=raw&s=notification"),
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode[] lNodes =
                    lDocument.DocumentNode.SelectNodesUtility("class", "conferenceList").ToArray();

                List<PrivateMessageNotification> lPmObjects = new List<PrivateMessageNotification>();
                lPmObjects.AddRange(from curNode in lNodes
                    let lTitel =
                        curNode.ChildNodes[curNode.FirstChild.Name.Equals("img") ? 1 : 0].InnerText
                    let lDatum =
                        curNode.ChildNodes[curNode.FirstChild.Name.Equals("img") ? 2 : 1].InnerText
                            .Split('.')
                    let lTimeStamp =
                        new DateTime(Convert.ToInt32(lDatum[2]), Convert.ToInt32(lDatum[1]),
                            Convert.ToInt32(lDatum[0]))
                    let lId =
                        Convert.ToInt32(curNode.Attributes["href"].Value.Substring(13,
                            curNode.Attributes["href"].Value.Length - 17))
                    select new PrivateMessageNotification(lTitel, lId, lTimeStamp));

                this._privateMessageNotifications = lPmObjects.ToArray();
                this._notification = lPmObjects.Cast<INotification>().ToArray();

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        /// <summary>
        ///     Gets a specified <paramref name="count" /> of notifications from the current ones.
        /// </summary>
        /// <param name="count">The notification count.</param>
        /// <returns>
        ///     An enumeration of notifications with a maximum length of <paramref name="count" />.
        /// </returns>
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<PrivateMessageNotification>>> GetPrivateMessageNotifications(
            int count)
        {
            if (this._notification != null)
                return this._notification.Length >= count
                    ? new ProxerResult<IEnumerable<PrivateMessageNotification>>(this._privateMessageNotifications)
                    : new ProxerResult<IEnumerable<PrivateMessageNotification>>(
                        this._privateMessageNotifications.Take(count).ToArray());
            ProxerResult lResult;
            if (!(lResult = await this.GetInfos()).Success)
                return new ProxerResult<IEnumerable<PrivateMessageNotification>>(lResult.Exceptions);

            return this._notification.Length >= count
                ? new ProxerResult<IEnumerable<PrivateMessageNotification>>(this._privateMessageNotifications)
                : new ProxerResult<IEnumerable<PrivateMessageNotification>>(
                    this._privateMessageNotifications.Take(count).ToArray());
        }

        #endregion
    }
}