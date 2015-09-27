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
        /// </summary>
        /// <exception cref="NotLoggedInException"></exception>
        /// <returns></returns>
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