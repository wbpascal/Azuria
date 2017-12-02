using System;
using Azuria.Api.v1.Input.Converter;
using Azuria.Enums.Info;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.List
{
    /// <summary>
    /// </summary>
    public sealed class IndustryListInput : PagedInputDataModel
    {
        /// <summary>
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("contains", Optional = true)]
        public string Contains { get; set; }

        /// <summary>
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("country", ConverterMethodName = nameof(GetCountryString), Optional = true)]
        public Country? Country { get; set; }

        /// <summary>
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("start", Optional = true)]
        public string StartsWith { get; set; }

        /// <summary>
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("type", Converter = typeof(ToTypeStringConverter), Optional = true)]
        public IndustryRole? Role { get; set; }

        private string GetCountryString(Country? country)
        {
            if (country == Enums.Info.Country.England)
                throw new InvalidOperationException(
                    "Country.England is not supported in this request! Please use Country.UnitedStates instead!"
                );
            return new ToShortStringConverter().Convert(country);
        }
    }
}