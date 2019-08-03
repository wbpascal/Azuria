using Azuria.Enums.Info;
using Azuria.Helpers.Extensions;

namespace Azuria.Api.v1.Input.Converter
{
    internal class ToShortStringConverter : IInputDataConverter<Language?>, IInputDataConverter<Country?>
    {
        /// <inheritdoc />
        public string Convert(Language? toConvert)
        {
            return toConvert?.ToShortString();
        }

        /// <inheritdoc />
        public string Convert(Country? toConvert)
        {
            return toConvert?.ToShortString();
        }
    }
}