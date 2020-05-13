using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Enums.Info;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class FskConverter : DataConverter<Fsk[]>
    {
        /// <inheritdoc />
        public override Fsk[] ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var lValue = reader.Value.ToString();
            if (string.IsNullOrEmpty(lValue.Trim())) return new Fsk[0];

            Dictionary<string, Fsk> lStringDictionary = EnumHelpers.GetDescriptionDictionary<Fsk>();
            return lValue.Split(' ')
                .Where(fskString => lStringDictionary.ContainsKey(fskString))
                .Select(fskString => lStringDictionary[fskString])
                .ToArray();
        }
    }
}