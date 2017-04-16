using System;
using Azuria.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class CategoryConverter : DataConverter<MediaEntryType>
    {
        #region Methods

        /// <inheritdoc />
        public override MediaEntryType ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return (MediaEntryType) Enum.Parse(typeof(MediaEntryType), reader.Value.ToString(), true);
        }

        #endregion
    }
}