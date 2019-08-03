using Azuria.Api.v1.Input.Converter;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enums;
using Azuria.Enums.List;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.List
{
    /// <summary>
    /// </summary>
    public class EntryListInput : PagedInputDataModel
    {
        /// <summary>
        /// Gets or sets the category of which entries are returned. 
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("kat", Converter = typeof(ToLowerConverter), Optional = true)]
        public MediaEntryType? Category { get; set; }

        /// <summary>
        /// Gets or sets the medium which entries are filter by.
        /// Optional, if omitted, invalid or null the default value of the api method will be used.
        /// 
        /// Invalid Values:
        /// * <see cref="MediaMedium.None"/>
        /// </summary>
        [InputData(
            "medium", Converter = typeof(ToLowerConverter), ForbiddenValues = new object[] {MediaMedium.None},
            Optional = true)]
        public MediaMedium? Medium { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if adult content is included in the result.
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("isH", Converter = typeof(ToLowerConverter), Optional = true)]
        public bool? ShowHContent { get; set; }

        /// <summary>
        /// Gets or sets the properties the returned entries will be sorted by. Default: <see cref="EntryListSort.Title" />
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("sort", Converter = typeof(ToLowerConverter), Optional = true)]
        public EntryListSort? SortBy { get; set; }

        /// <summary>
        /// Gets or sets the direction the returned entries will be sorted by.
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("sort_type", Converter = typeof(GetDescriptionConverter), Optional = true)]
        public SortDirection? SortDirection { get; set; }

        /// <summary>
        /// Gets or sets the string that all returned entries will start with.
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("start", ConverterMethodName = nameof(GetStartString), Optional = true)]
        public string StartsWith { get; set; }

        /// <summary>
        /// Gets or sets a value whether only entries will be returned if they start with non alphabetic
        /// characters (only numericals). Overrides value of <see cref="StartsWith"/>.
        /// </summary>
        public bool StartWithNonAlphabeticalChar { get; set; }

        internal string GetStartString(string startWith)
        {
            return this.StartWithNonAlphabeticalChar ? "nonAlpha" : this.StartsWith;
        }
    }
}