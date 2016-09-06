using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Azuria.Api;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;

namespace Azuria.Notifications.News
{
    /// <summary>
    /// </summary>
    public static class NewsNotificationManager
    {
        private static readonly Dictionary<Senpai, Dictionary<NewsNotificationEventHandler, string>> CallbackDictionary
            =
            new Dictionary<Senpai, Dictionary<NewsNotificationEventHandler, string>>();

        private static readonly double TimerDelay = TimeSpan.FromMinutes(15).TotalMilliseconds;

        private static readonly Timer Timer = new Timer(TimerDelay)
        {
            AutoReset = true
        };


        static NewsNotificationManager()
        {
            Timer.Elapsed += (sender, args) => CheckNotifications();
            Timer.Enabled = true;
        }

        #region Events

        /// <summary>
        ///     Represents a method that is executed when new news notifications are available.
        /// </summary>
        /// <param name="sender">The user that recieved the notifications.</param>
        /// <param name="e">The notifications. Maximum length of 50 elements.</param>
        public delegate void NewsNotificationEventHandler(Senpai sender, IEnumerable<NewsNotification> e);

        #endregion

        #region Methods

        private static async void CheckNotifications()
        {
            Timer.Stop();
            foreach (Senpai senpai in CallbackDictionary.Keys)
            {
                ProxerResult<int> lNotificationCountResult = await GetAvailableNotificationsCount(senpai);
                if (!lNotificationCountResult.Success || (lNotificationCountResult.Result == 0)) continue;
                NewsNotification[] lNotifications =
                    new NewsNotificationCollection(senpai).Take(Math.Min(lNotificationCountResult.Result, 50)).ToArray();
                if (!lNotifications.Any()) continue;

                Dictionary<NewsNotificationEventHandler, string> lChangeDictionary =
                    new Dictionary<NewsNotificationEventHandler, string>();
                foreach (
                    KeyValuePair<NewsNotificationEventHandler, string> notificationCallback in
                    CallbackDictionary[senpai])
                {
                    if (notificationCallback.Key == null) continue;
                    notificationCallback.Key?.Invoke(senpai,
                        lNotifications.TakeWhile(
                            notification => notification.NotificationId != notificationCallback.Value));
                    lChangeDictionary.Add(notificationCallback.Key, lNotifications.First().NotificationId);
                }
                foreach (KeyValuePair<NewsNotificationEventHandler, string> change in lChangeDictionary)
                    if (CallbackDictionary[senpai].ContainsKey(change.Key))
                        CallbackDictionary[senpai][change.Key] = change.Value;
            }
            Timer.Start();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static async Task<ProxerResult<int>> GetAvailableNotificationsCount(Senpai senpai)
        {
            ProxerResult<string> lResult =
                await
                    ApiInfo.HttpClient.GetRequest(new Uri("https://proxer.me/notifications?format=raw&s=count"),
                        senpai);

            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<int>(lResult.Exceptions);

            string lResponse = lResult.Result;

            if (lResponse.StartsWith("1")) return new ProxerResult<int>(new Exception[0]);
            try
            {
                string[] lResponseSplit = lResponse.Split('#');
                return lResponseSplit.Length < 6
                    ? new ProxerResult<int>(new Exception[] {new WrongResponseException {Response = lResponse}})
                    : new ProxerResult<int>(Convert.ToInt32(lResponseSplit[5]));
            }
            catch
            {
                return new ProxerResult<int>(ErrorHandler.HandleError(lResponse, false).Exceptions);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static INotificationCollection<NewsNotification> GetNotifications(Senpai senpai)
        {
            return new NewsNotificationCollection(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="eventHandler"></param>
        public static void RegisterNotificationCallback(Senpai senpai, NewsNotificationEventHandler eventHandler)
        {
            if (CallbackDictionary.ContainsKey(senpai) && !CallbackDictionary[senpai].ContainsKey(eventHandler))
                CallbackDictionary[senpai].Add(eventHandler, "-1");
            else
                CallbackDictionary.Add(senpai,
                    new Dictionary<NewsNotificationEventHandler, string> {{eventHandler, "-1"}});
            CheckNotifications();
        }

        #endregion
    }
}