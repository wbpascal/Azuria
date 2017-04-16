using System;
using Azuria.Enums.Info;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class LanguageConverter : DataConverter<MediaLanguage>
    {
        #region Methods

        /// <inheritdoc />
        public override MediaLanguage ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return LanguageHelpers.GetMediaLanguage(reader.Value.ToString());
        }

        #endregion
    }
}