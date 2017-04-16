using System;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Info
{
    internal class IsLicensedConverter : DataConverter<bool?>
    {
        #region Methods

        /// <inheritdoc />
        public override bool? ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (Convert.ToInt32(reader.Value))
            {
                case 1:
                    return false;
                case 2:
                    return true;
                default:
                    return null;
            }
        }

        #endregion
    }
}