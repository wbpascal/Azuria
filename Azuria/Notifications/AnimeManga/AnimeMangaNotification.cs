using Azuria.Main;
using Azuria.Main.Minor;

namespace Azuria.Notifications.AnimeManga
{
    /// <summary>
    ///     Represents an <see cref="Anime" />- or <see cref="Manga" />-notification.
    /// </summary>
    public class AnimeMangaNotification<T> : INotification where T : IAnimeMangaObject
    {
        internal AnimeMangaNotification(T parentObject, int lIndex, Senpai senpai)
        {
            this.ContentObject = (typeof(T) == typeof(Anime)
                ? new Anime.Episode(new Anime(parentObject.Name.GetObjectIfInitialised(""), parentObject.Id, senpai),
                    lIndex, AnimeLanguage.Unknown, senpai)
                : (IAnimeMangaContentBase)
                    new Manga.Chapter(new Manga(parentObject.Name.GetObjectIfInitialised(""), parentObject.Id, senpai),
                        lIndex, Language.Unkown, senpai)) as IAnimeMangaContent<T>;
        }

        #region Geerbt

        /// <summary>
        ///     Gets the type of the notification.
        /// </summary>
        public NotificationType Type => NotificationType.AnimeManga;

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        public IAnimeMangaContent<T> ContentObject { get; }

        #endregion
    }
}