using System;
using Azuria.Api.v1.Input.Converter;
using Azuria.Enums.Info;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.List
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class IndustryProjectsInput : InputDataModel
    {
        /// <summary>
        /// 
        /// </summary>
        [InputData("id", ForbiddenValues = new object[] {int.MinValue})]
        public int IndustryId { get; set; } = int.MinValue;

        /// <summary>
        /// 
        /// </summary>
        [InputData("isH", Converter = typeof(IsHConverter), Optional = true)]
        public bool? IsH { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        [InputData("type", Converter = typeof(ToTypeStringConverter), Optional = true)]
        public IndustryType? Type { get; set; }
    }
}