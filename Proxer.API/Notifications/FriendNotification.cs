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
    public class FriendNotification : INotification
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateCount"></param>
        public FriendNotification(int updateCount)
        {
            this.Typ = "Freund";
            this.Count = updateCount;
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
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<INotificationObject[]> getUpdates()
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }
    }
}
