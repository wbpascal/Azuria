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
    /// Eine Klasse, die eine Sammlung von <see cref="PmObject">Private Nachrichten</see> darstellt.
    /// </summary>
    public class PmCollection : INotificationCollection
    {
        private readonly Senpai _senpai;
        private INotificationObject[] _notificationObjects;
        private PmObject[] _pmObjects;

        internal PmCollection(Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationObjectType.PrivateMessage;
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
        public async Task<PmObject[]> GetPrivateMessages(int count)
        {
            if (this._pmObjects == null)
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

            return this._pmObjects.Length >= count
                ? this._pmObjects
                : this._pmObjects.Take(count).ToArray();
        }

        /// <summary>
        /// Gibt alle aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        /// </summary>
        /// <exception cref="NotLoggedInException">Wird ausgelöst, wenn der Benutzer noch nicht eingeloggt ist.</exception>
        /// <seealso cref="Senpai.Login"/>
        /// <returns>Ein Array mit allen aktuellen Benachrichtigungen.</returns>
        public async Task<PmObject[]> GetAllPrivateMessages()
        {
            if (this._pmObjects == null)
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

            return this._pmObjects;
        }


        private async Task GetInfos()
        {
            if (!this._senpai.LoggedIn) throw new NotLoggedInException();

            HtmlDocument lDocument = new HtmlDocument();
            string lResponse =
                (await HttpUtility.GetWebRequestResponse("https://proxer.me/messages?format=raw&s=notification",
                    this._senpai.LoginCookies)).Replace("</link>", "").Replace("\n", "");

            if (!Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler)) return;
            try
            {
                lDocument.LoadHtml(lResponse);

                if (lDocument.ParseErrors.Any()) return;
                HtmlNodeCollection lNodes =
                    lDocument.DocumentNode.SelectNodes("//a[@class='conferenceList']");

                if (lNodes != null)
                {
                    List<PmObject> lPmObjects = new List<PmObject>();
                    foreach (HtmlNode curNode in lNodes)
                    {
                        string lTitel;
                        string[] lDatum;
                        if (curNode.ChildNodes[1].Name.ToLower().Equals("img"))
                        {
                            lTitel = curNode.ChildNodes[0].InnerText;
                            lDatum = curNode.ChildNodes[1].InnerText.Split('.');

                            DateTime lTimeStamp = new DateTime(Convert.ToInt32(lDatum[2]),
                                Convert.ToInt32(lDatum[1]), Convert.ToInt32(lDatum[0]));
                            int lId =
                                Convert.ToInt32(curNode.Attributes["href"].Value.Substring(13,
                                    curNode.Attributes["href"].Value.Length - 17));

                            lPmObjects.Add(new PmObject(lId, lTitel, lTimeStamp));
                        }
                        else
                        {
                            lTitel = curNode.ChildNodes[0].InnerText;
                            lDatum = curNode.ChildNodes[1].InnerText.Split('.');

                            DateTime lTimeStamp = new DateTime(Convert.ToInt32(lDatum[2]),
                                Convert.ToInt32(lDatum[1]), Convert.ToInt32(lDatum[0]));
                            int lId =
                                Convert.ToInt32(curNode.Attributes["href"].Value.Substring(13,
                                    curNode.Attributes["href"].Value.Length - 17));

                            lPmObjects.Add(new PmObject(lTitel, lId, lTimeStamp));
                        }
                    }

                    this._pmObjects = lPmObjects.ToArray();
                    this._notificationObjects = lPmObjects.ToArray();
                }
            }
            catch (NullReferenceException)
            {
                this._senpai.ErrHandler.Add(lResponse);
            }
        }

        #endregion
    }
}