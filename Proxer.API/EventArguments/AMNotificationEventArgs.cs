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
    public class AMNotificationEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        public AMNotificationEventArgs(AnimeMangaNotification notification)
        {
            this.Notification = notification;
        }

        /// <summary>
        /// 
        /// </summary>
        public AnimeMangaNotification Notification { get; private set; }
    }
}
