using Proxer.API.Notifications.NotificationObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxer.API.Notifications
{
    /// <summary>
    /// 
    /// </summary>
    public class PMNotification : INotification
    {
        private readonly Senpai senpai;
        private PMObject[] updateObjects;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateCount"></param>
        /// <param name="senpai"></param>
        public PMNotification(int updateCount, Senpai senpai)
        {
            this.Typ = NotificationType.PrivateMessage;
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
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<INotificationObject[]> getUpdates()
        {
            if (updateObjects == null && senpai.LoggedIn)
            {



                return null;
            }
            else
            {
                //kann auch null sein
                return updateObjects;
            }
        }
    }
}
