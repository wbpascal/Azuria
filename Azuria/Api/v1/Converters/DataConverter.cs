using System;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    /// <summary>
    /// </summary>
    public abstract class DataConverter<T> : JsonConverter, IDataConverter
    {
        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }

        /// <summary>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public abstract T ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer);

        /// <inheritdoc />
        public override object ReadJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return this.ConvertJson(reader, objectType, existingValue, serializer);
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}