using System;
using Azuria.Enums.Info;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Info
{
    internal class PersonTypeConverter : DataConverter<PersonType>
    {
        /// <inheritdoc />
        public override PersonType ConvertJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            switch (reader.Value.ToString())
            {
                case "author-art":
                    return PersonType.AuthorArt;
                case "original-creator":
                    return PersonType.OriginalCreator;
                default:
                    return EnumHelpers.ParseFromString(reader.Value.ToString(), PersonType.Misc);
            }
        }
    }
}