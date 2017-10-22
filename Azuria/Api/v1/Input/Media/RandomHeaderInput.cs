using Azuria.Api.v1.Input.Converter;
using Azuria.Enums.Media;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Media
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RandomHeaderInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the style of the returned header.
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("style", Converter = typeof(ToTypeStringConverter), Optional = true)]
        public HeaderStyle? HeaderStyle { get; set; }
    }
}