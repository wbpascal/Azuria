using System;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class UnixToDateTimeConverter : DataConverter<DateTime>
    {
        /// <inheritdoc />
        public override DateTime ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return DateTimeHelpers.UnixTimeStampToDateTime(Convert.ToInt64(reader.Value));
        }
    }
}