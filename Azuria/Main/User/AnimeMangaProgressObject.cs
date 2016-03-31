using JetBrains.Annotations;

namespace Azuria.Main.User
{
    /// <summary>
    ///     Eine Klasse, die den Fortschritt eines <see cref="Azuria.User">Benutzer</see> bei einem
    ///     <see cref="Anime">Anime</see>
    ///     oder <see cref="Manga">Manga</see> darstellt.
    /// </summary>
    public class AnimeMangaProgressObject
    {
        /// <summary>
        ///     Initialisiert das Objekt.
        /// </summary>
        /// <param name="user">Der Benutzer, der mit dem Fortschritt zusammenhängt.</param>
        /// <param name="animeMangaObject">
        ///     Der <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see>, mit dem das Objekt
        ///     zusammenhängt.
        /// </param>
        /// <param name="currentProgress">Der aktuelle Fortschritt.</param>
        /// <param name="maxCount">
        ///     Die maximale Anzahl der <see cref="Anime.Episode">Episoden</see> oder
        ///     <see cref="Manga.Chapter">Kapitel</see>.
        /// </param>
        /// <param name="progress">
        ///     Die Kategorie, in der der <paramref name="user">Benutzer</paramref> seinen Fortschritt
        ///     einsortiert hat.
        /// </param>
        public AnimeMangaProgressObject([NotNull] Azuria.User user, [NotNull] IAnimeMangaObject animeMangaObject,
            int currentProgress,
            int maxCount, AnimeMangaProgress progress)
        {
            this.User = user;
            this.AnimeMangaObject = animeMangaObject;
            this.CurrentProgress = currentProgress;
            this.MaxCount = maxCount;
            this.Progress = progress;
        }

        #region Properties

        /// <summary>
        ///     Gibt den <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see> zurück, mit dem das Objekt zusammenhängt.
        /// </summary>
        [NotNull]
        public IAnimeMangaObject AnimeMangaObject { get; }

        /// <summary>
        ///     Gibt den aktuellen Fortschritt aus oder legt diesen fest.
        /// </summary>
        public int CurrentProgress { get; set; }

        /// <summary>
        ///     Gibt die maximale Anzahl der <see cref="Anime.Episode">Episoden</see> oder <see cref="Manga.Chapter">Kapitel</see>
        ///     zurück,
        ///     die durch das Objekt dargestellt werden.
        /// </summary>
        public int MaxCount { get; }

        /// <summary>
        ///     Gibt die Kategorie, in der der <see cref="User">Benutzer</see> seinen Fortschritt einsortiert hat zurück.
        /// </summary>
        public AnimeMangaProgress Progress { get; set; }

        /// <summary>
        ///     Gibt den <see cref="Azuria.User">Benutzer</see> zurück, mit dem der Fortschritt zusammenhängt.
        /// </summary>
        [NotNull]
        public Azuria.User User { get; set; }

        #endregion
    }
}