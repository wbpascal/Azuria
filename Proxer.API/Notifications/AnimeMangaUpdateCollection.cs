using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Proxer.API.Exceptions;
using Proxer.API.Main;
using Proxer.API.Utilities;
using Proxer.API.Utilities.Net;
using RestSharp;

namespace Proxer.API.Notifications
{
    /// <summary>
    ///     Eine Klasse, die eine Sammlung von <see cref="Main.Anime">Anime-</see> und <see cref="Main.Manga">Manga-</see>Benachrichtigungen
    ///     darstellt.
    /// </summary>
    public class AnimeMangaUpdateCollection : INotificationCollection
    {
        private readonly Senpai _senpai;
        private AnimeMangaUpdateObject[] _animeMangaUpdateObjects;
        private INotificationObject[] _notificationObjects;

        internal AnimeMangaUpdateCollection(Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationObjectType.AnimeManga;
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
        public async Task<ProxerResult<AnimeMangaUpdateObject[]>> GetAnimeMangaUpdates(int count)
        {
            if (this._animeMangaUpdateObjects != null)
                return this._animeMangaUpdateObjects.Length >= count
                    ? new ProxerResult<AnimeMangaUpdateObject[]>(this._animeMangaUpdateObjects)
                    : new ProxerResult<AnimeMangaUpdateObject[]>(this._animeMangaUpdateObjects.Take(count).ToArray());

            ProxerResult lResult;
            if (!(lResult = await this.GetInfos()).Success)
                return new ProxerResult<AnimeMangaUpdateObject[]>(lResult.Exceptions);

            return this._animeMangaUpdateObjects.Length >= count
                ? new ProxerResult<AnimeMangaUpdateObject[]>(this._animeMangaUpdateObjects)
                : new ProxerResult<AnimeMangaUpdateObject[]>(this._animeMangaUpdateObjects.Take(count).ToArray());
        }

        /// <summary>
        /// Gibt alle aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        /// </summary>
        /// <seealso cref="Senpai.Login"/>
        /// <returns>Ein Array mit allen aktuellen Benachrichtigungen.</returns>
        public async Task<ProxerResult<AnimeMangaUpdateObject[]>> GetAllAnimeMangaUpdates()
        {
            if (this._animeMangaUpdateObjects != null)
                return new ProxerResult<AnimeMangaUpdateObject[]>(this._animeMangaUpdateObjects);

            ProxerResult lResult;
            return !(lResult = await this.GetInfos()).Success
                ? new ProxerResult<AnimeMangaUpdateObject[]>(lResult.Exceptions)
                : new ProxerResult<AnimeMangaUpdateObject[]>(this._animeMangaUpdateObjects);
        }


        private async Task<ProxerResult> GetInfos()
        {
            if (!this._senpai.LoggedIn)
                return new ProxerResult(new Exception[] {new NotLoggedInException(this._senpai)});

            HtmlDocument lDocument = new HtmlDocument();
            string lResponse;

            IRestResponse lResponseObject =
                await
                    HttpUtility.GetWebRequestResponse(
                        "https://proxer.me/components/com_proxer/misc/notifications_misc.php",
                        this._senpai.LoginCookies);
            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseObject.Content))
                lResponse = System.Web.HttpUtility.HtmlDecode(lResponseObject.Content).Replace("\n", "");
            else return new ProxerResult(new[] { new WrongResponseException(), lResponseObject.ErrorException });

            if (string.IsNullOrEmpty(lResponse) ||
                !Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler))
                return new ProxerResult(new Exception[] { new WrongResponseException() });

            try
            {
                lDocument.LoadHtml(lResponse);
                
                HtmlNodeCollection lNodes =
                    lDocument.DocumentNode.SelectNodes("//a[@class='notificationList']");

                List<AnimeMangaUpdateObject> lAnimeMangaUpdateObjects = new List<AnimeMangaUpdateObject>();
                foreach (HtmlNode curNode in lNodes.Where(curNode => curNode.InnerText.StartsWith("Lesezeichen:")))
                {
                    string lName;
                    int lNumber;

                    int lId = Convert.ToInt32(curNode.Id.Substring(12));
                    string lMessage = curNode.ChildNodes["u"].InnerText;
                    Uri lLink = new Uri("https://proxer.me" + curNode.Attributes["href"].Value);

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

                    lAnimeMangaUpdateObjects.Add(new AnimeMangaUpdateObject(lMessage, lName, lNumber,
                        lLink, lId));
                }

                this._animeMangaUpdateObjects = lAnimeMangaUpdateObjects.ToArray();
                this._notificationObjects = lAnimeMangaUpdateObjects.ToArray();

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