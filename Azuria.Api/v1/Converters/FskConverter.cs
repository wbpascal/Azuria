using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Api.Enums.Info;
using Azuria.Api.Helpers;
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

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            string lValue = reader.Value.ToString();
            if (string.IsNullOrEmpty(lValue.Trim())) return new Fsk[0];
            return (from fskString in lValue.Split(' ')
                where FskHelpers.StringToFskDictionary.ContainsKey(fskString)
                select FskHelpers.StringToFskDictionary[fskString]).ToArray();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}