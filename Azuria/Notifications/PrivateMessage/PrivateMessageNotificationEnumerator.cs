using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Community.Conference;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Web;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria.Notifications.PrivateMessage
{
    /// <summary>
    /// </summary>
    public sealed class PrivateMessageNotificationEnumerator : INotificationEnumerator<PrivateMessageNotification>
    {
        private readonly Senpai _senpai;
        private int _itemIndex = -1;
        private PrivateMessageNotification[] _notifications = new PrivateMessageNotification[0];

        internal PrivateMessageNotificationEnumerator(Senpai senpai)
        {
            this._senpai = senpai;
        }

        #region Properties

        /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        public PrivateMessageNotification Current => this._notifications[this._itemIndex];

        /// <summary>Gets the current element in the collection.</summary>
        /// <returns>The current element in the collection.</returns>
        object IEnumerator.Current => this.Current;

        #endregion

        #region Inherited

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this._notifications = new PrivateMessageNotification[0];
        }

        /// <summary>Advances the enumerator to the next element of the collection.</summary>
        /// <returns>
        ///     true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the
        ///     end of the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public bool MoveNext()
        {
            this._itemIndex++;
            if (this._notifications.Any()) return this._itemIndex < this._notifications.Length;

            ProxerResult lGetNotificationsResult = Task.Run(this.GetNotifications).Result;
            if (!lGetNotificationsResult.Success)
                throw lGetNotificationsResult.Exceptions.FirstOrDefault() ?? new WrongResponseException();
            return lGetNotificationsResult.Success && this._notifications.Any();
        }

        /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public void Reset()
        {
            this._itemIndex = -1;
            this._notifications = new PrivateMessageNotification[0];
        }

        #endregion

        #region

        [ItemNotNull]
        private async Task<ProxerResult> GetNotifications()
        {
            HtmlDocument lDocument = new HtmlDocument();
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/messages?format=raw&s=notification"),
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode[] lNodes =
                    lDocument.DocumentNode.SelectNodesUtility("class", "conferenceList").ToArray();

                this._notifications = (from curNode in lNodes
                    let lTitle =
                        curNode.ChildNodes[curNode.FirstChild.Name.Equals("img") ? 1 : 0].InnerText
                    let lTimeStamp =
                        DateTime.ParseExact(
                            curNode.ChildNodes[curNode.FirstChild.Name.Equals("img") ? 2 : 1].InnerText, "dd.MM.yyyy",
                            CultureInfo.InvariantCulture)
                    let lId =
                        Convert.ToInt32(
                            curNode.Attributes["href"].Value.GetTagContents("?id=", "#top").FirstOrDefault() ?? "-1")
                    select new PrivateMessageNotification(new Conference(lTitle, lId, this._senpai), lTimeStamp))
                    .ToArray();

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult(ErrorHandler.HandleError(this._senpai, lResponse, false).Exceptions);
            }
        }

        #endregion
    }
}