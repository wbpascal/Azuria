using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Proxer.API.Exceptions;
using Proxer.API.Utilities;
using Proxer.API.Utilities.Net;

namespace Proxer.API.Notifications
{
    /// <summary>
    ///     Eine Klasse, die eine Sammlung von <see cref="NewsObject">News</see> darstellt.
    /// </summary>
    public class NewsCollection : INotificationCollection
    {
        private readonly Senpai _senpai;
        private NewsObject[] _newsObjects;
        private INotificationObject[] _notificationObjects;

        internal NewsCollection(Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationObjectType.News;
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
        public async Task<ProxerResult<NewsObject[]>> GetNews(int count)
        {
            if (this._notificationObjects != null)
                return this._notificationObjects.Length >= count
                    ? new ProxerResult<NewsObject[]>(this._newsObjects)
                    : new ProxerResult<NewsObject[]>(this._newsObjects.Take(count).ToArray());
            ProxerResult lResult;
            if (!(lResult = await this.GetInfos()).Success)
                return new ProxerResult<NewsObject[]>(lResult.Exceptions);

            return this._notificationObjects.Length >= count
                ? new ProxerResult<NewsObject[]>(this._newsObjects)
                : new ProxerResult<NewsObject[]>(this._newsObjects.Take(count).ToArray());
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
        public async Task<ProxerResult<NewsObject[]>> GetAllNews()
        {
            if (this._notificationObjects != null)
                return new ProxerResult<NewsObject[]>(this._newsObjects);

            ProxerResult lResult;
            return !(lResult = await this.GetInfos()).Success
                ? new ProxerResult<NewsObject[]>(lResult.Exceptions)
                : new ProxerResult<NewsObject[]>(this._newsObjects);
        }


        private async Task<ProxerResult> GetInfos()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        "https://proxer.me/notifications?format=json&s=news&p=1",
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            if (!lResponse.StartsWith("{\"error\":0"))
                return new ProxerResult
                {
                    Success = false
                };

            try
            {
                Dictionary<string, List<NewsObject>> lDeserialized =
                    JsonConvert.DeserializeObject<Dictionary<string, List<NewsObject>>>("{" +
                                                                                        lResponse.Substring(
                                                                                            "{\"error\":0,".Length));

                this._newsObjects = lDeserialized["notifications"].ToArray();
                this._notificationObjects = lDeserialized["notifications"].Cast<INotificationObject>().ToArray();

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