using System;
using Azuria.Enums.Info;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class IndustryRoleConverter : DataConverter<IndustryRole>
    {
        /// <inheritdoc />
        public override IndustryRole ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.Value.ToString())
            {
                case "streaming":
                    return IndustryRole.Streaming;
                case "record_label":
                    return IndustryRole.RecordLabel;
                case "talent_agent":
                    return IndustryRole.TalentAgent;
                default:
                    return EnumHelpers.ParseFromString(reader.Value.ToString(), IndustryRole.Misc);
            }
        }
    }
}