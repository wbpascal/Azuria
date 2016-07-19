using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Web;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria.Notifications.FriendRequest
{
    /// <summary>
    /// </summary>
    public class FriendRequestNotificationEnumerator : INotificationEnumerator<FriendRequestNotification>
    {
        private readonly Senpai _senpai;
        private int _itemIndex = -1;
        private FriendRequestNotification[] _notifications = new FriendRequestNotification[0];

        internal FriendRequestNotificationEnumerator(Senpai senpai)
        {
            this._senpai = senpai;
        }

        #region Properties

        /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        public FriendRequestNotification Current => this._notifications[this._itemIndex];

        /// <summary>Gets the current element in the collection.</summary>
        /// <returns>The current element in the collection.</returns>
        object IEnumerator.Current => this.Current;

        #endregion

        #region Inherited

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this._notifications = new FriendRequestNotification[0];
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
            return this._notifications.Any();
        }

        /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public void Reset()
        {
            this._itemIndex = -1;
            this._notifications = new FriendRequestNotification[0];
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
                        new Uri("https://proxer.me/user/my/connections?format=raw"),
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                IEnumerable<HtmlNode> lNodes = lDocument.DocumentNode.DescendantsAndSelf().Where(x => x.Name == "tr");

                this._notifications = (from curNode in lNodes
                    where
                        curNode.Id.StartsWith("entry") &&
                        curNode.FirstChild.FirstChild.Attributes["class"].Value
                            .Equals
                            ("accept")
                    let lUserId =
                        Convert.ToInt32(curNode.Id.Replace("entry", ""))
                    let lUserName =
                        curNode.InnerText.Split("  ".ToCharArray())[0]
                    let lDatum =
                        DateTime.ParseExact(curNode.ChildNodes[4].InnerText, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                    select
                        new FriendRequestNotification(new User.User(lUserName, lUserId, this._senpai), lDatum,
                            this._senpai)).ToArray();

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