using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Proxer.API.Exceptions;
using Proxer.API.Utilities;
using Proxer.API.Utilities.Net;
using RestSharp;

namespace Proxer.API.Notifications
{
    /// <summary>
    /// Eine Klasse, die eine Sammlung von <see cref="NewsObject">News</see> darstellt.
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
        /// <para>(Vererbt von <see cref="INotificationCollection"/>)</para>
        /// </summary>
        public NotificationObjectType Type { get; }

        /// <summary>
        ///     Gibt eine bestimmte Anzahl der aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        /// <para>(Vererbt von <see cref="INotificationCollection"/>)</para>
        /// </summary>
        /// <param name="count">Die Anzahl der Benachrichtigungen</param>
        /// <seealso cref="INotificationCollection.GetAllNotifications">GetAllNotifications Funktion</seealso>
        /// <seealso cref="Senpai.Login"/>
        /// <returns>
        ///     Ein Array mit der Anzahl an Elementen in <paramref name="count" /> spezifiziert.
        ///     Wenn <paramref name="count" /> > Array.length, dann wird der gesamte Array zurückgegeben.
        /// </returns>
        public async Task<ProxerResult<INotificationObject[]>> GetNotifications(int count)
        {
            if (this._notificationObjects != null)
                return this._notificationObjects.Length >= count
                    ? new ProxerResult<INotificationObject[]>(this._notificationObjects)
                    : new ProxerResult<INotificationObject[]>(this._notificationObjects.Take(count).ToArray());
            ProxerResult lResult;
            if (!(lResult = await this.GetInfos()).Success)
                return new ProxerResult<INotificationObject[]>(lResult.Exceptions);

            return this._notificationObjects.Length >= count
                ? new ProxerResult<INotificationObject[]>(this._notificationObjects)
                : new ProxerResult<INotificationObject[]>(this._notificationObjects.Take(count).ToArray());
        }


        /// <summary>
        /// Gibt alle aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        /// <para>(Vererbt von <see cref="INotificationCollection"/>)</para>
        /// </summary>
        /// <seealso cref="INotificationCollection.GetNotifications"/>
        /// <seealso cref="Senpai.Login"/>
        /// <returns>Ein Array mit allen aktuellen Benachrichtigungen.</returns>
        public async Task<ProxerResult<INotificationObject[]>> GetAllNotifications()
        {
            if (this._notificationObjects != null)
                return new ProxerResult<INotificationObject[]>(this._notificationObjects);

            ProxerResult lResult;
            return !(lResult = await this.GetInfos()).Success
                ? new ProxerResult<INotificationObject[]>(lResult.Exceptions)
                : new ProxerResult<INotificationObject[]>(this._notificationObjects);
        }

        #endregion

        #region

        /// <summary>
        ///     Gibt eine bestimmte Anzahl der aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
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
        /// Gibt alle aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        /// </summary>
        /// <seealso cref="Senpai.Login"/>
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
            if (!this._senpai.LoggedIn)
                return new ProxerResult(new Exception[] {new NotLoggedInException(this._senpai)});

            string lResponse;

            IRestResponse lResponseObject =
                await
                    HttpUtility.GetWebRequestResponse(
                        "https://proxer.me/notifications?format=json&s=news&p=1",
                        this._senpai.LoginCookies);
            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseObject.Content))
                lResponse = System.Web.HttpUtility.HtmlDecode(lResponseObject.Content).Replace("\n", "");
            else return new ProxerResult(new[] { new WrongResponseException(), lResponseObject.ErrorException });

            if (string.IsNullOrEmpty(lResponse) ||
                !Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler))
                return new ProxerResult(new Exception[] { new WrongResponseException() });

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
                this._notificationObjects = lDeserialized["notifications"].ToArray();

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