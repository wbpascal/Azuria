using Proxer.API.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proxer.API.EventArguments
{
    /// <summary>
    /// 
    /// </summary>
    public interface INotificationEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        int NotificationCount { get; }

        /// <summary>
        /// 
        /// </summary>
        NotificationEventArgsType Type { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum NotificationEventArgsType
    {
        /// <summary>
        /// 
        /// </summary>
        AnimeManga,
        /// <summary>
        /// 
        /// </summary>
        Friend,
        /// <summary>
        /// 
        /// </summary>
        News,
        /// <summary>
        /// 
        /// </summary>
        PrivateMessage
    }
}
