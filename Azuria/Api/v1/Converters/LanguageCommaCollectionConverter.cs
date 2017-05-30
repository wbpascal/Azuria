using System;
using System.Linq;
using Azuria.Enums.Info;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class LanguageCommaCollectionConverter : DataConverter<MediaLanguage[]>
    {
        /// <inheritdoc />
        public override MediaLanguage[] ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value.ToString()
                .Split(',')
                .Select(LanguageHelpers.GetMediaLanguage)
                .Where(language => language != MediaLanguage.Unkown)
                .ToArray();
        }
    }
}