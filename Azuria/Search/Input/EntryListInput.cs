namespace Azuria.Search.Input
{
    /// <summary>
    /// </summary>
    public class EntryListInput
    {
        #region Properties

        /// <summary>
        /// </summary>
        public AnimeMangaMedium Medium { get; set; } = AnimeMangaMedium.None;

        /// <summary>
        /// </summary>
        public bool ShowHContent { get; set; }

        /// <summary>
        /// </summary>
        public bool ShowOnlyNonAlphabeticalBeginnings { get; set; }

        /// <summary>
        /// </summary>
        public string StartWith { get; set; }

        #endregion
    }
}