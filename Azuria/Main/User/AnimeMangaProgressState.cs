namespace Azuria.Main.User
{
    /// <summary>
    ///     Eine Aufzählung, die die Kategorie darstellt, in der der <see cref="Azuria.User">Benutzer</see> seinen Fortschritt
    ///     mit
    ///     dem aktuellen
    ///     <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see> eingeordnet hat.
    /// </summary>
    public enum AnimeMangaProgressState
    {
        /// <summary>
        ///     Der <see cref="Azuria.User">Benutzer</see> hat den <see cref="Anime">Anime</see>
        ///     oder <see cref="Manga">Manga</see> als bereits geschaut markiert.
        /// </summary>
        Finished,

        /// <summary>
        ///     Der <see cref="Azuria.User">Benutzer</see> schaut den <see cref="Anime">Anime</see>
        ///     oder <see cref="Manga">Manga</see> momentan.
        /// </summary>
        InProgress,

        /// <summary>
        ///     Der <see cref="Azuria.User">Benutzer</see> hat den <see cref="Anime">Anime</see>
        ///     oder <see cref="Manga">Manga</see> markiert, dass er ihn noch sehen wird.
        /// </summary>
        Planned,

        /// <summary>
        ///     Der <see cref="Azuria.User">Benutzer</see> hat den <see cref="Anime">Anime</see>
        ///     oder <see cref="Manga">Manga</see> als abgebrochen markiert.
        /// </summary>
        Aborted
    }
}