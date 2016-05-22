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
    ///     Eine Klasse, die eine Sammlung von <see cref="Main.Anime">Anime-</see> und <see cref="Main.Manga">Manga-</see>
    ///     Benachrichtigungen
    ///     darstellt.
    /// </summary>
    public class AnimeMangaUpdateCollection : INotificationCollection
    {
        private readonly Senpai _senpai;
        private AnimeMangaUpdateObject[] _animeMangaUpdateObjects;
        private INotificationObject[] _notificationObjects;

        internal AnimeMangaUpdateCollection([NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationObjectType.AnimeManga;
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
        /// </summary>
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
        /// </summary>
        /// <param name="count">Die Anzahl der Benachrichtigungen</param>
        /// <seealso cref="INotificationCollection.GetAllNotifications">GetAllNotifications Funktion</seealso>
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
        /// </summary>
        /// <param name="count">Die Anzahl der Benachrichtigungen</param>
        /// <returns>
        ///     Ein Array mit der Anzahl an Elementen in <paramref name="count" /> spezifiziert.
        ///     Wenn <paramref name="count" /> > Array.length, dann wird der gesamte Array zurückgegeben.
        /// </returns>
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<AnimeMangaUpdateObject>>> GetAnimeMangaUpdates(int count)
        {
            if (this._animeMangaUpdateObjects != null)
                return this._animeMangaUpdateObjects.Length >= count
                    ? new ProxerResult<IEnumerable<AnimeMangaUpdateObject>>(this._animeMangaUpdateObjects)
                    : new ProxerResult<IEnumerable<AnimeMangaUpdateObject>>(
                        this._animeMangaUpdateObjects.Take(count).ToArray());

            ProxerResult lResult;
            if (!(lResult = await this.GetInfos()).Success)
                return new ProxerResult<IEnumerable<AnimeMangaUpdateObject>>(lResult.Exceptions);

            return this._animeMangaUpdateObjects.Length >= count
                ? new ProxerResult<IEnumerable<AnimeMangaUpdateObject>>(this._animeMangaUpdateObjects)
                : new ProxerResult<IEnumerable<AnimeMangaUpdateObject>>(
                    this._animeMangaUpdateObjects.Take(count).ToArray());
        }

        /// <summary>
        ///     Gibt alle aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        /// </summary>
        /// <returns>Ein Array mit allen aktuellen Benachrichtigungen.</returns>
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<AnimeMangaUpdateObject>>> GetAllAnimeMangaUpdates()
        {
            if (this._animeMangaUpdateObjects != null)
                return new ProxerResult<IEnumerable<AnimeMangaUpdateObject>>(this._animeMangaUpdateObjects);

            ProxerResult lResult;
            return !(lResult = await this.GetInfos()).Success
                ? new ProxerResult<IEnumerable<AnimeMangaUpdateObject>>(lResult.Exceptions)
                : new ProxerResult<IEnumerable<AnimeMangaUpdateObject>>(this._animeMangaUpdateObjects);
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
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode[] lNodes =
                    lDocument.DocumentNode.SelectNodesUtility("class", "notificationList").ToArray();

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
                this._notificationObjects = lAnimeMangaUpdateObjects.Cast<INotificationObject>().ToArray();

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