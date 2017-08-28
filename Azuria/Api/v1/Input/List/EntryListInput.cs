using Azuria.Api.v1.Input.Converter;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enums;
using Azuria.Enums.List;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.List
{
    /// <summary>
    /// Represents the input of the <see cref="ListRequestBuilder.GetEntryList" /> method.
    /// </summary>
    public class EntryListInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the category of which entries are returned. Default: <see cref="MediaEntryType.Anime" />
        /// </summary>
        [InputData("kat", Converter = typeof(ToLowerConverter), Optional = true)]
        public MediaEntryType Category { get; set; } = MediaEntryType.Anime;

        /// <summary>
        /// Gets or sets the medium which entries are filter by. The filter will not be applied if the value is
        /// left default (or null). Default: null
        /// </summary>
        [InputData(
            "medium", Converter = typeof(ToLowerConverter), ForbiddenValues = new object[] {MediaMedium.None},
            Optional = true)]
        public MediaMedium? Medium { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating if adult content is included in the result.
        /// </summary>
        [InputData("isH", Converter = typeof(ToLowerConverter), Optional = true)]
        public bool ShowHContent { get; set; }

        /// <summary>
        /// Gets or sets the properties the returned entries will be sorted by. Default: <see cref="EntryListSort.Title" />
        /// </summary>
        [InputData("sort", Converter = typeof(ToLowerConverter), Optional = true)]
        public EntryListSort SortBy { get; set; } = EntryListSort.Title;

        /// <summary>
        /// Gets or sets the direction the returned entries will be sorted by.
        /// Default: <see cref="Enums.SortDirection.Ascending" />
        /// </summary>
        [InputData("sort_type", Converter = typeof(GetDescriptionConverter), Optional = true)]
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

        /// <summary>
        /// Gets or sets the string that all returned entries will start with.
        /// </summary>
        [InputData("start", ConverterMethodName = nameof(GetStartString), Optional = true)]
        public string StartWith { get; set; }

        /// <summary>
        /// Gets or sets a value whether only entries will be returned if they start with non alphabetic
        /// characters (only numericals).
        /// </summary>
        public bool StartWithNonAlphabeticalChar { get; set; }

        internal string GetStartString(string startWith)
        {
            return this.StartWithNonAlphabeticalChar ? "nonAlpha" : this.StartWith;
        }
    }
}