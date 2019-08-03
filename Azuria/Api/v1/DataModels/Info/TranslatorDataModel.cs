using System;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class TranslatorDataModel : TranslatorBasicDataModel
    {
        /// <summary>
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

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
        [JsonProperty("image")]
        public Uri Image { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("link")]
        public Uri Link { get; set; }
    }
}