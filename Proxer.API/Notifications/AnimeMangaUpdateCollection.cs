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
        public NotificationObjectType Type { get; private set; }

        /// <summary>
        ///     Gibt eine bestimmte Anzahl der aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        /// <para>(Vererbt von <see cref="INotificationCollection"/>)</para>
        /// </summary>
        /// <param name="count">Die Anzahl der Benachrichtigungen</param>
        /// <seealso cref="INotificationCollection.GetAllNotifications">GetAllNotifications Funktion</seealso>
        /// <exception cref="NotLoggedInException">Wird ausgelöst, wenn der Benutzer noch nicht eingeloggt ist.</exception>
        /// <seealso cref="Senpai.Login"/>
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
        /// <exception cref="NotLoggedInException">Wird ausgelöst, wenn der Benutzer noch nicht eingeloggt ist.</exception>
        /// <seealso cref="Senpai.Login"/>
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
        public async Task<AnimeMangaUpdateObject[]> GetAnimeMangaUpdates(int count)
        {
            if (this._animeMangaUpdateObjects == null)
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

            return this._animeMangaUpdateObjects.Length >= count
                ? this._animeMangaUpdateObjects
                : this._animeMangaUpdateObjects.Take(count).ToArray();
        }

        /// <summary>
        /// Gibt alle aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        /// </summary>
        /// <exception cref="NotLoggedInException">Wird ausgelöst, wenn der Benutzer noch nicht eingeloggt ist.</exception>
        /// <seealso cref="Senpai.Login"/>
        /// <returns>Ein Array mit allen aktuellen Benachrichtigungen.</returns>
        public async Task<AnimeMangaUpdateObject[]> GetAllAnimeMangaUpdates()
        {
            if (this._animeMangaUpdateObjects == null)
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

            return this._animeMangaUpdateObjects;
        }


        private async Task GetInfos()
        {
            if (!this._senpai.LoggedIn) throw new NotLoggedInException();

            HtmlDocument lDocument = new HtmlDocument();
            string lResponse =
                await HttpUtility.GetWebRequestResponse(
                    "https://proxer.me/components/com_proxer/misc/notifications_misc.php", this._senpai.LoginCookies);

            if (!Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler)) return;
            try
            {
                lDocument.LoadHtml(lResponse);

                if (lDocument.ParseErrors.Any()) return;
                HtmlNodeCollection lNodes =
                    lDocument.DocumentNode.SelectNodes("//a[@class='notificationList']");

                if (lNodes == null) return;

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
            }
            catch (NullReferenceException)
            {
                this._senpai.ErrHandler.Add(lResponse);
            }
        }

        #endregion
    }
}