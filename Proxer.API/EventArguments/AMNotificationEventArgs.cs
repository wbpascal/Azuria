using Proxer.API.Notifications;

namespace Proxer.API.EventArguments
{
    /// <summary>
    /// </summary>
    public class AmNotificationEventArgs : INotificationEventArgs
    {
        private readonly Senpai _senpai;

        /// <summary>
        /// </summary>
        /// <param name="count"></param>
        /// <param name="senpai"></param>
        internal AmNotificationEventArgs(int count, Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationEventArgsType.AnimeManga;
            this.NotificationCount = count;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public AnimeMangaUpdateCollection Benachrichtigungen
        {
            get { return this._senpai.AnimeMangaUpdates; }
        }

        /// <summary>
        /// </summary>
        public int NotificationCount { get; private set; }

        /// <summary>
        /// </summary>
        public NotificationEventArgsType Type { get; private set; }

        #endregion
    }
}