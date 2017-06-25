using Azuria.Api.v1.RequestBuilder;
using Azuria.Enums;
using Azuria.Enums.List;

namespace Azuria.Api.v1.Input
{
    /// <summary>
    /// Represents the input of the <see cref="ListRequestBuilder.GetEntryList" /> method.
    /// </summary>
    public class EntryListInput
    {
        /// <summary>
        /// Gets or sets the category of which entries are returned. Default: <see cref="MediaEntryType.Anime" />
        /// </summary>
        public MediaEntryType Category { get; set; } = MediaEntryType.Anime;

        /// <summary>
        /// Gets or sets the medium which entries are filter by. The filter will not be applied if the value is
        /// left default (or null). Default: null
        /// </summary>
        public MediaMedium? Medium { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating if adult content is included in the result.
        /// </summary>
        public bool ShowHContent { get; set; }

        /// <summary>
        /// Gets or sets the properties the returned entries will be sorted by. Default: <see cref="EntryListSort.Title" />
        /// </summary>
        public EntryListSort SortBy { get; set; } = EntryListSort.Title;

        /// <summary>
        /// Gets or sets the direction the returned entries will be sorted by.
        /// Default: <see cref="Enums.SortDirection.Ascending" />
        /// </summary>
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

        /// <summary>
        /// Gets or sets the string that all returned entries will start with.
        /// </summary>
        public string StartWith { get; set; }

        /// <summary>
        /// Gets or sets a value whether only entries will be returned if they start with non alphabetic
        /// characters (only numericals).
        /// </summary>
        public bool StartWithNonAlphabeticalChar { get; set; }
    }
}