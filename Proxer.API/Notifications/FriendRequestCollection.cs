using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Proxer.API.Exceptions;
using Proxer.API.Utilities;

// ReSharper disable CoVariantArrayConversion

// ReSharper disable InvertIf

namespace Proxer.API.Notifications
{
    /// <summary>
    /// </summary>
    public class FriendRequestCollection : INotificationCollection
    {
        private readonly Senpai _senpai;
        private FriendRequestObject[] _friendRequestObjects;
        private INotificationObject[] _notificationObjects;

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        internal FriendRequestCollection(Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationObjectType.FriendRequest;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public NotificationObjectType Type { get; private set; }

        #endregion

        #region Geerbt

        /// <summary>
        /// </summary>
        /// <param name="count"></param>
        /// <exception cref="NotLoggedInException"></exception>
        /// <returns></returns>
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
        /// </summary>
        /// <exception cref="NotLoggedInException"></exception>
        /// <returns></returns>
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
        /// </summary>
        /// <param name="count"></param>
        /// <exception cref="NotLoggedInException"></exception>
        /// <returns></returns>
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
        /// </summary>
        /// <exception cref="NotLoggedInException"></exception>
        /// <returns></returns>
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
                    let lDescription = curNode.ChildNodes[3].ChildNodes[1].InnerText
                    let lDatumSplit = curNode.ChildNodes[4].InnerText.Split('-')
                    let lDatum =
                        new DateTime(Convert.ToInt32(lDatumSplit[0]), Convert.ToInt32(lDatumSplit[1]),
                            Convert.ToInt32(lDatumSplit[2]))
                    let lOnline =
                        curNode.ChildNodes[1].ChildNodes[1].FirstChild.Attributes["src"].Value.Equals(
                            "/images/misc/onlineicon.png")
                    select new FriendRequestObject(lUserName, lUserId, lDescription, lDatum, lOnline, this._senpai))
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