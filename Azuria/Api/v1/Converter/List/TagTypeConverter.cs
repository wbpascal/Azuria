using System;
using System.Collections.Generic;
using Azuria.Enums.List;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converter.List
{
    internal class TagTypeConverter : DataConverter<TagType?>
    {
        public override TagType? ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var lTagType = reader.Value?.ToString();
            return ParseTagType(lTagType);
        }

        public static TagType? ParseTagType(string tagType)
        {
            Dictionary<string, TagType> lStringDictionary = EnumHelpers.GetDescriptionDictionary<TagType>();
            return tagType != null && lStringDictionary.ContainsKey(tagType)
                ? lStringDictionary[tagType]
                : (TagType?) null;
        }
    }
}