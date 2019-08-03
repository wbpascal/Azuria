using System;
using Azuria.Enums.Info;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class LanguageConverter : DataConverter<Language>
    {
        /// <inheritdoc />
        public override Language ConvertJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            return LanguageHelpers.GetLanguageFromIdentifier(reader.Value.ToString());
        }
    }
}