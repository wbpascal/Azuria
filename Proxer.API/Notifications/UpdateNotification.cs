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
            this.Typ = "Update";
            this.Count = updateCount;
            this.cookies = cookies;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Typ { get; private set; }

        /// <summary>
        /// Gibt die Updates der Benachrichtigungen in einem Array zurück
        /// </summary>
        /// <returns>UpdateObject-Array</returns>
        public async Task<INotificationObject[]> getUpdates()
        {
            UpdateObject[] lReturn = new UpdateObject[this.Count];
            string[] updateRaw = Utility.Utility.GetTagContents(await HttpUtility.GetWebRequestResponseAsync("", cookies), "<a class=\"notificationList\"", "</a>").ToArray();

            for (int i = 0; i < this.Count; i++)
            {
                lReturn[i] = new UpdateObject(Utility.Utility.GetTagContents(updateRaw[i], "<u>", "</u>")[0]);
            }

            return lReturn;
        }
    }
}
