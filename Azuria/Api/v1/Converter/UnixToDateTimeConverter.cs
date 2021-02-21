using System;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converter
{
    internal class UnixToDateTimeConverter : DataConverter<DateTime>
    {
        /// <inheritdoc />
        public override DateTime ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return DateTimeHelpers.UnixTimeStampToDateTime(Convert.ToUInt64(reader.Value));
        }
    }
}