using System;
using System.Collections.Generic;
using Azuria.Enums.List;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.List
{
    internal class TagSubtypeConverter : DataConverter<TagSubtype>
    {
        public override TagSubtype ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var lSubtype = reader.Value?.ToString();
            Dictionary<string, TagSubtype> lStringDictionary = EnumHelpers.GetDescriptionDictionary<TagSubtype>();
            return lStringDictionary.ContainsKey(lSubtype)
                ? lStringDictionary[lSubtype]
                : throw new InvalidOperationException($"'{lSubtype}' is not a valid subtype!");
        }
    }
}