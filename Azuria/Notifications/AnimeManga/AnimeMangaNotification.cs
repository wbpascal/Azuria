using Azuria.Main;

namespace Azuria.Notifications.AnimeManga
{
    /// <summary>
    ///     Represents an <see cref="Anime" />- or <see cref="Manga" />-notification.
    /// </summary>
    public class AnimeMangaNotification<T> : INotification where T : class, IAnimeMangaObject
    {
        internal AnimeMangaNotification(IAnimeMangaContent<T> contentObject, Senpai senpai)
        {
            this.ContentObject = contentObject;
        }

        #region Geerbt

        /// <summary>
        ///     Gets the type of the notification.
        /// </summary>
        public NotificationType Type => typeof(T) == typeof(Anime) ? NotificationType.Anime : NotificationType.Manga;

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        public IAnimeMangaContent<T> ContentObject { get; }

        #endregion
    }
}