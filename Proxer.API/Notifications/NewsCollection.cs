using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Proxer.API.Exceptions;
using Proxer.API.Utilities;

// ReSharper disable CoVariantArrayConversion

// ReSharper disable InvertIf

namespace Proxer.API.Notifications
{
    /// <summary>
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
        /// </summary>
        /// <exception cref="NotLoggedInException"></exception>
        /// <returns></returns>
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