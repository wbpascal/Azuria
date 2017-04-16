using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Enums.Info;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Info
{
    internal class LanguageCollectionConverter : DataConverter<MediaLanguage[]>
    {
        #region Methods

        /// <inheritdoc />
        public override MediaLanguage[] ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            List<MediaLanguage> lLanguages = new List<MediaLanguage>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndArray) break;
                lLanguages.AddRange(
                    reader.Value.ToString().Split(',').Select(LanguageHelpers.GetMediaLanguage)
                );
            }
            return lLanguages.ToArray();
        }

        #endregion
    }
}