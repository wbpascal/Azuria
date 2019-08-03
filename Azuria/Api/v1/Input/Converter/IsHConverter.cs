namespace Azuria.Api.v1.Input.Converter
{
    internal class IsHConverter : IInputDataConverter<bool?>
    {
        /// <inheritdoc />
        public string Convert(bool? toConvert)
        {
            switch (toConvert)
            {
                case true:
                    return "1";
                case false:
                    return "-1";
                default:
                    return "0";
            }
        }
    }
}