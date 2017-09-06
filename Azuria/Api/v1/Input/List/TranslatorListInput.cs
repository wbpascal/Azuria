using Azuria.Api.v1.Input.Converter;
using Azuria.Enums.Info;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.List
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class TranslatorListInput : InputDataModel
    {
        /// <summary>
        /// 
        /// </summary>
        [InputData("contains", Optional = true)]
        public string Contains { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [InputData(
            "country", Converter = typeof(ToShortStringConverter),
            ForbiddenValues = new object[] {Enums.Info.Country.Japan, Enums.Info.Country.UnitedStates},
            Optional = true)]
        public Country? Country { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [InputData("start", Optional = true)]
        public string StartsWith { get; set; }
    }
}