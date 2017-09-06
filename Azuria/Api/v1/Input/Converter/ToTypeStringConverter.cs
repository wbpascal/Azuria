using Azuria.Enums.Info;
using Azuria.Helpers.Extensions;

namespace Azuria.Api.v1.Input.Converter
{
    internal class ToTypeStringConverter : IInputDataConverter<IndustryType?>
    {
        /// <inheritdoc />
        public string Convert(IndustryType? toConvert)
        {
            return toConvert?.ToTypeString();
        }
    }
}