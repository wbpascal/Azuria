using System;
using System.Collections.Generic;
using Azuria.Api.v1.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Info
{
    internal class LanguageConverter : JsonConverter
    {
        #region

        /// <summary>
        ///     Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        ///     <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
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
            List<AnimeMangaLanguage> lLanguages = new List<AnimeMangaLanguage>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndArray) break;
                switch (reader.Value.ToString())
                {
                    case "de":
                        lLanguages.Add(AnimeMangaLanguage.German);
                        break;
                    case "en":
                        lLanguages.Add(AnimeMangaLanguage.English);
                        break;
                    case "gersub":
                        lLanguages.Add(AnimeMangaLanguage.GerSub);
                        break;
                    case "gerdub":
                        lLanguages.Add(AnimeMangaLanguage.GerDub);
                        break;
                    case "engsub":
                        lLanguages.Add(AnimeMangaLanguage.EngSub);
                        break;
                    case "engdub":
                        lLanguages.Add(AnimeMangaLanguage.EngDub);
                        break;
                }
            }
            return lLanguages.ToArray();
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