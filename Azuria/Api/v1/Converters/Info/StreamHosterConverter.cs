using System;
using System.Linq;
using Azuria.Enums.Info;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Info
{
    internal class StreamHosterConverter : DataConverter<StreamHoster[]>
    {
        #region Methods

        /// <inheritdoc />
        public override StreamHoster[] ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value.ToString()
                .Split(',')
                .Select(StreamHosterHelpers.GetFromString)
                .Where(hoster => hoster != StreamHoster.None)
                .ToArray();
        }

        #endregion
    }
}