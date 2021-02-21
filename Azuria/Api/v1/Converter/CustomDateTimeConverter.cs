using System;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converter
{
    internal class CustomDateTimeConverter : DataConverter<DateTime>
    {
        /// <inheritdoc />
        public override DateTime ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return DateTime.Parse(reader.Value.ToString());
        }
    }
}