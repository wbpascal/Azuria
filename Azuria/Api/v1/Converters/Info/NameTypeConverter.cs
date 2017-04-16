using System;
using Azuria.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Info
{
    internal class NameTypeConverter : DataConverter<MediaNameType>
    {
        #region Methods

        /// <inheritdoc />
        public override MediaNameType ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.Value.ToString())
            {
                case "name":
                    return MediaNameType.Original;
                case "nameeng":
                    return MediaNameType.English;
                case "nameger":
                    return MediaNameType.German;
                case "namejap":
                    return MediaNameType.Japanese;
                case "syn":
                    return MediaNameType.Synonym;
                default:
                    throw new InvalidOperationException();
            }
        }

        #endregion
    }
}