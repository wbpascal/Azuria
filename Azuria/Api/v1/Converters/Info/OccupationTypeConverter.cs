using System;
using Azuria.Enums.Info;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Info
{
    internal class OccupationTypeConverter : DataConverter<Occupation>
    {
        /// <inheritdoc />
        public override Occupation ConvertJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            return EnumHelpers.ParseFromString(reader.Value.ToString(), Occupation.Misc);
        }
    }
}