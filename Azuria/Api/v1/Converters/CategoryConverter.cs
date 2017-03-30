using System;
using Azuria.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class CategoryConverter : JsonConverter
    {
        #region Methods

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(MediaEntryType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            return Enum.Parse(typeof(MediaEntryType), (string) reader.Value, true);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString().ToLowerInvariant());
        }

        #endregion
    }
}