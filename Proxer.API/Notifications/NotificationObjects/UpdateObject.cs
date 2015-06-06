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
    public class UpdateObject : INotificationObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public UpdateObject(string message)
        {
            this.Typ = "Update";
            this.Message = message;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Typ { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; private set; }
    }
}
