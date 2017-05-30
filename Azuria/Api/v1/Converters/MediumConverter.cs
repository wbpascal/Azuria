using System;
using Azuria.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class MediumConverter : DataConverter<MediaMedium>
    {
        /// <inheritdoc />
        public override MediaMedium ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.Value.ToString())
            {
                case "animeseries":
                    return MediaMedium.Animeseries;
                case "movie":
                    return MediaMedium.Movie;
                case "ova":
                    return MediaMedium.Ova;
                case "hentai":
                    return MediaMedium.Hentai;
                case "mangaseries":
                    return MediaMedium.Mangaseries;
                case "oneshot":
                    return MediaMedium.OneShot;
                case "doujin":
                    return MediaMedium.Doujin;
                case "hmanga":
                    return MediaMedium.HManga;
            }
            return MediaMedium.None;
        }
    }
}