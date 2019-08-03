using System.Collections;
using Azuria.Helpers.Extensions;

namespace Azuria.Api.v1.Input.Converter
{
    internal class SpaceSeparatedEnumerationConverter : IInputDataConverter<IEnumerable>
    {
        /// <inheritdoc />
        public string Convert(IEnumerable toConvert)
        {
            return toConvert?.ToString(' ');
        }
    }
}