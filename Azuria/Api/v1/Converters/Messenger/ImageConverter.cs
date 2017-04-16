using System;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Messenger
{
    internal class ImageConverter : DataConverter<Uri>
    {
        #region Methods

        /// <inheritdoc />
        public override Uri ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string lValue = reader.Value.ToString();
            return lValue.Contains(":")
                       ? new Uri($"{ApiConstants.ProxerAvatarCdnUrl}/{lValue.Split(':')[1]}")
                       : null;
        }

        #endregion
    }
}