using System;
using Azuria.AnimeManga;
using Azuria.AnimeManga.Properties;

namespace Azuria.Notifications.AnimeManga
{
    /// <summary>
    ///     Represents an <see cref="Anime" />- or <see cref="Manga" />-notification.
    /// </summary>
    public class AnimeMangaNotification<T> : INotification where T : IAnimeMangaObject
    {
        private readonly Senpai _senpai;

        internal AnimeMangaNotification(int notificationId, T animeMangaObject, int contentIndex,
            AnimeMangaLanguage language, DateTime timeStamp, Senpai senpai)
        {
            this._senpai = senpai;
            this.AnimeMangaObject = animeMangaObject;
            this.ContentIndex = contentIndex;
            this.Language = language;
            this.NotificationId = notificationId;
            this.TimeStamp = timeStamp;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public T AnimeMangaObject { get; set; }

        /// <summary>
        /// </summary>
        public int ContentIndex { get; set; }

        /// <summary>
        /// </summary>
        public AnimeMangaLanguage Language { get; set; }

        /// <summary>
        /// </summary>
        public int NotificationId { get; }

        #region Implementation of INotification

        string INotification.NotificationId => this.NotificationId.ToString();

        #endregion

        /// <summary>
        /// </summary>
        public DateTime TimeStamp { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public static implicit operator AnimeMangaNotification<T>(AnimeMangaNotification<IAnimeMangaObject> notification
        )
        {
            return new AnimeMangaNotification<T>(notification.NotificationId, (T) notification.AnimeMangaObject,
                notification.ContentIndex, notification.Language, notification.TimeStamp, notification._senpai);
        }

        #endregion
    }
}