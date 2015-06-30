using Proxer.API.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxer.API.EventArguments
{
    /// <summary>
    /// 
    /// </summary>
    public class NotificationEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        public NotificationEventArgs(INotification notification)
        {
            this.Notification = notification;
        }

        /// <summary>
        /// 
        /// </summary>
        public INotification Notification { get; private set; }
    }
}
