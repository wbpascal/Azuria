using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxer.API.EventArgs
{
    public class NotificationEventArgs
    {
        public NotificationEventArgs(INotification notification)
        {
            this.Notification = notification;
        }

        public INotification Notification { get; private set; }
    }
}
