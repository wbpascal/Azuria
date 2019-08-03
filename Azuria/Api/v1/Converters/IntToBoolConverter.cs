using System;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class IntToBoolConverter : DataConverter<bool>
    {
        /// <inheritdoc />
        public override bool ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return new IntToNullableBoolConverter().ConvertJson(reader, objectType, existingValue, serializer) ?? false;
        }
    }
}