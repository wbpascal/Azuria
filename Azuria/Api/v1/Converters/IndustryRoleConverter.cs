using System;
using Azuria.Enums.Info;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class IndustryRoleConverter : DataConverter<IndustryType>
    {
        /// <inheritdoc />
        public override IndustryType ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.Value.ToString())
            {
                case "streaming":
                    return IndustryType.Streaming;
                case "record_label":
                    return IndustryType.RecordLabel;
                case "talent_agent":
                    return IndustryType.TalentAgent;
                default:
                    return EnumHelpers.ParseFromString(reader.Value.ToString(), IndustryType.Misc);
            }
        }
    }
}