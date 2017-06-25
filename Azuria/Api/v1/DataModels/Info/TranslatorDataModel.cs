using System;
using Azuria.Api.v1.Converters;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class TranslatorDataModel : IDataModel
    {
        /// <summary>
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("country")]
        [JsonConverter(typeof(CountryConverter))]
        public Country Country { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("cprojects")]
        public int CProjects { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("image")]
        public Uri Image { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("link")]
        public Uri Link { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}