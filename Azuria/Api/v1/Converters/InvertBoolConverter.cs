using System;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    /// <summary>
    /// </summary>
    public class InvertBoolConverter : DataConverter<bool>
    {
        /// <inheritdoc />
        public override bool ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return !Convert.ToBoolean(reader.Value);
        }
    }
}