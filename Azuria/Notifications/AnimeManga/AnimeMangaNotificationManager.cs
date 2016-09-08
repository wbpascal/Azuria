using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Azuria.AnimeManga;
using Azuria.Api;
using Azuria.ErrorHandling;
using Azuria.Exceptions;

namespace Azuria.Notifications.AnimeManga
{
    /// <summary>
    /// </summary>
    public static class AnimeMangaNotificationManager
    {
        private static readonly Dictionary<Senpai, List<AnimeNotificationEventHandler>> AnimeCallbackDictionary =
            new Dictionary<Senpai, List<AnimeNotificationEventHandler>>();

        private static readonly Dictionary<Senpai, List<MangaNotificationEventHandler>> MangaCallbackDictionary =
            new Dictionary<Senpai, List<MangaNotificationEventHandler>>();

        private static readonly double TimerDelay = TimeSpan.FromMinutes(15).TotalMilliseconds;

        private static readonly Timer Timer = new Timer(TimerDelay)
        {
            AutoReset = true
        };


        static AnimeMangaNotificationManager()
        {
            Timer.Elapsed += (sender, args) => CheckNotifications();
            Timer.Enabled = true;
        }

        #region Events

        /// <summary>
        /// </summary>
        /// <param name="sender">The user that recieved the notifications.</param>
        /// <param name="e">The notifications.</param>
        public delegate void AnimeNotificationEventHandler(
            Senpai sender, IEnumerable<AnimeMangaNotification<Anime>> e);

        /// <summary>
        /// </summary>
        /// <param name="sender">The user that recieved the notifications.</param>
        /// <param name="e">The notifications.</param>
        public delegate void MangaNotificationEventHandler(
            Senpai sender, IEnumerable<AnimeMangaNotification<Manga>> e);

        #endregion

        #region Methods

        private static async void CheckNotifications()
        {
            Timer.Stop();
            foreach (Senpai senpai in AnimeCallbackDictionary.Keys)
            {
                ProxerResult<int> lNotificationCountResult = await GetAvailableNotificationsCount(senpai);
                if (!lNotificationCountResult.Success || (lNotificationCountResult.Result == 0)) continue;
                AnimeMangaNotification<Anime>[] lNotifications =
                    new AnimeMangaNotificationCollection<Anime>(senpai, lNotificationCountResult.Result).ToArray();
                if (!lNotifications.Any()) continue;
                foreach (AnimeNotificationEventHandler notificationCallback in AnimeCallbackDictionary[senpai])
                    notificationCallback?.Invoke(senpai, lNotifications);
            }
            foreach (Senpai senpai in MangaCallbackDictionary.Keys)
            {
                ProxerResult<int> lNotificationCountResult = await GetAvailableNotificationsCount(senpai);
                if (!lNotificationCountResult.Success || (lNotificationCountResult.Result == 0)) continue;
                AnimeMangaNotification<Manga>[] lNotifications =
                    new AnimeMangaNotificationCollection<Manga>(senpai, lNotificationCountResult.Result).ToArray();
                if (!lNotifications.Any()) continue;
                foreach (MangaNotificationEventHandler notificationCallback in MangaCallbackDictionary[senpai])
                    notificationCallback?.Invoke(senpai, lNotifications);
            }
            Timer.Start();
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="notification"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static async Task<ProxerResult> DeleteNotification<T>(this AnimeMangaNotification<T> notification,
            Senpai senpai) where T : class, IAnimeMangaObject
        {
            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"id", notification.NotificationId.ToString()}
            };

            ProxerResult<string> lResult =
                await
                    ApiInfo.HttpClient.PostRequest(
                        new Uri("https://proxer.me/notifications?format=json&s=deleteNotification"), lPostArgs, senpai);

            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult(lResult.Exceptions);

            return new ProxerResult {Success = lResult.Result.Contains("\"error\":0")};
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static INotificationCollection<AnimeMangaNotification<Anime>> GetAnimeNotifications(Senpai senpai)
        {
            return new AnimeMangaNotificationCollection<Anime>(senpai);
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
                    : new ProxerResult<int>(Convert.ToInt32(lResponseSplit[6]));
            }
            catch
            {
                return new ProxerResult<int>(ErrorHandler.HandleError(lResponse, false).Exceptions);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static INotificationCollection<AnimeMangaNotification<Manga>> GetMangaNotifications(Senpai senpai)
        {
            return new AnimeMangaNotificationCollection<Manga>(senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="eventHandler"></param>
        public static void RegisterAnimeNotificationCallback(Senpai senpai, AnimeNotificationEventHandler eventHandler)
        {
            if (AnimeCallbackDictionary.ContainsKey(senpai) && !AnimeCallbackDictionary[senpai].Contains(eventHandler))
                AnimeCallbackDictionary[senpai].Add(eventHandler);
            else AnimeCallbackDictionary.Add(senpai, new List<AnimeNotificationEventHandler>(new[] {eventHandler}));
            CheckNotifications();
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="eventHandler"></param>
        public static void RegisterMangaNotificationCallback(Senpai senpai, MangaNotificationEventHandler eventHandler)
        {
            if (MangaCallbackDictionary.ContainsKey(senpai) && !MangaCallbackDictionary[senpai].Contains(eventHandler))
                MangaCallbackDictionary[senpai].Add(eventHandler);
            else MangaCallbackDictionary.Add(senpai, new List<MangaNotificationEventHandler>(new[] {eventHandler}));
            CheckNotifications();
        }

        #endregion
    }
}