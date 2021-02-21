using System;
using System.Collections.Generic;
using Azuria.Enums.List;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.List
{
    internal class TagSubtypeConverter : DataConverter<TagSubtype?>
    {
        public override TagSubtype? ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var lSubtype = reader.Value?.ToString();
            return ParseSubtype(lSubtype);
        }

        public static TagSubtype? ParseSubtype(string subtype)
        {
            Dictionary<string, TagSubtype> lStringDictionary = EnumHelpers.GetDescriptionDictionary<TagSubtype>();
            return subtype != null && lStringDictionary.ContainsKey(subtype)
                ? lStringDictionary[subtype]
                : (TagSubtype?) null;
        }
    }
}