﻿using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Utilities.ErrorHandling;

namespace Azuria.Notifications.AnimeManga
{
    /// <summary>
    ///     Represents an <see cref="Anime" />- or <see cref="Manga" />-notification.
    /// </summary>
    public class AnimeMangaNotification<T> : INotification where T : class, IAnimeMangaObject
    {
        private readonly Senpai _senpai;

        internal AnimeMangaNotification(int notificationId, IAnimeMangaContent<T> contentObject, Senpai senpai)
        {
            this._senpai = senpai;
            this.ContentObject = contentObject;
            this.NotificationId = notificationId;
        }

        #region Inherited

        /// <summary>
        ///     Gets the type of the notification.
        /// </summary>
        public NotificationType Type => typeof(T) == typeof(Anime) ? NotificationType.Anime : NotificationType.Manga;

        string INotification.NotificationId => this.NotificationId.ToString();

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        public IAnimeMangaContent<T> ContentObject { get; }

        /// <summary>
        /// </summary>
        public int NotificationId { get; }

        #endregion

        #region

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<ProxerResult> Delete()
        {
            return await this.DeleteNotification(this._senpai);
        }

        #endregion
    }
}