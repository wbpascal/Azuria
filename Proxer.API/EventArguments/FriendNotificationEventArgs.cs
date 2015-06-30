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
    public class FriendNotificationEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        public FriendNotificationEventArgs(FriendNotification notification)
        {
            this.Notification = notification;
        }

        /// <summary>
        /// 
        /// </summary>
        public FriendNotification Notification { get; private set; }
    }
}
