using System;
using Azuria.Api.v1.Converter;
using Newtonsoft.Json;

namespace Azuria.Test.Serilization
{
    public class TestConverter : DataConverter<int>
    {
        /// <inheritdoc />
        public override int ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return 42;
        }
    }
}