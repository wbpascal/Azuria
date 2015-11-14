using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Proxer.API.Exceptions;
using Proxer.API.Utilities;
using Proxer.API.Utilities.Net;

namespace Proxer.API.Notifications
{
    /// <summary>
    ///     Eine Klasse, die eine Sammlung von <see cref="FriendRequestObject">Freundschaftsanfragen</see> darstellt
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
        ///     <para>(Vererbt von <see cref="INotificationCollection" />)</para>
        /// </summary>
        public NotificationObjectType Type { get; }


        /// <summary>
        ///     Gibt alle aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        ///     <para>(Vererbt von <see cref="INotificationCollection" />)</para>
        ///     <para>Mögliche Fehler, die <see cref="ProxerResult" /> enthalten kann:</para>
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Ausnahme</term>
        ///             <description>Beschreibung</description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 <see cref="NotLoggedInException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="WrongResponseException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <seealso cref="INotificationCollection.GetNotifications" />
        /// <seealso cref="Senpai.Login" />
        /// <returns>Ein Array mit allen aktuellen Benachrichtigungen.</returns>
        public async Task<ProxerResult<IEnumerable<INotificationObject>>> GetAllNotifications()
        {
            if (this._notificationObjects != null)
                return new ProxerResult<IEnumerable<INotificationObject>>(this._notificationObjects);

            ProxerResult lResult;
            return !(lResult = await this.GetInfos()).Success
                ? new ProxerResult<IEnumerable<INotificationObject>>(lResult.Exceptions)
                : new ProxerResult<IEnumerable<INotificationObject>>(this._notificationObjects);
        }

        /// <summary>
        ///     Gibt eine bestimmte Anzahl der aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        ///     <para>(Vererbt von <see cref="INotificationCollection" />)</para>
        ///     <para>Mögliche Fehler, die <see cref="ProxerResult" /> enthalten kann:</para>
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Ausnahme</term>
        ///             <description>Beschreibung</description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 <see cref="NotLoggedInException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="WrongResponseException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="count">Die Anzahl der Benachrichtigungen</param>
        /// <seealso cref="INotificationCollection.GetAllNotifications">GetAllNotifications Funktion</seealso>
        /// <seealso cref="Senpai.Login" />
        /// <returns>
        ///     Ein Array mit der Anzahl an Elementen in <paramref name="count" /> spezifiziert.
        ///     Wenn <paramref name="count" /> > Array.length, dann wird der gesamte Array zurückgegeben.
        /// </returns>
        public async Task<ProxerResult<IEnumerable<INotificationObject>>> GetNotifications(int count)
        {
            if (this._notificationObjects != null)
                return this._notificationObjects.Length >= count
                    ? new ProxerResult<IEnumerable<INotificationObject>>(this._notificationObjects)
                    : new ProxerResult<IEnumerable<INotificationObject>>(this._notificationObjects.Take(count).ToArray());
            ProxerResult lResult;
            if (!(lResult = await this.GetInfos()).Success)
                return new ProxerResult<IEnumerable<INotificationObject>>(lResult.Exceptions);

            return this._notificationObjects.Length >= count
                ? new ProxerResult<IEnumerable<INotificationObject>>(this._notificationObjects)
                : new ProxerResult<IEnumerable<INotificationObject>>(this._notificationObjects.Take(count).ToArray());
        }

        #endregion

        #region

        /// <summary>
        ///     Gibt eine bestimmte Anzahl der aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        ///     <para>Mögliche Fehler, die <see cref="ProxerResult" /> enthalten kann:</para>
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Ausnahme</term>
        ///             <description>Beschreibung</description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 <see cref="NotLoggedInException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="WrongResponseException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="count">Die Anzahl der Benachrichtigungen</param>
        /// <seealso cref="Senpai.Login" />
        /// <returns>
        ///     Ein Array mit der Anzahl an Elementen in <paramref name="count" /> spezifiziert.
        ///     Wenn <paramref name="count" /> > Array.length, dann wird der gesamte Array zurückgegeben.
        /// </returns>
        public async Task<ProxerResult<FriendRequestObject[]>> GetFriendRequests(int count)
        {
            if (this._notificationObjects != null)
                return this._notificationObjects.Length >= count
                    ? new ProxerResult<FriendRequestObject[]>(this._friendRequestObjects)
                    : new ProxerResult<FriendRequestObject[]>(this._friendRequestObjects.Take(count).ToArray());
            ProxerResult lResult;
            if (!(lResult = await this.GetInfos()).Success)
                return new ProxerResult<FriendRequestObject[]>(lResult.Exceptions);

            return this._notificationObjects.Length >= count
                ? new ProxerResult<FriendRequestObject[]>(this._friendRequestObjects)
                : new ProxerResult<FriendRequestObject[]>(this._friendRequestObjects.Take(count).ToArray());
        }

        /// <summary>
        ///     Gibt alle aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        ///     <para>Mögliche Fehler, die <see cref="ProxerResult" /> enthalten kann:</para>
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Ausnahme</term>
        ///             <description>Beschreibung</description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 <see cref="NotLoggedInException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="WrongResponseException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <seealso cref="Senpai.Login" />
        /// <returns>Ein Array mit allen aktuellen Benachrichtigungen.</returns>
        public async Task<ProxerResult<FriendRequestObject[]>> GetAllFriendRequests()
        {
            if (this._notificationObjects != null)
                return new ProxerResult<FriendRequestObject[]>(this._friendRequestObjects);

            ProxerResult lResult;
            return !(lResult = await this.GetInfos()).Success
                ? new ProxerResult<FriendRequestObject[]>(lResult.Exceptions)
                : new ProxerResult<FriendRequestObject[]>(this._friendRequestObjects);
        }


        private async Task<ProxerResult> GetInfos()
        {
            HtmlDocument lDocument = new HtmlDocument();
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        "https://proxer.me/user/my/connections?format=raw",
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNodeCollection lNodes = lDocument.DocumentNode.SelectNodes("//tr");

                List<FriendRequestObject> lFriendRequests = new List<FriendRequestObject>();

                if (lNodes != null)
                    lFriendRequests = (from curNode in lNodes
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
                                           new FriendRequestObject(lUserName, lUserId, lDatum,
                                               this._senpai))
                        .ToList();

                this._friendRequestObjects = lFriendRequests.ToArray();
                this._notificationObjects = lFriendRequests.Cast<INotificationObject>().ToArray();

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