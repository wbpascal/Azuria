using System;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public class StringCommaCollectionConverter : DataConverter<string[]>
    {
        /// <inheritdoc />
        public override string[] ConvertJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value.ToString().Split(',');
        }
    }
}