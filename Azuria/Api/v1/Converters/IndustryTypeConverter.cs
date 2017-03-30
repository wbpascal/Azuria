using System;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class IndustryTypeConverter : JsonConverter
    {
        #region Methods

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IndustryType);
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            string lValue = reader.Value.ToString();
            switch (lValue)
            {
                case "streaming":
                    return IndustryType.Streaming;
                case "record_label":
                    return IndustryType.RecordLabel;
                case "talent_agent":
                    return IndustryType.TalentAgent;
                default:
                    return Enum.Parse(typeof(IndustryType), lValue, true);
            }
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }

        #endregion
    }
}