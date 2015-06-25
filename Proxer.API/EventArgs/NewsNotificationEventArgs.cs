using Proxer.API.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxer.API.EventArgs
{
    /// <summary>
    /// 
    /// </summary>
    public class NewsNotificationEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        public NewsNotificationEventArgs(NewsNotification notification)
        {
            this.Notification = notification;
        }

        /// <summary>
        /// 
        /// </summary>
        public NewsNotification Notification { get; private set; }
    }
}
