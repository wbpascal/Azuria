namespace Azuria.Enums.Info
{
    /// <summary>
    /// Represents an enumeration which aims to help categorizing an anime or
    /// manga for certain age groups. ("Freiwillige Selbstkontrolle")
    /// </summary>
    public enum Fsk
    {
        /// <summary>
        /// The anime or manga is suitable for all age groups.
        /// </summary>
        Fsk0,

        /// <summary>
        /// The anime or manga is only suitable for persons of age 6 or older.
        /// </summary>
        Fsk6,

        /// <summary>
        /// The anime or manga is only suitable for persons of age 12 or older.
        /// </summary>
        Fsk12,

        /// <summary>
        /// The anime or manga is only suitable for persons of age 16 or older.
        /// </summary>
        Fsk16,

        /// <summary>
        /// The anime or manga is only suitable for persons of age 18 or older.
        /// </summary>
        Fsk18,

        /// <summary>
        /// The anime or manga contains violent language.
        /// </summary>
        BadWords,

        /// <summary>
        /// The anime or manga contains violence.
        /// </summary>
        Violence,

        /// <summary>
        /// The anime or manga could invoke feelings of fear for some persons.
        /// </summary>
        Fear,

        /// <summary>
        /// The anime or manga contains sexually explicit content.
        /// </summary>
        Sex,

        /// <summary>
        /// The age group of the anime or manga is unknown.
        /// </summary>
        Unknown
    }
}