using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Media.Properties;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class GenreConverter : JsonConverter
    {
        #region Methods

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IEnumerable<GenreType>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            string lValue = reader.Value.ToString();
            if (string.IsNullOrEmpty(lValue.Trim())) return new GenreType[0];
            return (from genreString in lValue.Split(' ')
                where GenreHelper.StringToGenreDictionary.ContainsKey(genreString)
                select GenreHelper.StringToGenreDictionary[genreString]).ToList();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}