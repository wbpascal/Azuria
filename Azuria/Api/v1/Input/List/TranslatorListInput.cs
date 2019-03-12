using Azuria.Api.v1.Input.Converter;
using Azuria.Enums.Info;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.List
{
    /// <summary>
    /// </summary>
    public class TranslatorListInput : PagedInputDataModel
    {
        /// <summary>
        /// Gets or sets the string the names of the returned translator groups have to contain.
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("contains", Optional = true)]
        public string Contains { get; set; }

        /// <summary>
        /// Gets or sets the country of the returned translator groups.
        /// Optional, if omitted, invalid or null the default value of the api method will be used.
        /// 
        /// Invalid Values:
        /// * <see cref="Enums.Info.Country.Japan"/>
        /// * <see cref="Enums.Info.Country.UnitedStates"/> (use <see cref="Enums.Info.Country.England"/> instead)
        /// </summary>
        [InputData(
            "country", Converter = typeof(ToShortStringConverter),
            ForbiddenValues = new object[] {Enums.Info.Country.Japan, Enums.Info.Country.UnitedStates},
            Optional = true)]
        public Country? Country { get; set; }

        /// <summary>
        /// Gets or sets the string every name of the returned translator groups has to start with.
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("start", Optional = true)]
        public string StartsWith { get; set; }
    }
}