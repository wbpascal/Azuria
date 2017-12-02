using System;
using Azuria.Api.v1.Input.Converter;
using Azuria.Enums.Info;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.List
{
    /// <summary>
    /// </summary>
    public sealed class IndustryProjectsInput : PagedInputDataModel
    {
        /// <summary>
        /// Gets or sets the id of the industry of which the projects should be returned.
        /// </summary>
        [InputData("id")]
        public int IndustryId { get; set; }

        /// <summary>
        /// Gets or sets whether only H-content should be included in the results. If null both H and non-H content will be included.
        /// </summary>
        [InputData("isH", Converter = typeof(IsHConverter))]
        public bool? IsH { get; set; } = false;

        /// <summary>
        /// Gets or sets a value which describes the role which the industry should have had for the entry to be returned.
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// 
        /// **Example:** If this property is set to <see cref="IndustryRole.Publisher"/> then only entries will be returned where this industry worked as a publisher.
        /// </summary>
        [InputData("type", Converter = typeof(ToTypeStringConverter), Optional = true)]
        public IndustryRole? Role { get; set; }
    }
}