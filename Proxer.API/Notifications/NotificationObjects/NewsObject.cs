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
    public class NewsObject : INotificationObject
    {
        /// <summary>
        /// 
        /// </summary>
        public NewsObject()
        {
            this.Typ = "News";
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
