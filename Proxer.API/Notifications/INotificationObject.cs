using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxer.API.Notifications
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
        NotificationObjectType Typ { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string Message { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum NotificationObjectType
    {
        /// <summary>
        /// 
        /// </summary>
        FriendRequest,

        /// <summary>
        /// 
        /// </summary>
        News,

        /// <summary>
        /// 
        /// </summary>
        PrivateMessage,

        /// <summary>
        /// 
        /// </summary>
        AnimeManga,

        /// <summary>
        /// 
        /// </summary>
        Dummy
    };
}
