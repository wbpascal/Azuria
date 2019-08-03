using System;
using Azuria.Enums;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class CategoryConverter : DataConverter<MediaEntryType>
    {
        /// <inheritdoc />
        public override MediaEntryType ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return EnumHelpers.ParseFromString<MediaEntryType>(reader.Value.ToString());
        }
    }
}