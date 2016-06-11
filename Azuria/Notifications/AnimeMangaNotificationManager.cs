using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Net;

namespace Azuria.Notifications
{
    /// <summary>
    /// </summary>
    public class AnimeMangaNotificationManager
    {
        /// <summary>
        ///     Represents a method that is executed when new private message notifications are available.
        /// </summary>
        /// <param name="sender">The user that recieved the notifications.</param>
        /// <param name="e">The notifications.</param>
        public delegate void AnimeMangaNotificationEventHandler(Senpai sender, IEnumerable<AnimeMangaNotification> e);

        private static readonly double TimerDelay = TimeSpan.FromMinutes(15).TotalMilliseconds;

        private static readonly Timer Timer = new Timer(TimerDelay)
        {
            AutoReset = true
        };

        private static readonly Dictionary<Senpai, List<AnimeMangaNotificationEventHandler>> CallbackDictionary =
            new Dictionary<Senpai, List<AnimeMangaNotificationEventHandler>>();

        static AnimeMangaNotificationManager()
        {
            Timer.Elapsed += (sender, args) => CheckNotifications();
            Timer.Enabled = true;
        }

        #region

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="eventHandler"></param>
        public static void AddEventCallback(Senpai senpai, AnimeMangaNotificationEventHandler eventHandler)
        {
            if (CallbackDictionary.ContainsKey(senpai) && !CallbackDictionary[senpai].Contains(eventHandler))
                CallbackDictionary[senpai].Add(eventHandler);
            else CallbackDictionary.Add(senpai, new List<AnimeMangaNotificationEventHandler>(new[] {eventHandler}));
        }

        private static async void CheckNotifications()
        {
            foreach (Senpai senpai in CallbackDictionary.Keys)
            {
                ProxerResult<int> lNotificationCountResult = await GetAvailableNotificationsCount(senpai);
                if (!lNotificationCountResult.Success || lNotificationCountResult.Result == 0) continue;
                AnimeMangaNotification[] lNotifications =
                    new AnimeMangaNotificationCollection(senpai).Take(lNotificationCountResult.Result).ToArray();
                foreach (AnimeMangaNotificationEventHandler notificationCallback in CallbackDictionary[senpai])
                {
                    notificationCallback?.Invoke(senpai, lNotifications);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static async Task<ProxerResult<int>> GetAvailableNotificationsCount(Senpai senpai)
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(new Uri("https://proxer.me/notifications?format=raw&s=count"),
                        senpai.LoginCookies, senpai);

            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult<int>(lResult.Exceptions);

            string lResponse = lResult.Result;

            if (lResponse.StartsWith("1")) return new ProxerResult<int>(new Exception[0]);
            try
            {
                string[] lResponseSplit = lResponse.Split('#');
                return lResponseSplit.Length < 6
                    ? new ProxerResult<int>(new Exception[] {new WrongResponseException {Response = lResponse}})
                    : new ProxerResult<int>(Convert.ToInt32(lResponseSplit[3]));
            }
            catch
            {
                return new ProxerResult<int>((await ErrorHandler.HandleError(senpai, lResponse, false)).Exceptions);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static INotificationCollection<AnimeMangaNotification> GetNotifications(Senpai senpai)
        {
            return new AnimeMangaNotificationCollection(senpai);
        }

        #endregion
    }
}