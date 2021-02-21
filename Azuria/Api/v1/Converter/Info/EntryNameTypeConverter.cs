using System;
using Azuria.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converter.Info
{
    internal class EntryNameTypeConverter : DataConverter<MediaNameType>
    {
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
                case "namekor":
                    return MediaNameType.Korean;
                case "namezhn":
                    return MediaNameType.Chinese;
                case "syn":
                    return MediaNameType.Synonym;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}