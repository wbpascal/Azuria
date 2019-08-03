using System;
using Azuria.Helpers.Extensions;

namespace Azuria.Api.v1.Input.Converter
{
    internal class GetDescriptionConverter : IInputDataConverter<Enum>
    {
        /// <inheritdoc />
        public string Convert(Enum toConvert)
        {
            return toConvert.GetDescription();
        }
    }
}