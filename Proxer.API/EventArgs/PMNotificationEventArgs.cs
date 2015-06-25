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
    public class PMNotificationEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        public PMNotificationEventArgs(PMNotification notification)
        {
            this.Notification = notification;
        }

        /// <summary>
        /// 
        /// </summary>
        public PMNotification Notification { get; private set; }
    }
}
