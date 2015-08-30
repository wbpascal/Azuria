using System.Collections.Generic;
using Proxer.API.Notifications;

namespace Proxer.API.EventArguments
{
    /// <summary>
    /// </summary>
    public class PmNotificationEventArgs : INotificationEventArgs
    {
        private readonly Senpai _senpai;

        /// <summary>
        /// </summary>
        /// <param name="count"></param>
        /// <param name="senpai"></param>
        internal PmNotificationEventArgs(int count, Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationEventArgsType.PrivateMessage;
            this.NotificationCount = count;
        }

        /// <summary>
        /// </summary>
        public List<PmObject> Benchrichtigungen
        {
            get { return this._senpai.PrivateMessages; }
        }

        /// <summary>
        /// </summary>
        public int NotificationCount { get; private set; }

        /// <summary>
        /// </summary>
        public NotificationEventArgsType Type { get; private set; }
    }
}