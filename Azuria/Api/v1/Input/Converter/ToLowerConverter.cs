namespace Azuria.Api.v1.Input.Converter
{
    internal class ToLowerConverter : IInputDataConverter<object>
    {
        /// <inheritdoc />
        public string Convert(object toConvert)
        {
            return toConvert?.ToString().ToLowerInvariant();
        }
    }
}