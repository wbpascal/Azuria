using Azuria.Api.v1.RequestBuilder;
using Azuria.Enums;
using Azuria.Enums.List;

namespace Azuria.Api.v1.Input
{
    /// <summary>
    /// Represents the input of the <see cref="ListRequestBuilder.GetEntryList"/> method.
    /// </summary>
    public class EntryListInput
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public MediaEntryType Category { get; set; } = MediaEntryType.Anime;

        /// <summary>
        /// </summary>
        public MediaMedium? Medium { get; set; } = null;

        /// <summary>
        /// </summary>
        public bool ShowHContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EntryListSort SortBy { get; set; } = EntryListSort.Title;

        /// <summary>
        /// 
        /// </summary>
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

        /// <summary>
        /// </summary>
        public string StartWith { get; set; }

        /// <summary>
        /// </summary>
        public bool StartWithNonAlphabeticalChar { get; set; }

        #endregion
    }
}