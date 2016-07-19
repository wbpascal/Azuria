using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Web;

namespace Azuria.Notifications.FriendRequest
{
    /// <summary>
    /// </summary>
    public static class FriendRequestNotificationManager
    {
        /// <summary>
        ///     Represents a method that is executed when new friend request notifications are available.
        /// </summary>
        /// <param name="sender">The user that recieved the notifications.</param>
        /// <param name="e">The notifications.</param>
        public delegate void FriendRequestNotificationEventHandler(
            Senpai sender, IEnumerable<FriendRequestNotification> e);

        private static readonly Dictionary<Senpai, List<FriendRequestNotificationEventHandler>> CallbackDictionary =
            new Dictionary<Senpai, List<FriendRequestNotificationEventHandler>>();

        private static readonly double TimerDelay = TimeSpan.FromMinutes(15).TotalMilliseconds;

        private static readonly Timer Timer = new Timer(TimerDelay)
        {
            AutoReset = true
        };


        static FriendRequestNotificationManager()
        {
            Timer.Elapsed += (sender, args) => CheckNotifications();
            Timer.Enabled = true;
        }

        #region

        private static async void CheckNotifications()
        {
            Timer.Stop();
            foreach (Senpai senpai in CallbackDictionary.Keys)
            {
                ProxerResult<int> lNotificationCountResult = await GetAvailableNotificationsCount(senpai);
                if (!lNotificationCountResult.Success || lNotificationCountResult.Result == 0) continue;
                FriendRequestNotification[] lNotifications =
                    new FriendRequestNotificationCollection(senpai).Take(lNotificationCountResult.Result).ToArray();
                foreach (FriendRequestNotificationEventHandler notificationCallback in CallbackDictionary[senpai])
                {
                    notificationCallback?.Invoke(senpai, lNotifications);
                }
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
                    HttpUtility.GetResponseErrorHandling(new Uri("https://proxer.me/notifications?format=raw&s=count"),
                        senpai);

            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult<int>(lResult.Exceptions);

            string lResponse = lResult.Result;

            if (lResponse.StartsWith("1")) return new ProxerResult<int>(new Exception[0]);
            try
            {
                string[] lResponseSplit = lResponse.Split('#');
                return lResponseSplit.Length < 6
                    ? new ProxerResult<int>(new Exception[] {new WrongResponseException {Response = lResponse}})
                    : new ProxerResult<int>(Convert.ToInt32(lResponseSplit[4]));
            }
            catch
            {
                return new ProxerResult<int>(ErrorHandler.HandleError(senpai, lResponse, false).Exceptions);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static INotificationCollection<FriendRequestNotification> GetNotifications(Senpai senpai)
        {
            return new FriendRequestNotificationCollection(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="eventHandler"></param>
        public static void RegisterNotificationCallback(Senpai senpai,
            FriendRequestNotificationEventHandler eventHandler)
        {
            if (CallbackDictionary.ContainsKey(senpai) && !CallbackDictionary[senpai].Contains(eventHandler))
                CallbackDictionary[senpai].Add(eventHandler);
            else CallbackDictionary.Add(senpai, new List<FriendRequestNotificationEventHandler>(new[] {eventHandler}));
            CheckNotifications();
        }

        #endregion
    }
}