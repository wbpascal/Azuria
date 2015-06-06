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
    public interface INotificationObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string getObjectType();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string getMessage();
    }
}
