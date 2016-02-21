namespace Azuria.Main.Minor
{
    /// <summary>
    ///     Eine Enumeration, die die freiwillige Selbstkontrolle eines <see cref="Anime">Anime</see> oder
    ///     <see cref="Manga">Manga</see> darstellt.
    /// </summary>
    public enum Fsk
    {
        /// <summary>
        ///     Der <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see> ist für alle Altersgruppen geeignet.
        /// </summary>
        Fsk0,

        /// <summary>
        ///     Der <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see> ist für Personen ab 6 Jahren geeignet.
        /// </summary>
        Fsk6,

        /// <summary>
        ///     Der <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see> ist für Personen ab 12 Jahren geeignet.
        /// </summary>
        Fsk12,

        /// <summary>
        ///     Der <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see> ist für Personen ab 16 Jahren geeignet.
        /// </summary>
        Fsk16,

        /// <summary>
        ///     Der <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see> ist für Personen ab 18 Jahren geeignet.
        /// </summary>
        Fsk18,

        /// <summary>
        ///     Der <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see> enthält Kraftausdrücke.
        /// </summary>
        BadWords,

        /// <summary>
        ///     Der <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see> enthält Gewalt.
        /// </summary>
        Violence,

        /// <summary>
        ///     Der <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see> könnte zu Furcht bei einigen Personen führen.
        /// </summary>
        Fear,

        /// <summary>
        ///     Der <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see> enthält Geschlechtsverkehr.
        /// </summary>
        Sex
    }
}