using System;
using Azuria.Media.Properties;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters
{
    internal class CountryConverter : JsonConverter
    {
        #region Methods
        
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Country);
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            switch (reader.Value.ToString())
            {
                case "de":
                    return Country.Germany;
                case "en":
                case "us":
                    return Country.EnglandUnitedStates;
                case "jp":
                    return Country.Japan;
                default:
                    return Country.Unkown;
            }
        }
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}