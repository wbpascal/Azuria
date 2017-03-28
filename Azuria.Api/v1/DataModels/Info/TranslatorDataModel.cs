using System;
using Azuria.Api.Enums.Info;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class TranslatorDataModel : IDataModel
    {
        #region Properties

        /// <summary>
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("country")]
        [JsonConverter(typeof(CountryConverter))]
        public Country Country { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("link")]
        public Uri Link { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("image")]
        public Uri Image { get; set; }

        #endregion
    }
}