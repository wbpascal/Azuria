using System;
using Azuria.Media.Properties;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Info
{
    internal class PublisherTypeConverter : JsonConverter
    {
        #region Methods

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            string lValue = reader.Value.ToString();
            switch (lValue)
            {
                case "streaming":
                    return Industry.IndustryType.StreamPartner;
                default:
                    return Enum.Parse(typeof(Industry.IndustryType), lValue, true);
            }
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }

        #endregion
    }
}