using System;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Anime
{
    internal class StreamPartnerConverter : DataConverter<StreamHoster>
    {
        /// <inheritdoc />
        public override StreamHoster ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
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
                    return (StreamHoster) Enum.Parse(typeof(StreamHoster), reader.Value.ToString(), true);
            }
        }
    }
}