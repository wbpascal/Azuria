using System;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class IntToBoolConverter : DataConverter<bool>
    {
        #region Methods

        /// <inheritdoc />
        public override bool ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Convert.ToInt32(reader.Value) == 1;
        }

        #endregion
    }
}