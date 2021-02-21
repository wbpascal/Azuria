using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Enums.Info;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converter
{
    internal class GenreConverter : DataConverter<Genre[]>
    {
        /// <inheritdoc />
        public override Genre[] ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var lValue = reader.Value.ToString();
            return ParseGenre(lValue);
        }

        public static Genre[] ParseGenre(string value)
        {
            if (string.IsNullOrEmpty(value.Trim())) return new Genre[0];

            Dictionary<string, Genre> lStringDictionary = EnumHelpers.GetDescriptionDictionary<Genre>();
            return value.Split(' ')
                .Where(genre => lStringDictionary.ContainsKey(genre))
                .Select(genre => lStringDictionary[genre])
                .ToArray();
        }
    }
}