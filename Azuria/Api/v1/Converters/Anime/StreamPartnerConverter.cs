using System;
using Azuria.Media.Properties;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Anime
{
    internal class StreamPartnerConverter : JsonConverter
    {
        #region Methods

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(StreamHoster);
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            switch (reader.Value.ToString())
            {
                case "clipfish-extern":
                    return StreamHoster.Clipfish;
                case "crunchyroll_en":
                    return StreamHoster.Crunchyroll;
                case "novamov":
                    return StreamHoster.Auroravid;
                case "Proxer-Stream":
                    return StreamHoster.ProxerStream;
                case "streamcloud2":
                    return StreamHoster.Streamcloud;
                default:
                    return Enum.Parse(typeof(StreamHoster), reader.Value.ToString(), true);
            }
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }

        #endregion
    }
}