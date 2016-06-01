using Azuria.Main;
using JetBrains.Annotations;

namespace Azuria.Notifications
{
    /// <summary>
    ///     Represents an <see cref="Anime" />- or <see cref="Manga" />-notification.
    /// </summary>
    public class AnimeMangaNotification : INotification
    {
        internal AnimeMangaNotification([NotNull] string message)
        {
            this.Type = NotificationType.AnimeManga;
            this.Message = message;
            this.Name = "";
            this.Number = -1;
            this.Id = -1;
        }

        internal AnimeMangaNotification([NotNull] string message, [NotNull] string name, int number, int id)
        {
            this.Type = NotificationType.AnimeManga;
            this.Message = message;
            this.Name = name.Trim();
            this.Number = number;
            this.Id = id;
        }

        #region Geerbt

        /// <summary>
        ///     Gets the message of the notification.
        /// </summary>
        public string Message { get; }

        /// <summary>
        ///     Gets the type of the notification.
        /// </summary>
        public NotificationType Type { get; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the id of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        ///     Gets the title of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        [NotNull]
        public string Name { get; }

        /// <summary>
        ///     Gets the <see cref="Anime.Episode" />- or <see cref="Manga.Chapter" />- number.
        /// </summary>
        public int Number { get; }

        #endregion

        #region

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return this.Name + " #" + this.Number + " ist jetzt online!";
        }

        #endregion
    }
}