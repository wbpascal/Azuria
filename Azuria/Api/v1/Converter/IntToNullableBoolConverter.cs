using System;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converter
{
    internal class IntToNullableBoolConverter : DataConverter<bool?>
    {
        /// <inheritdoc />
        public override bool? ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (int.TryParse(reader.Value.ToString(), out int result))
                return result == 1;
            return null;
        }
    }
}