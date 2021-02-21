using System;
using System.Collections.Generic;
using Azuria.Enums.Info;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converter.Info
{
    internal class AdaptionTypeConverter : DataConverter<AdaptionType?>
    {
        /// <inheritdoc />
        public override AdaptionType? ConvertJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var lValue = reader.Value.ToString();
            return ParseAdaptionType(lValue);
        }

        public static AdaptionType? ParseAdaptionType(string type)
        {
            Dictionary<string, AdaptionType> lDescriptionDictionary =
                EnumHelpers.GetDescriptionDictionary<AdaptionType>();
            return type != null && lDescriptionDictionary.ContainsKey(type)
                ? lDescriptionDictionary[type]
                : (AdaptionType?)null;
        }
    }
}