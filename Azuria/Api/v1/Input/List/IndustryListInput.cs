using System;
using Azuria.Api.v1.Input.Converter;
using Azuria.Enums.Info;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.List
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class IndustryListInput : InputDataModel
    {
        /// <summary>
        /// 
        /// </summary>
        [InputData("contains", ForbiddenValues = new object[] {""}, Optional = true)]
        public string Contains { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [InputData("country", ConverterMethodName = nameof(GetCountryString), Optional = true)]
        public Country? Country { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [InputData("start", ForbiddenValues = new object[] {""}, Optional = true)]
        public string StartsWith { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [InputData("type", Converter = typeof(ToTypeStringConverter), Optional = true)]
        public IndustryType? Type { get; set; }

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