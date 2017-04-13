using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Enums.Info;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class FskConverter : JsonConverter
    {
        #region Methods

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IEnumerable<Fsk>);
        }

        public override object ReadJson(
            JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            string lValue = reader.Value.ToString();
            if (string.IsNullOrEmpty(lValue.Trim())) return new Fsk[0];
            return lValue.Split(' ')
                .Where(fskString => FskHelpers.StringToFskDictionary.ContainsKey(fskString))
                .Select(fskString => FskHelpers.StringToFskDictionary[fskString])
                .ToArray();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}