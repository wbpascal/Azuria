using Azuria.Api.v1.Input.Converter;
using Azuria.Enums;
using Azuria.Enums.List;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.List
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class TagListInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets a string which all returned tags should contain in either their name or description.
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("search", Optional = true)]
        public string Search { get; set; }

        /// <summary>
        /// Gets or sets the category the results should be sorted by.
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("sort", Converter = typeof(ToLowerConverter), Optional = true)]
        public TagListSort? Sort { get; set; }

        /// <summary>
        /// Gets or sets the direction in which the results should be sorted.
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("sort_type", Converter = typeof(GetDescriptionConverter), Optional = true)]
        public SortDirection? SortDirection { get; set; }

        /// <summary>
        /// Gets or sets the subtype of the returned tags.
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("subtype", Converter = typeof(GetDescriptionConverter), Optional = true)]
        public TagSubtype? Subtype { get; set; }

        /// <summary>
        /// Gets or sets the type of the returned tags.
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("type", Converter = typeof(GetDescriptionConverter), Optional = true)]
        public TagType? Type { get; set; }
    }
}