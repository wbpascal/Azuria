using System;
using Azuria.Media.Properties;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class LanguageConverter : JsonConverter
    {
        #region Methods

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

        internal static AnimeMangaLanguage GetLanguageFromString(string input)
        {
            switch (input)
            {
                case "de":
                    return AnimeMangaLanguage.German;
                case "en":
                    return AnimeMangaLanguage.English;
                case "gersub":
                    return AnimeMangaLanguage.GerSub;
                case "gerdub":
                    return AnimeMangaLanguage.GerDub;
                case "engsub":
                    return AnimeMangaLanguage.EngSub;
                case "engdub":
                    return AnimeMangaLanguage.EngDub;
            }
            return AnimeMangaLanguage.Unkown;
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
            return GetLanguageFromString(reader.Value.ToString());
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