using System;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Info
{
    internal class CharacterGenderConverter : DataConverter<CharacterGender?>
    {
        private readonly CharacterGender? _defaultGender;

        public CharacterGenderConverter()
            : this(null)
        {
        }

        public CharacterGenderConverter(CharacterGender? defaultGender)
        {
            this._defaultGender = defaultGender;
        }

        /// <inheritdoc />
        public override CharacterGender? ConvertJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            switch (reader.Value.ToString())
            {
                case "f":
                    return CharacterGender.Female;
                case "m":
                    return CharacterGender.Male;
                default:
                    return this._defaultGender;
            }
        }
    }
}