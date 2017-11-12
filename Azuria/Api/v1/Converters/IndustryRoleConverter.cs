using System;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class IndustryRoleConverter : DataConverter<IndustryRole>
    {
        /// <inheritdoc />
        public override IndustryRole ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string lValue = reader.Value.ToString();
            switch (lValue)
            {
                case "streaming":
                    return IndustryRole.Streaming;
                case "record_label":
                    return IndustryRole.RecordLabel;
                case "talent_agent":
                    return IndustryRole.TalentAgent;
                default:
                    return (IndustryRole) Enum.Parse(typeof(IndustryRole), lValue, true);
            }
        }
    }
}