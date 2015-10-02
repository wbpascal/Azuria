using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Proxer.API.Exceptions;
using Proxer.API.Utilities;

namespace Proxer.API.Notifications
{
    /// <summary>
    /// Eine Klasse, die eine Sammlung von <see cref="FriendRequestObject">Freundschaftsanfragen</see> darstellt
    /// </summary>
    public class FriendRequestCollection : INotificationCollection
    {
        private readonly Senpai _senpai;
        private FriendRequestObject[] _friendRequestObjects;
        private INotificationObject[] _notificationObjects;


        internal FriendRequestCollection(Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationObjectType.FriendRequest;
        }

        #region Geerbt

        /// <summary>
        ///     Gibt den Typ der Benachrichtigung zurück.
        /// <para>(Vererbt von <see cref="INotificationCollection"/>)</para>
        /// </summary>
        public NotificationObjectType Type { get; private set; }

        /// <summary>
        ///     Gibt eine bestimmte Anzahl der aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        /// <para>(Vererbt von <see cref="INotificationCollection"/>)</para>
        /// </summary>
        /// <param name="count">Die Anzahl der Benachrichtigungen</param>
        /// <seealso cref="INotificationCollection.GetAllNotifications">GetAllNotifications Funktion</seealso>
        /// <seealso cref="Senpai.Login"/>
        /// <exception cref="NotLoggedInException">Wird ausgelöst, wenn der Benutzer nicht eingeloggt ist.</exception>
        /// <returns>
        ///     Ein Array mit der Anzahl an Elementen in <paramref name="count" /> spezifiziert.
        ///     Wenn <paramref name="count" /> > Array.length, dann wird der gesamte Array zurückgegeben.
        /// </returns>
        public async Task<INotificationObject[]> GetNotifications(int count)
        {
            if (this._notificationObjects == null)
            {
                try
                {
                    await this.GetInfos();
                }
                catch (NotLoggedInException)
                {
                    throw new NotLoggedInException();
                }
            }

            return this._notificationObjects.Length >= count
                ? this._notificationObjects
                : this._notificationObjects.Take(count).ToArray();
        }

        /// <summary>
        /// Gibt alle aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        /// <para>(Vererbt von <see cref="INotificationCollection"/>)</para>
        /// </summary>
        /// <seealso cref="INotificationCollection.GetNotifications"/>
        /// <seealso cref="Senpai.Login"/>
        /// <exception cref="NotLoggedInException">Wird ausgelöst, wenn der Benutzer noch nicht eingeloggt ist.</exception>
        /// <returns>Ein Array mit allen aktuellen Benachrichtigungen.</returns>
        public async Task<INotificationObject[]> GetAllNotifications()
        {
            if (this._notificationObjects == null)
            {
                try
                {
                    await this.GetInfos();
                }
                catch (NotLoggedInException)
                {
                    throw new NotLoggedInException();
                }
            }

            return this._notificationObjects;
        }

        #endregion

        #region

        /// <summary>
        ///     Gibt eine bestimmte Anzahl der aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        /// </summary>
        /// <param name="count">Die Anzahl der Benachrichtigungen</param>
        /// <exception cref="NotLoggedInException">Tritt auf, wenn der Benutzer noch nicht angemeldet ist.</exception>
        /// <seealso cref="Senpai.Login" />
        /// <returns>
        ///     Ein Array mit der Anzahl an Elementen in <paramref name="count" /> spezifiziert.
        ///     Wenn <paramref name="count" /> > Array.length, dann wird der gesamte Array zurückgegeben.
        /// </returns>
        public async Task<FriendRequestObject[]> GetFriendRequests(int count)
        {
            if (this._friendRequestObjects == null)
            {
                try
                {
                    await this.GetInfos();
                }
                catch (NotLoggedInException)
                {
                    throw new NotLoggedInException();
                }
            }

            return this._friendRequestObjects.Length >= count
                ? this._friendRequestObjects
                : this._friendRequestObjects.Take(count).ToArray();
        }

        /// <summary>
        /// Gibt alle aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        /// </summary>
        /// <exception cref="NotLoggedInException">Wird ausgelöst, wenn der Benutzer noch nicht eingeloggt ist.</exception>
        /// <seealso cref="Senpai.Login"/>
        /// <returns>Ein Array mit allen aktuellen Benachrichtigungen.</returns>
        public async Task<FriendRequestObject[]> GetAllFriendRequests()
        {
            if (this._friendRequestObjects == null)
            {
                try
                {
                    await this.GetInfos();
                }
                catch (NotLoggedInException)
                {
                    throw new NotLoggedInException();
                }
            }

            return this._friendRequestObjects;
        }


        private async Task GetInfos()
        {
            if (!this._senpai.LoggedIn) throw new NotLoggedInException();
            HtmlDocument lDocument = new HtmlDocument();
            string lResponse =
                (await HttpUtility.GetWebRequestResponse("https://proxer.me/user/my/connections?format=raw",
                    this._senpai.LoginCookies))
                    .Replace("</link>", "").Replace("\n", "");

            if (!Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler)) return;
            try
            {
                lDocument.LoadHtml(lResponse);

                if (lDocument.ParseErrors.Any()) return;
                HtmlNodeCollection lNodes = lDocument.DocumentNode.SelectNodes("//tr");

                if (lNodes == null) return;
                List<FriendRequestObject> lFriendRequests = (from curNode in lNodes
                    where
                        curNode.Id.StartsWith("entry") &&
                        curNode.FirstChild.FirstChild.Attributes["class"].Value.Equals("accept")
                    let lUserId = Convert.ToInt32(curNode.Id.Replace("entry", ""))
                    let lUserName = curNode.InnerText.Split("  ".ToCharArray())[0]
                    let lDatumSplit = curNode.ChildNodes[4].InnerText.Split('-')
                    let lDatum =
                        new DateTime(Convert.ToInt32(lDatumSplit[0]), Convert.ToInt32(lDatumSplit[1]),
                            Convert.ToInt32(lDatumSplit[2]))
                    select new FriendRequestObject(lUserName, lUserId, lDatum, this._senpai))
                    .ToList();

                this._friendRequestObjects = lFriendRequests.ToArray();
                this._notificationObjects = lFriendRequests.ToArray();
            }
            catch (NullReferenceException)
            {
                this._senpai.ErrHandler.Add(lResponse);
            }
        }

        #endregion
    }
}