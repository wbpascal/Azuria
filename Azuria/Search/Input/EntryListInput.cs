namespace Azuria.Search.Input
{
    /// <summary>
    /// </summary>
    public class EntryListInput
    {
        #region Properties

        /// <summary>
        /// </summary>
        public MediaMedium Medium { get; set; } = MediaMedium.None;

        /// <summary>
        /// </summary>
        public bool ShowHContent { get; set; }

        /// <summary>
        /// </summary>
        public string StartWith { get; set; }

        /// <summary>
        /// </summary>
        public bool StartWithNonAlphabeticalChar { get; set; }

        #endregion
    }
}