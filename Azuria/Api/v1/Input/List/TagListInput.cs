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
        /// 
        /// </summary>
        [InputData("search", Optional = true)]
        public string Search { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [InputData("sort", Converter = typeof(ToLowerConverter), Optional = true)]
        public TagListSort Sort { get; set; } = TagListSort.Tag;
        
        /// <summary>
        /// 
        /// </summary>
        [InputData("sort_type", Converter = typeof(GetDescriptionConverter), Optional = true)]
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

        /// <summary>
        /// 
        /// </summary>
        [InputData("subtype", Converter = typeof(GetDescriptionConverter), Optional = true)]
        public TagSubtype? Subtype { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [InputData("type", Converter = typeof(GetDescriptionConverter), Optional = true)]
        public TagType? Type { get; set; }
    }
}