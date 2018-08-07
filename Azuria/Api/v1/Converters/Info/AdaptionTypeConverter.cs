using System;
using System.Collections.Generic;
using Azuria.Enums.Info;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Info
{
    internal class AdaptionTypeConverter : DataConverter<AdaptionType?>
    {
        /// <inheritdoc />
        public override AdaptionType? ConvertJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string lValue = reader.Value.ToString();
            Dictionary<string, AdaptionType> lDescriptionDictionary =
                EnumHelpers.GetDescriptionDictionary<AdaptionType>();
            switch (lValue)
            {
                case null:
                case "":
                    return null;
                default:
                    return lDescriptionDictionary.ContainsKey(lValue)
                               ? lDescriptionDictionary[lValue]
                               : (AdaptionType?) null;
            }
        }
    }
}