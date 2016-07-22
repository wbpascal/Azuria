using System;
using System.Linq;
using Azuria.AnimeManga;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Info
{
    internal class FskConverter : JsonConverter
    {
        #region

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            string lValue = reader.Value.ToString();
            if (string.IsNullOrEmpty(lValue?.Trim())) return new FskType[0];
            return (from fskString in lValue.Split(' ')
                where FskHelper.StringToFskDictionary.ContainsKey(fskString)
                select FskHelper.StringToFskDictionary[fskString]).ToList();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        #endregion
    }
}