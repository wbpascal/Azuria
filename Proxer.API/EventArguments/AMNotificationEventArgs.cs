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
    public class AMNotificationEventArgs : INotificationEventArgs
    {
        private readonly Senpai senpai;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="senpai"></param>
        internal AMNotificationEventArgs(int count, Senpai senpai)
        {
            this.senpai = senpai;
            this.Type = NotificationEventArgsType.AnimeManga;
            this.NotificationCount = count;
        }

        /// <summary>
        /// 
        /// </summary>
        public int NotificationCount { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public NotificationEventArgsType Type { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public List<AnimeMangaUpdateObject> Benachrichtigungen
        {
            get
            {
                return this.senpai.AnimeMangaUpdates;
            }
        }
    }
}
