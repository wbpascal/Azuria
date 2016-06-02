using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Main;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Net;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria.Notifications
{
    /// <summary>
    ///     Represents a collection of <see cref="Anime" />- and <see cref="Manga" />-notifications.
    /// </summary>
    public class AnimeMangaNotificationCollection : INotificationCollection
    {
        private readonly Senpai _senpai;
        private AnimeMangaNotification[] _animeMangaNotifications;
        private INotification[] _notification;

        internal AnimeMangaNotificationCollection([NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationType.AnimeManga;
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
        ///     Gets a specified <paramref name="count" /> of notifications from the current ones.
        /// </summary>
        /// <param name="count">The notification count.</param>
        /// <returns>
        ///     An enumeration of notifications with a maximum length of <paramref name="count" />.
        /// </returns>
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<AnimeMangaNotification>>> GetAnimeMangaNotifications(int count)
        {
            if (this._animeMangaNotifications != null)
                return this._animeMangaNotifications.Length >= count
                    ? new ProxerResult<IEnumerable<AnimeMangaNotification>>(this._animeMangaNotifications)
                    : new ProxerResult<IEnumerable<AnimeMangaNotification>>(
                        this._animeMangaNotifications.Take(count).ToArray());

            ProxerResult lResult;
            if (!(lResult = await this.GetInfos()).Success)
                return new ProxerResult<IEnumerable<AnimeMangaNotification>>(lResult.Exceptions);

            return this._animeMangaNotifications.Length >= count
                ? new ProxerResult<IEnumerable<AnimeMangaNotification>>(this._animeMangaNotifications)
                : new ProxerResult<IEnumerable<AnimeMangaNotification>>(
                    this._animeMangaNotifications.Take(count).ToArray());
        }

        /// <summary>
        ///     Gets all notifications of the current <see cref="INotificationCollection.Type" />.
        /// </summary>
        /// <returns>An enumeration of notifications.</returns>
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<AnimeMangaNotification>>> GetAllAnimeMangaNotifications()
        {
            if (this._animeMangaNotifications != null)
                return new ProxerResult<IEnumerable<AnimeMangaNotification>>(this._animeMangaNotifications);

            ProxerResult lResult;
            return !(lResult = await this.GetInfos()).Success
                ? new ProxerResult<IEnumerable<AnimeMangaNotification>>(lResult.Exceptions)
                : new ProxerResult<IEnumerable<AnimeMangaNotification>>(this._animeMangaNotifications);
        }


        [ItemNotNull]
        private async Task<ProxerResult> GetInfos()
        {
            HtmlDocument lDocument = new HtmlDocument();
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/components/com_proxer/misc/notifications_misc.php"),
                        this._senpai.LoginCookies,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode[] lNodes =
                    lDocument.DocumentNode.SelectNodesUtility("class", "notificationList").ToArray();

                List<AnimeMangaNotification> lAnimeMangaUpdateObjects = new List<AnimeMangaNotification>();

                foreach (HtmlNode curNode in lNodes.Where(curNode => curNode.InnerText.StartsWith("Lesezeichen:")))
                {
                    string lName;
                    int lNumber;

                    int lId = Convert.ToInt32(curNode.Id.Substring(12));
                    string lMessage = curNode.ChildNodes["u"].InnerText;

                    if (lMessage.IndexOf('#') != -1)
                    {
                        lName = lMessage.Split('#')[0];
                        if (!int.TryParse(lMessage.Split('#')[1], out lNumber)) lNumber = -1;
                    }
                    else
                    {
                        lName = "";
                        lNumber = -1;
                    }

                    lAnimeMangaUpdateObjects.Add(new AnimeMangaNotification(lMessage, lName, lNumber, lId));
                }

                this._animeMangaNotifications = lAnimeMangaUpdateObjects.ToArray();
                this._notification = lAnimeMangaUpdateObjects.Cast<INotification>().ToArray();

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }

            #endregion
        }
    }
}