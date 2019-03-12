using Azuria.Api.v1.Input.Converter;
using Azuria.Enums;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Ucp
{
    /// <summary>
    /// 
    /// </summary>
    public class ListSumInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the category from which the points will be returned. 
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("kat", Converter = typeof(ToLowerConverter), Optional = true)]
        public MediaEntryType? Category { get; set; }
    }
}