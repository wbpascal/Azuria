using System.ComponentModel;

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
        [Description("fsk0")] Fsk0,

        /// <summary>
        /// The anime or manga is only suitable for persons of age 6 or older.
        /// </summary>
        [Description("fsk6")] Fsk6,

        /// <summary>
        /// The anime or manga is only suitable for persons of age 12 or older.
        /// </summary>
        [Description("fsk12")] Fsk12,

        /// <summary>
        /// The anime or manga is only suitable for persons of age 16 or older.
        /// </summary>
        [Description("fsk16")] Fsk16,

        /// <summary>
        /// The anime or manga is only suitable for persons of age 18 or older.
        /// </summary>
        [Description("fsk18")] Fsk18,

        /// <summary>
        /// The anime or manga contains bad language.
        /// </summary>
        [Description("bad_language")] BadLanguage,

        /// <summary>
        /// The anime or manga contains violence.
        /// </summary>
        [Description("violence")] Violence,

        /// <summary>
        /// The anime or manga could invoke feelings of fear.
        /// </summary>
        [Description("fear")] Fear,

        /// <summary>
        /// The anime or manga contains sexually explicit content.
        /// </summary>
        [Description("sex")] Sex,

        /// <summary>
        /// The age group of the anime or manga is unknown.
        /// </summary>
        [Description("unkown")] Unknown
    }
}