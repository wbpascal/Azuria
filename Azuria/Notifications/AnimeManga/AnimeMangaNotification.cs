using Azuria.AnimeManga;

namespace Azuria.Notifications.AnimeManga
{
    /// <summary>
    ///     Represents an <see cref="Anime" />- or <see cref="Manga" />-notification.
    /// </summary>
    public class AnimeMangaNotification<T> : INotification where T : IAnimeMangaObject
    {
        private readonly Senpai _senpai;

        internal AnimeMangaNotification(int notificationId, IAnimeMangaContent<T> contentObject, Senpai senpai)
        {
            this._senpai = senpai;
            this.ContentObject = contentObject;
            this.NotificationId = notificationId;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public IAnimeMangaContent<T> ContentObject { get; }

        /// <summary>
        /// </summary>
        public int NotificationId { get; }

        #region Implementation of INotification

        string INotification.NotificationId => this.NotificationId.ToString();

        #endregion

        #endregion
    }
}