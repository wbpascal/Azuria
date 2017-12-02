using System;
using System.Collections.Generic;
using Azuria.Enums.Info;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Info
{
    internal class LanguageCollectionConverter : DataConverter<MediaLanguage[]>
    {
        /// <inheritdoc />
        public override MediaLanguage[] ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            List<MediaLanguage> lLanguages = new List<MediaLanguage>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndArray) break;
                lLanguages.Add(LanguageHelpers.GetMediaLanguage(reader.Value.ToString()));
            }
            return lLanguages.ToArray();
        }
    }
}