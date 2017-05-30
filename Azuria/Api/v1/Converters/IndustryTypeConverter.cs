using System;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class IndustryTypeConverter : DataConverter<IndustryType>
    {
        /// <inheritdoc />
        public override IndustryType ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
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
                    return (IndustryType) Enum.Parse(typeof(IndustryType), lValue, true);
            }
        }
    }
}