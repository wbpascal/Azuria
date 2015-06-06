using Proxer.API.Notifications.NotificationObjects;
using Proxer.API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Proxer.API.Notifications
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateNotification : INotification
    {
        private readonly CookieContainer cookies;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateCount"></param>
        /// <param name="cookies"></param>
        public UpdateNotification(int updateCount, CookieContainer cookies)
        {
            this.Count = updateCount;
            this.cookies = cookies;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count {get; private set; }

        /// <summary>
        /// Gibt die Updates der Benachrichtigungen in einem Array zurück
        /// </summary>
        /// <returns>UpdateObject-Array</returns>
        public async Task<INotificationObject[]> getUpdates()
        {
            string updateRaw = await HttpUtility.GetWebRequestResponseAsync("", cookies);
            return new UpdateObject[Count];
        }
    }
}
