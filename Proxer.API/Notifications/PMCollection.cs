using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Proxer.API.Exceptions;
using Proxer.API.Utilities;
using Proxer.API.Utilities.Net;
using RestSharp;

namespace Proxer.API.Notifications
{
    /// <summary>
    ///     Eine Klasse, die eine Sammlung von <see cref="PmObject">Private Nachrichten</see> darstellt.
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
        ///     <para>(Vererbt von <see cref="INotificationCollection" />)</para>
        /// </summary>
        public NotificationObjectType Type { get; }

        /// <summary>
        ///     Gibt eine bestimmte Anzahl der aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        ///     <para>(Vererbt von <see cref="INotificationCollection" />)</para>
        /// </summary>
        /// <param name="count">Die Anzahl der Benachrichtigungen</param>
        /// <seealso cref="INotificationCollection.GetAllNotifications">GetAllNotifications Funktion</seealso>
        /// <seealso cref="Senpai.Login" />
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
        ///     Gibt alle aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        ///     <para>(Vererbt von <see cref="INotificationCollection" />)</para>
        /// </summary>
        /// <seealso cref="INotificationCollection.GetNotifications" />
        /// <seealso cref="Senpai.Login" />
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
        public async Task<ProxerResult<PmObject[]>> GetPrivateMessages(int count)
        {
            if (this._notificationObjects != null)
                return this._notificationObjects.Length >= count
                    ? new ProxerResult<PmObject[]>(this._pmObjects)
                    : new ProxerResult<PmObject[]>(this._pmObjects.Take(count).ToArray());
            ProxerResult lResult;
            if (!(lResult = await this.GetInfos()).Success)
                return new ProxerResult<PmObject[]>(lResult.Exceptions);

            return this._notificationObjects.Length >= count
                ? new ProxerResult<PmObject[]>(this._pmObjects)
                : new ProxerResult<PmObject[]>(this._pmObjects.Take(count).ToArray());
        }

        /// <summary>
        ///     Gibt alle aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        /// </summary>
        /// <seealso cref="Senpai.Login" />
        /// <returns>Ein Array mit allen aktuellen Benachrichtigungen.</returns>
        public async Task<ProxerResult<PmObject[]>> GetAllPrivateMessages()
        {
            if (this._notificationObjects != null)
                return new ProxerResult<PmObject[]>(this._pmObjects);

            ProxerResult lResult;
            return !(lResult = await this.GetInfos()).Success
                ? new ProxerResult<PmObject[]>(lResult.Exceptions)
                : new ProxerResult<PmObject[]>(this._pmObjects);
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
                        "https://proxer.me/messages?format=raw&s=notification",
                        this._senpai.LoginCookies);
            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseObject.Content))
                lResponse = System.Web.HttpUtility.HtmlDecode(lResponseObject.Content).Replace("\n", "");
            else return new ProxerResult(new[] {new WrongResponseException(), lResponseObject.ErrorException});

            if (string.IsNullOrEmpty(lResponse) ||
                !Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler))
                return new ProxerResult(new Exception[] {new WrongResponseException()});

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNodeCollection lNodes =
                    lDocument.DocumentNode.SelectNodes("//a[@class='conferenceList']");

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
                this._notificationObjects = lPmObjects.Cast<INotificationObject>().ToArray();

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