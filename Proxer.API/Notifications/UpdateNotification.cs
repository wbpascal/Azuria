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
        private readonly User senpai;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateCount"></param>
        /// <param name="senpai"></param>
        public UpdateNotification(int updateCount, User senpai)
        {
            this.Typ = NotificationType.AnimeManga;
            this.Count = updateCount;
            this.senpai = senpai;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public NotificationType Typ { get; private set; }

        /// <summary>
        /// Gibt die Updates der Benachrichtigungen in einem Array zurück
        /// </summary>
        /// <returns>UpdateObject-Array</returns>
        public async Task<INotificationObject[]> getUpdates()
        {
            //UpdateObject[] lReturn = new UpdateObject[this.Count];
            //if (senpai.LoggedIn)
            //{
            //    string[] updateRaw = Utility.Utility.GetTagContents(await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/components/com_proxer/misc/notifications_misc.php", senpai.LoginCookies), "<a class=\"notificationList\"", "</a>").ToArray();

            //    for (int i = 0; i < this.Count; i++)
            //    {
            //        UpdateObject lUpdate =new UpdateObject(Utility.Utility.GetTagContents(updateRaw[i], "<u>", "</u>")[0]);
            //        lReturn[i] = lUpdate;
            //        senpai.AMUpdates.Add(lUpdate);
            //    }
            //}
            //return lReturn;
      
            throw new NotImplementedException();
        }
    }
}
