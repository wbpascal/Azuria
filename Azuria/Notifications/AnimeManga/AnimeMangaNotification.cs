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
        internal AnimeMangaNotification(int notificationId, T animeMangaObject, int contentIndex,
            AnimeMangaLanguage language, DateTime timeStamp, Senpai senpai)
        {
            this.AnimeMangaObject = animeMangaObject;
            this.ContentIndex = contentIndex;
            this.Language = language;
            this.NotificationId = notificationId;
            this.Senpai = senpai;
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

        /// <inheritdoc />
        public Senpai Senpai { get; }

        /// <summary>
        /// </summary>
        public DateTime TimeStamp { get; set; }

        #endregion
    }
}