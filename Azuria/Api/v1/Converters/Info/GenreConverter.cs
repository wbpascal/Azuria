using System;
using System.Linq;
using Azuria.AnimeManga;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Info
{
    internal class GenreConverter : JsonConverter
    {
        #region

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            string lValue = reader.Value as string;
            if (string.IsNullOrEmpty(lValue?.Trim())) return new GenreType[0];
            return (from genreString in lValue.Split(' ')
                where GenreHelper.StringToGenreDictionary.ContainsKey(genreString)
                select GenreHelper.StringToGenreDictionary[genreString]).ToList();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        #endregion
    }
}