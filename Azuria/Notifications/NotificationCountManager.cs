using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Notifications;

namespace Azuria.Notifications
{
    internal static class NotificationCountManager
    {
        private static DateTime _lastTimeNotificationChecked = DateTime.MinValue;

        private static readonly Dictionary<Senpai, List<INotificationManager>> NotificationManagers =
            new Dictionary<Senpai, List<INotificationManager>>();

        private static readonly double TimerDelay = TimeSpan.FromMinutes(15).TotalMilliseconds;

        private static readonly Timer Timer = new Timer(TimerDelay)
        {
            AutoReset = true
        };


        static NotificationCountManager()
        {
            Timer.Elapsed += async (sender, args) => await CheckNotifications().ConfigureAwait(false);
            Timer.Enabled = true;
        }

        #region Methods

        private static async Task CheckNotifications()
        {
            _lastTimeNotificationChecked = DateTime.Now;
            foreach (KeyValuePair<Senpai, List<INotificationManager>> notificationManager in NotificationManagers)
            {
                ProxerApiResponse<NotificationCountDataModel> lResult = await RequestHandler.ApiRequest(
                    ApiRequestBuilder.NotificationGetCount(notificationManager.Key));
                if (!lResult.Success || (lResult.Result == null)) continue;
                foreach (INotificationManager manager in notificationManager.Value)
                    manager.OnNewNotificationsAvailable(lResult.Result);
            }
        }

        internal static async Task CheckNotificationsForNewEvent()
        {
            if (DateTime.Now.Subtract(_lastTimeNotificationChecked) < TimeSpan.FromMinutes(1)) return;
            await CheckNotifications();
        }

        internal static T GetOrAddManager<T>(Senpai senpai, T newInstance) where T : class, INotificationManager
        {
            if (NotificationManagers.ContainsKey(senpai) &&
                NotificationManagers[senpai].Any(manager => manager.GetType() == typeof(T)))
                return NotificationManagers[senpai].Find(manager => manager.GetType() == typeof(T)) as T;
            if (NotificationManagers.ContainsKey(senpai))
            {
                NotificationManagers[senpai].Add(newInstance);
                return newInstance;
            }

            NotificationManagers.Add(senpai, new List<INotificationManager> {newInstance});
            return newInstance;
        }

        #endregion
    }
}