using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Enums.Info;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class GenreConverter : JsonConverter
    {
        #region Methods

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IEnumerable<Genre>);
        }

        public override object ReadJson(
            JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            string lValue = reader.Value.ToString();
            if (string.IsNullOrEmpty(lValue.Trim())) return new Genre[0];

            Dictionary<string, Genre> lStringDictionary = EnumHelpers.GetDescriptionDictionary<Genre>();
            return lValue.Split(' ')
                .Where(genre => lStringDictionary.ContainsKey(genre))
                .Select(genre => lStringDictionary[genre])
                .ToArray();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}