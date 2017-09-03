using System;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class IndustryDataModel : IndustryBasicDataModel
    {
        /// <summary>
        /// </summary>
        public Uri CoverImage => new Uri($"https://cdn.proxer.me/industry/{this.Id}.jpg");

        /// <summary>
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("link")]
        public Uri Link { get; set; }
    }
}