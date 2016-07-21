using System;
using Azuria.Api.v1.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Info
{
    internal class AnimeMangaMediumConverter : JsonConverter
    {
        #region

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString().ToLowerInvariant());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            switch (reader.Value as string)
            {
                case "animeseries":
                    return AnimeMangaMedium.Animeseries;
                case "movie":
                    return AnimeMangaMedium.Movie;
                case "ova":
                    return AnimeMangaMedium.Ova;
                case "hentai":
                    return AnimeMangaMedium.Hentai;
                case "mangaseries":
                    return AnimeMangaMedium.Mangaseries;
                case "oneshot":
                    return AnimeMangaMedium.OneShot;
                case "doujin":
                    return AnimeMangaMedium.Doujin;
                case "hmanga":
                    return AnimeMangaMedium.HManga;
            }
            return AnimeMangaMedium.Unkown;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        #endregion
    }
}