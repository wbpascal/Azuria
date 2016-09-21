namespace Azuria.Media.Properties
{
    /// <summary>
    /// Represents an enumeration which aims to help categorizing an <see cref="Anime">Anime</see> or
    /// <see cref="Manga">Manga</see> for certain age groups. ("Freiwillige Selbstkontrolle")
    /// </summary>
    public enum FskType
    {
        /// <summary>
        /// The <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see> is suitable for all age groups.
        /// </summary>
        Fsk0,

        /// <summary>
        /// The <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see> is only suitable for persons of age 6 or older.
        /// </summary>
        Fsk6,

        /// <summary>
        /// The <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see> is only suitable for persons of age 12 or older.
        /// </summary>
        Fsk12,

        /// <summary>
        /// The <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see> is only suitable for persons of age 16 or older.
        /// </summary>
        Fsk16,

        /// <summary>
        /// The <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see> is only suitable for persons of age 18 or older.
        /// </summary>
        Fsk18,

        /// <summary>
        /// The <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see> contains violent language.
        /// </summary>
        BadWords,

        /// <summary>
        /// The <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see> contains violence.
        /// </summary>
        Violence,

        /// <summary>
        /// The <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see> could invoke feelings of fear for some persons.
        /// </summary>
        Fear,

        /// <summary>
        /// The <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see> contains sexually explicit content.
        /// </summary>
        Sex,

        /// <summary>
        /// The age group of the <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see> is unknown.
        /// </summary>
        Unknown
    }
}