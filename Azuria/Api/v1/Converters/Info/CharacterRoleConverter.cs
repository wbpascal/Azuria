using System;
using Azuria.Enums.Info;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Info
{
    /// <summary>
    /// 
    /// </summary>
    public class CharacterRoleConverter : DataConverter<CharacterRole>
    {
        /// <inheritdoc />
        public override CharacterRole ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return EnumHelpers.ParseFromString<CharacterRole>(reader.Value.ToString());
        }
    }
}