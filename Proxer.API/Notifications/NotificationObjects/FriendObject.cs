using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxer.API.Notifications.NotificationObjects
{
    /// <summary>
    /// 
    /// </summary>
    public class FriendObject : INotificationObject
    {
        /// <summary>
        /// 
        /// </summary>
        public FriendObject()
        {
            this.Typ = "Freund";
        }

        /// <summary>
        /// 
        /// </summary>
        public string Typ { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get { throw new NotImplementedException(); }
        }
    }
}
