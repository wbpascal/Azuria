using System;
using System.Linq;
using Azuria.Enums.Info;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Info
{
    internal class StreamHosterConverter : DataConverter<string[]>
    {
        /// <inheritdoc />
        public override string[] ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value.ToString()
                .Split(',')
                .ToArray();
        }
    }
}