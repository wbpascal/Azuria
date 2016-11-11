using System;
using System.Collections.Generic;
using Azuria.Media.Properties;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Info
{
    internal class StreamHosterConverter : JsonConverter
    {
        #region Methods

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IEnumerable<StreamHoster>);
        }

        /// <summary>Reads the JSON representation of the object.</summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            List<StreamHoster> lStreamPartners = new List<StreamHoster>();
            foreach (string streamPartner in reader.Value.ToString().Split(','))
                switch (streamPartner)
                {
                    case "mp4upload":
                        lStreamPartners.Add(StreamHoster.Mp4Upload);
                        break;
                    case "crunchyroll_de":
                    case "crunchyroll_en":
                        lStreamPartners.Add(StreamHoster.Crunchyroll);
                        break;
                    case "dailymotion":
                        lStreamPartners.Add(StreamHoster.Dailymotion);
                        break;
                    case "proxer-stream":
                        lStreamPartners.Add(StreamHoster.ProxerStream);
                        break;
                    case "streamcloud2":
                        lStreamPartners.Add(StreamHoster.Streamcloud);
                        break;
                    case "viewster":
                        lStreamPartners.Add(StreamHoster.Viewster);
                        break;
                    case "yourupload":
                        lStreamPartners.Add(StreamHoster.YourUpload);
                        break;
                }
            return lStreamPartners.ToArray();
        }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }

        #endregion
    }
}