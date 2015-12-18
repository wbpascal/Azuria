using System;

namespace Azuria.Notifications
{
    /// <summary>
    ///     Eine Klasse, die eine <see cref="Main.Anime">Anime-</see> oder <see cref="Main.Manga">Manga-</see>Benachrichtigung
    ///     darstellt.
    /// </summary>
    public class AnimeMangaUpdateObject : INotificationObject
    {
        internal AnimeMangaUpdateObject(string message)
        {
            this.Type = NotificationObjectType.AnimeManga;
            this.Message = message;
            this.Name = "";
            this.Number = -1;
            this.Link = null;
            this.Id = -1;
        }

        internal AnimeMangaUpdateObject(string message, string name, int number, Uri link, int id)
        {
            this.Type = NotificationObjectType.AnimeManga;
            this.Message = message;
            this.Name = name.Trim();
            this.Number = number;
            this.Link = link;
            this.Id = id;
        }

        #region Geerbt

        /// <summary>
        ///     Gibt die Nachricht der Benachrichtigung als Text zurück.
        ///     <para>(Vererbt von <see cref="INotificationObject" />)</para>
        /// </summary>
        public string Message { get; }

        /// <summary>
        ///     Gibt den Typ der Benachrichtigung zurück.
        ///     <para>(Vererbt von <see cref="INotificationObject" />)</para>
        /// </summary>
        public NotificationObjectType Type { get; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gibt die Id des <see cref="Main.Anime">Anim-</see> oder <see cref="Main.Manga">Manga</see> zurück.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        ///     Gibt den Link zur <see cref="Main.Anime.Episode">Episode</see> oder zum
        ///     <see cref="Main.Manga.Chapter">Kapitel</see> zurück.
        /// </summary>
        public Uri Link { get; private set; }

        /// <summary>
        ///     Gibt den Namen des <see cref="Main.Anime">Anime</see> oder <see cref="Main.Manga">Manga</see> zurück.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gibt die Nummer der <see cref="Main.Anime.Episode">Episode</see> oder des
        ///     <see cref="Main.Manga.Chapter">Kapitels</see> zurück.
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