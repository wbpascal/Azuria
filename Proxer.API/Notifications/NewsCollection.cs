using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Proxer.API.Exceptions;
using Proxer.API.Utilities;

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
        public NotificationObjectType Type { get; private set; }

        /// <summary>
        ///     Gibt eine bestimmte Anzahl der aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        /// <para>(Vererbt von <see cref="INotificationCollection"/>)</para>
        /// </summary>
        /// <param name="count">Die Anzahl der Benachrichtigungen</param>
        /// <exception cref="NotLoggedInException">Wird ausgelöst, wenn der Benutzer noch nicht eingeloggt ist.</exception>
        /// <seealso cref="Senpai.Login"/>
        /// <seealso cref="INotificationCollection.GetAllNotifications">GetAllNotifications Funktion</seealso>
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
        /// <exception cref="NotLoggedInException">Wird ausgelöst, wenn der Benutzer noch nicht eingeloggt ist.</exception>
        /// <seealso cref="Senpai.Login"/>
        /// <seealso cref="INotificationCollection.GetNotifications"/>
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
        public async Task<NewsObject[]> GetNews(int count)
        {
            if (this._newsObjects == null)
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

            return this._newsObjects.Length >= count
                ? this._newsObjects
                : this._newsObjects.Take(count).ToArray();
        }

        /// <summary>
        /// Gibt alle aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        /// </summary>
        /// <exception cref="NotLoggedInException">Wird ausgelöst, wenn der Benutzer noch nicht eingeloggt ist.</exception>
        /// <seealso cref="Senpai.Login"/>
        /// <returns>Ein Array mit allen aktuellen Benachrichtigungen.</returns>
        public async Task<NewsObject[]> GetAllNews()
        {
            if (this._newsObjects == null)
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

            return this._newsObjects;
        }


        private async Task GetInfos()
        {
            if (!this._senpai.LoggedIn) throw new NotLoggedInException();
            string lResponse =
                await HttpUtility.GetWebRequestResponse("https://proxer.me/notifications?format=json&s=news&p=1",
                    this._senpai.LoginCookies);
            if (!lResponse.StartsWith("{\"error\":0")) return;
            Dictionary<string, List<NewsObject>> lDeserialized =
                JsonConvert.DeserializeObject<Dictionary<string, List<NewsObject>>>("{" +
                                                                                    lResponse.Substring(
                                                                                        "{\"error\":0,".Length));
            this._newsObjects = lDeserialized["notifications"].ToArray();
            this._notificationObjects = lDeserialized["notifications"].ToArray();
        }

        #endregion
    }
}